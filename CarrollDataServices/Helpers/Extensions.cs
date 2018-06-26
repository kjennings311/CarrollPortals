using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;

namespace Carroll.Data.Services.Helpers
{
    public static class Extensions
    {
        public static string AsString(this XmlDocument xmlDoc)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter tx = new XmlTextWriter(sw))
                {
                    xmlDoc.WriteTo(tx);
                    string strXmlText = sw.ToString();
                    return strXmlText;
                }
            }
        }

        public static void SetPropertyValue(this object p_object, string p_propertyName, object value)
        {
            PropertyInfo property = p_object.GetType().GetProperty(p_propertyName);
            Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            // Handle guid's if empty strings or nulls
            if (t.FullName == "System.Guid")
            {
                Guid? _guid = null;
                // READ VALUE TO STRING AND CONVERT TO guid
                try
                {
                    string sGuid = Convert.ToString(value);
                    _guid = new Guid(sGuid);
                }
                catch { }
                property.SetValue(p_object, _guid, null);
            }
            else if (t.FullName == "System.Boolean")
            {
                bool _bool = false;
                try
                {
                    string val = Convert.ToString(value);
                    if ((val.ToLower() == "on") || (val.ToLower() == "yes") || (val.ToLower() == "true")) _bool = true;


                }
                catch { }
                property.SetValue(p_object, _bool, null);
            }
            else if (t.FullName == "System.DateTime")
            {
                DateTime? dt = null;
                try
                {
                    string val = Convert.ToString(value);

                    dt = string.IsNullOrEmpty(val) ? (DateTime?)null : DateTime.Parse(val);
                }
                catch { }
                property.SetValue(p_object, dt, null);

            }
            else
            {
                object safeValue = (value == null) ? null : Convert.ChangeType(value, t);
                property.SetValue(p_object, safeValue, null);
            }
        }

        public static object GetPropertyValue(this object obj, string propName)
        {
            return obj.GetType().GetProperty(propName).GetValue(obj, null);
        }
    }
}



