using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwnersPets.Models
{
    public class UserDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PetsCount { get; set; }
    }
}