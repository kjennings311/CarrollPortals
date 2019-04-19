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
        FormGeneralLiabilityClaim=9,
        AllClaims=10,
        PayPeriods=11,
        CarrollPositions=12

    }

    public enum DFieldType
    {
        IsText=0,
        IsDate=1,
        IsPerson=2,
        IsPic=3
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
        public string label { get; set; }
        public DFieldType type { get; set; }
        public string href { get; set; }
    }

    public class ClaimDetails
    {
        public dynamic Claim { get; set; }
        public dynamic Comments { get; set; }
        public dynamic Attchments { get; set; }
        public dynamic Activity { get; set; }
    }

    public class PrintClaimDetails
    {
        public dynamic ClaimDetails { get; set; }
        public dynamic PropertyDetails { get; set; }
    }

    public class RecordUpdateResult
    {
        public string RecordId { get; set; }
        public bool Succeded { get; set; }
    }

    public class EmailParams
    {
        public EmailParams()
        {
            logo = "carroll.jpg";
            signature = "<p> Pavan Nanduri <br>  Vice President, Information Systems <br> Carroll Organization <br> pavan.nanduri@carrollorg.com <br> 3340 Peachtree Rd NE - Suite 2250 <br> Atlanta, GA  30326 <br> Office: 404 - 812 - 8298<br> Cell: 770 - 508 - 5050 <br> Fax: 404 - 806 - 4312 <br> www.CarrollOrganization.com <br> <br>";
            fromemail = "sekhar.babu@forcitude.com";
            Company = "Carroll Organisation";
            mailfooterhtml = "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
            mailstart = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <table >  ";
            
        }

        public string logo { get; set; }
        public string signature { get; set; }
        public string fromemail { get; set; }
        public string Company { get; set; }
        public string mailfooterhtml { get; set; }
        public string mailbody { get; set; }
        public string mailstart { get; set; } 
           }

}
