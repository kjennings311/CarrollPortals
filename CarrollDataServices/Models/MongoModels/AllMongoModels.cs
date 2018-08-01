using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [BsonId]
        public ObjectId Id { get; set; }
        [Required]
        [BsonElement("FormName")]
        [Display(Name ="Form Name")]
        public BsonString FormName { get; set; }
        [Required]
        [BsonElement("FormNamePlural")]
        [Display(Name = "Form Name Plural")]
        public BsonString FormNamePlural { get; set; }
        [BsonElement("CreatedBy")]
        public BsonString CreatedBy { get; set; }
        [BsonElement("CreatedDate")]
        public BsonDateTime CreatedDate { get; set; }
        [BsonElement("ModifiedBy")]
        public BsonString ModifiedBy { get; set; }
        [BsonElement("ModifiedDate")]
         public BsonDateTime ModifiedDate { get; set; }
      

    }
}