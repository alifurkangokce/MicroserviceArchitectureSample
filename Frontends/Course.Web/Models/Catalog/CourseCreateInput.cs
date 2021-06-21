using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Course.Web.Models.Catalog
{
    public class CourseCreateInput
    {
        [Display(Name = "Kurs ismi")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Kurs fiyatı")]
        [Required]
        public decimal Price { get; set; }
        [Display(Name = "Kurs açıklama")]
        [Required]
        public string Description { get; set; }

        public string UserId { get; set; }
        public FeatureViewModel Feature { get; set; }
        [Display(Name = "Kurs Kategori")]
        [Required]
        public string CategoryId { get; set; }
        [Display(Name = "Resim")]
        public string Picture { get; set; }
        [Display(Name = "Kurs Resmi")]
        public IFormFile PhotoFormFile { get; set; }
    }
}
