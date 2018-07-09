using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carroll.Data.Entities
{
    
    public enum EntityType
    {
        Property=0,
        Contact=1,
        Partner=2,
        User=3,
        Role=4,
        UserInRole = 5,
        UserInProperty = 6,
        FormPropertyDamageClaim=7,
        FormMoldDamageClaim=8,
        FormGeneralLiabilityClaim=9
    }

    public class Config
    {
        public string PkName { get; set; }
        public string EtType { get; set; }
        public List<DtableConfigArray> Columns { get; set; }
        public dynamic Rows { get; set; }
    }

    public class DtableConfigArray
    {
        public string name { get; set; }
        public string data { get; set; }
        public bool autoWidth { get; set; }
    }
}
