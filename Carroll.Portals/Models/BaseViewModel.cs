using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carroll.Portals.Models
{
    public class BaseViewModel
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string RoleType { get; set; }
        public string Name { get; set; }
        public string PropertyId { get; set; }
        public string AllowedProp {get; set;}
        public bool Isallowed { get; set; }
    }
}