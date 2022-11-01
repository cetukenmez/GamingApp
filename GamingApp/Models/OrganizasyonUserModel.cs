using GamingApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamingApp.Models
{
    public class OrganizasyonUserModel
    {
        public int Id { get; set; }
        public List<User> SeciliOyuncular { get; set; }
    }
}