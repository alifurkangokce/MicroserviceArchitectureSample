using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Course.Shared.Dtos;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Course.Services.Discount.Services
{
    public class DiscountService:IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }


        public async  Task<Response<List<Models.Discount>>> GetAll()
        {
            var discount = await _dbConnection.QueryAsync<Models.Discount>("Select * from discount");
            return Response<List<Models.Discount>>.Success(discount.ToList(),200);
        }

        public async Task<Response<Models.Discount>> GetById(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("Select * from discount where id=@Id",new{Id=id})).SingleOrDefault();
            return discount==null ? Response<Models.Discount>.Fail("Discount Not Found",404) : Response<Models.Discount>.Success(discount,200);
        }

        public async Task<Response<NoContent>> Save(Models.Discount discount)
        {
            var saveStatus =
                await _dbConnection.ExecuteAsync(
                    "Insert into discount (userid,rate,code) values (@UserId,@Rate,@Code)",discount);
            return saveStatus>0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("An Error occurred While Adding",500);
        }

        public async Task<Response<NoContent>> Update(Models.Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync(
                "update discount set userid=@UserId,code=@Code,rate=@Rate where id=@Id", new
                {
                    UserId = discount.UserId,
                    Id = discount.Id,
                    Code = discount.Code,
                    Rate = discount.Rate
                });
            return status > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("Discount Not Found", 404);
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var status = await _dbConnection.ExecuteAsync("Delete from discount where id=@Id",new {Id = id});
            return status > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("Discount Not Found",404);
            
        }

        public async Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discount = await _dbConnection.QueryAsync<Models.Discount>(
                "Select * from discount where userid=@UserId and code=@Code", new
                {
                    UserId = userId,
                    Code = code

                });
            var hasDiscount = discount.FirstOrDefault();
            return hasDiscount == null
                ? Response<Models.Discount>.Fail("Discount Not Found", 404)
                : Response<Models.Discount>.Success(hasDiscount, 200);

        }
    }
}
