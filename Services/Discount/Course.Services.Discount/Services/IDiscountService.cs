﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Shared.Dtos;

namespace Course.Services.Discount.Services
{
    public interface IDiscountService
    {
        Task<Response<List<Models.Discount>>> GetAll();
        Task<Response<Models.Discount>> GetById(int Id);
        Task<Response<NoContent>> Save(Models.Discount discount);
        Task<Response<NoContent>> Update(Models.Discount discount);
        Task<Response<NoContent>> Delete(int Id);
        Task<Response<Models.Discount>> GetByCodeAndUserId(string code,string userId);

    }
}
