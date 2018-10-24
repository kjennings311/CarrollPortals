using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Carroll.Data.Services.Models
{
    /// <summary>
    /// To create custom Forms from serverside..
    /// </summary>
    public class FormField
    {
        public string FieldLabel;
        public string FieldName;
        [JsonConverter(typeof(StringEnumConverter))]
        public FieldTypes FieldType;
        public bool Required;
        [JsonConverter(typeof(StringEnumConverter))]
        public FieldValidationTypes FieldValidationType;
        
        // This url must load keyvalue pairs or dictionary

        public string DataLoadUrl;
        public string FieldValue;
        public string ValidationExpressionServerSide;
        public string PopOverText;


    }

    public enum FieldTypes
    {
        Text,
        Select,
        Check,
        List,
        Person,
        Radio,
        LongText,
        Hidden,
        Password,
        File
    }
    public enum FieldValidationTypes
    {
        Email,
        Phone,
        PhoneWithExt,
        Number,
        DateTime,
        Decimal,
        Person,
        Currency,
        Hyperlink,
        None,
        Contact,
        TaxId,
        Password,
        Zip
    }

}