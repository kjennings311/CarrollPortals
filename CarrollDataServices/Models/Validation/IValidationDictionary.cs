using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace Carroll.Data.Services.Models.Validation
{

    public interface IValidationDictionary 
    {
        void AddError(string key, string errorMessage);
        bool IsValid { get;  }

        ModelStateDictionary GetUnderlyingDictionary { get; }
      
    }
}
