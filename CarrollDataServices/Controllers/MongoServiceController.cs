using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
//using MongoDB.Driver.Builders;
using Carroll.Data.Services.Models.MongoModels;
using System.Web.Http.Cors;

namespace Carroll.Data.Services.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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

        [ActionName("GetAllForms")]
        [HttpGet]
        public List<BsonDocument> GetAllForms()
        {
            var res = _db.GetCollection<BsonDocument>("Forms").Find(new BsonDocument()).ToListAsync();
            return res.Result;
        }
        //public dynamic GetAllForms1()
        //{
        //    var res =  _db.GetCollection<Form>("Forms").Find(new BsonDocument()).ToListAsync();
        //    return res.Result;
        //    //System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    //var sJsonText = oSerializer.Serialize(res.Result);
        //    //return sJsonText;
        //}
        
        [ActionName("GetForm")]
        [HttpGet]
        public dynamic GetForm(ObjectId Id)
        {
            var filter = Builders<Form>.Filter.Eq("_id", Id);
            return _db.GetCollection<Form>("Forms").Find(filter).ToListAsync().Result;
        }


        [ActionName("CEForm")]
        [HttpPost]
        public dynamic CEForm([FromBody]string f)
        {
            //IMongoCollection<BsonDocument> forms =  _db.GetCollection<BsonDocument>("Forms");

            //forms.InsertOneAsync(f);
            //return GetAllForms();

            string s = "1";
            return GetAllForms();
            //bool isNew = true;
            //try
            //{
            //    string s = f.Id.ToString();

            //}
            //catch { isNew = false; }
            //if (isNew)
            //{
            //    _db.GetCollection<Form>("Forms").InsertOne(f);
            //    //  return _db.GetCollection<Form>("Forms").Find(new BsonDocument()).ToListAsync().Result;
            //    return GetAllForms();
            //}
            //else
            //{
            //    var filter = Builders<Form>.Filter.Eq(s => s.Id, f.Id);
            //    _db.GetCollection<Form>("Forms").ReplaceOneAsync(filter, f);
            //    return GetAllForms();
            //}
        }




        [ActionName("CreateForm")]
        [HttpPost]
        public dynamic CreateForm(Form f)
        {
            f.CreatedDate = DateTime.Now;

            _db.GetCollection<Form>("Forms").InsertOne(f);
            return _db.GetCollection<Form>("Forms").Find(new BsonDocument()).ToListAsync().Result;
        }

        public dynamic UpdateForm(Form f)
        {
            //f.Id = new ObjectId(Id);
           // var res = Query<Form>.EQ(pd => pd.Id, f.Id);
           // var operation = Update<Form>.Replace(f);
           
            var filter = Builders<Form>.Filter.Eq(s=> s.Id,f.Id);
            _db.GetCollection<Form>("Forms").ReplaceOneAsync(filter, f);
           return GetAllForms();

        //    var update = Builders<Form>.Update.Set(f.FormName.ToString(), f.FormName);
        //    update = Builders<Form>.Update.Set(f.FormNamePlural.ToString(), f.FormNamePlural);
        //    update = Builders<Form>.Update.Set(f.ModifiedBy.ToString(), f.CreatedBy);
        //    update = Builders<Form>.Update.Set(f.ModifiedDate.ToString(), DateTime.Now);

            //    _db.GetCollection<Form>("Forms").UpdateOne(filter, update);            
            //    return _db.GetCollection<Form>("Forms").ToJson();
        }

    }
}
