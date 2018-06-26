using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Web.Script.Serialization;
using Carroll.Data.Services.Models.Validation;
using System.Web.Http.ModelBinding;
using Carroll.Data.Services.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Reflection;


namespace Carroll.Data.Services.Helpers
{
    public sealed class Utility
    {

        #region [ Work around to pass error details to the validateFilter ]
        public static string Serialize(object obj)
        {
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return oSerializer.Serialize(obj);
        }
        public static IValidationDictionary DeSerialize(string obj)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();

            IValidationDictionary _ret = jss.Deserialize<IValidationDictionary>(obj);
            return _ret;
        }
        #endregion

        public static List<Error> GetValidationErrors(IValidationDictionary _modelState)
        {
            ModelStateDictionary _model = _modelState.GetUnderlyingDictionary;
            //var errors = _model.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray();
           // List<Error> _err = new List<Error>();
            var result = from ms in _model
                         where ms.Value.Errors.Any()
                         let fieldKey = ms.Key
                         let errors = ms.Value.Errors
                         from error in errors
                         select new Error(fieldKey, error.ErrorMessage);

          
            //http://jnye.co/Posts/23/return-all-modelstate-errors-with-field-keys-as-json-data-in-mvc
            return result.ToList();
        }

       public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static string SerializeObject(object dataToSerialize)
        {
            if (dataToSerialize == null) return null;

            using (System.IO.StringWriter stringwriter = new System.IO.StringWriter())
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(dataToSerialize.GetType());
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
        }

        public static T DeserializeObject<T>(string xmlText)
        {
            if (String.IsNullOrWhiteSpace(xmlText)) return default(T);

            using (System.IO.StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }

        /// <summary>
        ///  Gives option to lookup class property by property name.. ex: property.nAME = PROPERTY["NAME"]
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(object Object, string propertyName)
        {
            return Object.GetType().GetProperties()
               .Single(pi => pi.Name == propertyName)
               .GetValue(Object, null);
        }

        #region [ Helpers ]
        public static List<Error> CheckModelValid(IValidationDictionary _modelState)
        {
            if (!_modelState.IsValid)
            {

                return Utility.GetValidationErrors(_modelState);

            }
            return null;

        }

        public static HttpResponseMessage ReturnRecordResponse(IValidationDictionary modelState,bool bSucceded, string OptionalData = "")
        {
            List<Error> Errors = CheckModelValid(modelState);
            if (Errors != null)
            {
                // return Request.CreateResponse(HttpStatusCode.BadRequest, new {Errors});
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var sJsonText = oSerializer.Serialize(Errors);
                var resp = new HttpResponseMessage()
                {
                    Content = new StringContent(sJsonText),
                    StatusCode = HttpStatusCode.BadRequest
                };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return resp;


            }
            else if (!bSucceded)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(
                        "There were Errors processing your request. Please try back later.",
                        Encoding.UTF8,
                        "application/json"
                    ),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                string sReturnContent = "OK";
                if (OptionalData != string.Empty)
                {
                    sReturnContent = OptionalData;
                }

                return new HttpResponseMessage()
                {
                    Content = new StringContent(
                       sReturnContent,
                        Encoding.UTF8,
                        "application/json"
                    ),
                    StatusCode = HttpStatusCode.Created
                };
            }

        }

        public static bool ValidateFormData(IValidationDictionary _validationDictionary,  List<FormField> FormData)
        {
            foreach (FormField _field in FormData)
            {
                if ((_field.Required) && (string.IsNullOrEmpty(_field.FieldValue.Trim())))
                //          if("a" == "a")
                {
                    _validationDictionary.AddError(_field.FieldName, string.Concat(_field.FieldLabel, " is required."));
                }
                else if (!string.IsNullOrEmpty(_field.ValidationExpressionServerSide))
                {
                    if (!string.IsNullOrEmpty(_field.FieldValue.Trim()))
                    {
                        if (!Regex.IsMatch(_field.FieldValue.Trim(), _field.ValidationExpressionServerSide))
                        {
                            _validationDictionary.AddError(_field.FieldName, string.Concat(_field.FieldLabel, " is invalid format."));
                        }
                    }

                }

                // Also let's make sure we validate dropdown options aswell. 
                switch (_field.FieldValidationType)
                {
                    case FieldValidationTypes.Email:
                        if (!string.IsNullOrEmpty(_field.FieldValue.Trim()))
                        {
                            if (!Regex.IsMatch(_field.FieldValue.Trim(), @"[\w-]+@([\w-]+\.)+[\w-]+"))
                            {
                                _validationDictionary.AddError(_field.FieldName, string.Concat(_field.FieldLabel, " is invalid format."));
                            }
                        }

                        break;
                    default:
                        break;
                }
            }
            return _validationDictionary.IsValid;
        }

        public static Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().
                   SingleOrDefault(assembly => assembly.GetName().Name == name);
        }

        #endregion
    }
}
