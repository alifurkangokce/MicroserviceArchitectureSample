using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Course.Web.Models;
using Course.Web.Models.Catalog;
using Course.Web.Services.Interfaces;

namespace Course.Web.Services
{
    public class CatalogService:ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<CourseViewModel> GetByCourseId(string courseId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCourseAsync(string courseId)
        {
            throw new NotImplementedException();
        }
    }
}
