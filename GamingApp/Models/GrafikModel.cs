using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingApp.Models
{
    public class GrafikModel
    {
        public string Tarih { get; set; }

        public double Toplam { get; set; }
        public string Foto { get; set; }
        public string DataTipi { get; set; }
    }


    public class HesapModel
    {

        public string İsim { get; set; }
        public decimal Ucret { get; set; }

    }

    public class SonucObj
    {

        public int sonuc_kod { get; set; }
        public string sonuc_mesaj { get; set; }

    }
}
