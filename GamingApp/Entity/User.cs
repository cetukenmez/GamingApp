using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GamingApp.Entity
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string UserNameSurname { get; set; }
        public string UserPhoto { get; set; }
        public virtual ICollection<GameUser> GameUser { get; set; }
        public virtual ICollection<SessionUser> SessionUser { get; set; }

    }
}
