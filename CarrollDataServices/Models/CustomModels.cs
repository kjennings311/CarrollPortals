using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carroll.Data.Services.Models
{
   
    public class TokenInput
    {
        public string id { get; set; }
        public string name { get; set; }

        public bool ReadOnly { get; set; }
    }

    public class KeyValuePair
    {
        public string Key;
        public string Value;
        public KeyValuePair(string key, string value)
        {
            Value = value;
            Key = key;
        }
    }

}