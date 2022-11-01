using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingApp.Entity
{
    public class Logs
    {
        public int LogsId { get; set; }
        public DateTime LogDate { get; set; }
        public string LogDesc { get; set; }
        public string UserName { get; set; }
        public string InsertTable { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public string IpAddress { get; set; }
    }
}
