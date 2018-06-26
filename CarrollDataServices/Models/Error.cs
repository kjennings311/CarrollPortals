using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carroll.Data.Services.Models
{
    [Serializable]
    public class Error
    {
        public Error(string Key, string Message)
        {
            this.Key = Key;
            this.Message = Message;
        }
        public string Key { get; set; }
        public string Message { get; set; }
    }
}
