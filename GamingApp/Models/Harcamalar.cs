using GamingApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamingApp.Models
{
    public class Harcamalar
    {
        public int UserId { get; set; }
        public string KullaniciAdSoyad { get; set; }
        public int OrganizasyonId { get; set; }
        public int KategoriId { get; set; }
        public string KategoriAdi { get; set; }
        public decimal Tutar { get; set; }
        public decimal alcakVercek { get; set; }

    }
}