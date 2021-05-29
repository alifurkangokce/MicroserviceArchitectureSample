using System;
using System.Collections.Generic;
using System.Text;
using Course.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Course.Shared.ControllerBases
{
    public class CustomBaseController:ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
