using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingApp.Models
{
    public class SeansModel
    {
        public List<SeansUserModel> UrunList { get; set; }
        public int SessionId { get; set; }


    }

    public class SeansUserModel
    {
        public int SESSIONUSERID { get; set; }
        public decimal ARTITUTARI { get; set; }

    }
}
