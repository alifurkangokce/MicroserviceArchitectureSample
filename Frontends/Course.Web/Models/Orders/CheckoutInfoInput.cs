using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models.Orders
{
    public class CheckoutInfoInput
    {
        [Display(Name = "İl")]
        public string Province { get; set; }
        [Display(Name = "İlçe")]
        public string Distinct { get; set; }
        [Display(Name = "Cadde")]
        public string Street { get; set; }
        [Display(Name = "Posta Kodu")]
        public string ZipCode { get; set; }
        [Display(Name = "Adres")]
        public string Line { get; set; }
        [Display(Name = "Kart Adı")]
        public string CardName { get; set; }
        [Display(Name = "Kart No")]
        public string CardNumber { get; set; }
        [Display(Name = "Kart Son Kullanma Tarihi(ay/yil)")]
        public string Expiration { get; set; }
        [Display(Name = "Cvv")]
        public string CVV { get; set; }
        [Display(Name = "Toplam Fiyat")]
        public decimal TotalPrice { get; set; }

    }
}
