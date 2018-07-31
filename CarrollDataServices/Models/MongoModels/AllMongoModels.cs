using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Carroll.Data.Services.Models.MongoModels
{
    public class SiteUser
    {
        
        public ObjectId Id { get; set; }
        [BsonElement("FirstName")]
        public string FirstName { get; set; }
        [BsonElement("LastName")]
        public string LastName { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }
        [BsonElement("Password")]
        public string Password { get; set; }
        [BsonElement("IsActive")]
        public bool IsActive { get; set; }
        [BsonElement("CreatedBy")]
        public string CreatedBy { get; set; }
        [BsonElement("CreatedDate")]
        public DateTime CreatedDate { get; set; }

    }

    public class Form
    {

        public ObjectId Id { get; set; }
        [BsonElement("FormName")]
        public string FormName { get; set; }
        [BsonElement("FormNamePlural")]
        public string FormNamePlural { get; set; }
        [BsonElement("CreatedBy")]
        public string CreatedBy { get; set; }
        [BsonElement("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [BsonElement("ModifiedBy")]
        public bool ModifiedBy { get; set; }
        [BsonElement("ModifiedDate")]
        public DateTime ModifiedDate { get; set; }
      

    }
}