using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingApp.Entity
{
    public class SessionUser
    {
        public int SessionUserId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int SessionId { get; set; }
        public Session Session { get; set; }
        public decimal Cash { get; set; }
        public decimal CashOut { get; set; }
        public decimal TotalCash { get; set; }
    }
}
