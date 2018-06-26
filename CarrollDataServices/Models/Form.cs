using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carroll.Data.Services.Models
{
    public class Form
    {
        public string FormName;
        public List<FormField> FormFields;
        public string FormPostUrl;
        
        // Fields below are used if we are loading data for edit mode
        public bool IsEditForm;
        public string DBFieldId;
        public int UserId;
        public string UserDisplayName;

    }
}