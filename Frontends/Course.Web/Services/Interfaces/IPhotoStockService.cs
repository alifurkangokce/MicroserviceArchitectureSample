using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Web.Models.PhotoStocks;
using Microsoft.AspNetCore.Http;

namespace Course.Web.Services.Interfaces
{
   public  interface IPhotoStockService
   {
       Task<PhotoViewModel> UploadPhoto(IFormFile photo);
       Task<Boolean> DeletePhoto(string photoUrl);
   }
}
