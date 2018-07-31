using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Carroll.Data.Services.Models.MongoModels;

namespace Carroll.Data.Services.Controllers
{
    public class MongoServiceController : ApiController
    {
        IMongoClient _client;      
        IMongoDatabase _db;

        public MongoServiceController()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _db = _client.GetDatabase("DynamicForms");
        }

        public bool AuthenticateUser(string User,string Pass)
        {
            //var res = Query<SiteUser>.EQ(p=>p.Email,User);
            //return _db.GetCollection<Product>("Products").FindOne(res);

            var usercollection = _db.GetCollection<SiteUser>("SiteUsers");

            var count = usercollection.Find(u => u.Email == User && u.Password == Pass).ToList().Count;
            if (count == 1)
                return true;
            else
             return  false;
        }

        [ActionName("Index")]
        [HttpGet]
        public string Index()
        {
            var res= _db.GetCollection<BsonDocument>("Forms").ToJson();
            return res;
        }
        public string GetAllForms()
        {
            return _db.GetCollection<Form>("SiteUsers").ToJson();
        }

        public string GetForm(ObjectId Id)
        {
            var filter = Builders<Form>.Filter.Eq("_id", Id);
            return _db.GetCollection<Form>("Forms").Find(filter).ToJson();
        }

            public string CreateForm(Form f)
        {
            _db.GetCollection<Form>("Forms").InsertOne(f);
            return _db.GetCollection<Form>("Forms").ToJson();
        }

        public string UpdateForm(ObjectId Id,Form f)
        {
            f.Id = Id;
            var res = Query<Form>.EQ(pd => pd.Id, Id);
           // var operation = Update<Form>.Replace(f);
           
            var filter = Builders<Form>.Filter.Eq("Id", Id);

            var update = Builders<Form>.Update.Set(f.FormName.ToString(), f.FormName);
            update = Builders<Form>.Update.Set(f.FormNamePlural.ToString(), f.FormNamePlural);
            update = Builders<Form>.Update.Set(f.ModifiedBy.ToString(), f.CreatedBy);
            update = Builders<Form>.Update.Set(f.ModifiedDate.ToString(), DateTime.Now);

            _db.GetCollection<Form>("Forms").UpdateOne(filter, update);            
            return _db.GetCollection<Form>("Forms").ToJson();
        }

    }
}
