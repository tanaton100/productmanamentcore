﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductmanagementCore.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Password { get; set; }
        public IEnumerable<Products> Products { get; set; }
    }
}
