using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingApp.Entity
{
    public class Game
    {
        public int GameId { get; set; }
        public string GameName { get; set; }
        public DateTime CreateDate { get; set; }
        public int GameAdminUserId { get; set; }
        public int? OyunTuruId { get; set; }
        public OyunTuru OyunTuru { get; set; }
        public bool GameClosed { get; set; }
        public virtual ICollection<GameUser> GameUser { get; set; }
        public virtual ICollection<Session> Session { get; set; }
        public virtual ICollection<Logs> Logs { get; set; }

    }
}
