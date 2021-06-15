﻿
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Course.Shared.Dtos;
using Course.Web.Models;
using Course.Web.Services.Interfaces;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Course.Web.Services
{
    public class IdentityService:IIdentityService
    {
        private readonly HttpClient _httpclient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient httpclient, IHttpContextAccessor httpContextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpclient = httpclient;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }


        public async Task<Response<bool>> SignIn(SigninInput input)
        {
            var disco = await _httpclient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.BaseUri,
                Policy = new DiscoveryPolicy{RequireHttps = false}
            });
            if (disco.IsError)
            {
                throw disco.Exception;
            }

            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = input.Email,
                Password = input.Password,
                Address = disco.TokenEndpoint
            };
            var token = await _httpclient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (token.IsError)
            {
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();
                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent,
                    new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
                return Response<bool>.Fail(errorDto.Errors,404);
            }

            var userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = disco.UserInfoEndpoint
            };
            var userInfo = await _httpclient.GetUserInfoAsync(userInfoRequest);
            if (userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims,CookieAuthenticationDefaults.AuthenticationScheme,
                "name","role");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name =OpenIdConnectParameterNames.AccessToken,Value = token.AccessToken},
                new AuthenticationToken{Name =OpenIdConnectParameterNames.RefreshToken,Value = token.RefreshToken},
                new AuthenticationToken{Name =OpenIdConnectParameterNames.ExpiresIn,Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("O",CultureInfo.InvariantCulture)},
            });
            authenticationProperties.IsPersistent = input.IsRemember;
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal, authenticationProperties);
            return Response<bool>.Success(200);


        }

        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            var disco = await _httpclient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.BaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });
            if (disco.IsError)
            {
                throw disco.Exception;
            }

            var refreshToken =
                await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            RefreshTokenRequest refreshTokenRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = disco.TokenEndpoint
            };
            var token = await _httpclient.RequestRefreshTokenAsync(refreshTokenRequest);
            if (token.IsError)
            {
                return null;
            }

            var authenticationTokens=new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name =OpenIdConnectParameterNames.AccessToken,Value = token.AccessToken},
                new AuthenticationToken{Name =OpenIdConnectParameterNames.RefreshToken,Value = token.RefreshToken},
                new AuthenticationToken{Name =OpenIdConnectParameterNames.ExpiresIn,Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("O",CultureInfo.InvariantCulture)},
            };
            var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();
            var properties = authenticationResult.Properties;
            properties.StoreTokens(authenticationTokens);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,authenticationResult.Principal,properties);

            return token;

        }

        public async Task RevokeRefreshToken()
        {
            var disco = await _httpclient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.BaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });
            if (disco.IsError)
            {
                throw disco.Exception;
            }

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                Address = disco.RevocationEndpoint,
                Token = refreshToken,
                TokenTypeHint = "refresh_token"
            };

            await _httpclient.RevokeTokenAsync(tokenRevocationRequest);

        }
    }
}
