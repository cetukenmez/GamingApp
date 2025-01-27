﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GamingApp.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName
        {
            get;
            set;
        }
        [Required]
        [DataType(DataType.Password)]
        public string Password
        {
            get;
            set;
        }
        public bool RememberLogin
        {
            get;
            set;
        }
        public string ReturnUrl
        {
            get;
            set;
        }
    }
}
