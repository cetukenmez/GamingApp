using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingApp.Entity
{
    public class Session
    {
        public int SessionId { get; set; }
        public string SessionName { get; set; }
        public Game Game { get; set; }
        public int GameId { get; set; }
        public bool SessionClosed { get; set; }
        public DateTime SessionTime { get; set; }
        public virtual ICollection<SessionUser> SessionUser { get; set; }

    }
}
