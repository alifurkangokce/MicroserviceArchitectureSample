using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models.Catalog
{
    public class FeatureViewModel
    {
        [Display(Name = "Kalan Süre")]
        [Required]
        public int Duration { get; set; }
    }
}
