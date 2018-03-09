using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LotteryBacktest
{
    public class MongoDAO
    {
        protected static IMongoClient _client = new MongoClient("mongodb://121.42.139.20:27017");
        protected static IMongoDatabase _database = _client.GetDatabase("local");

        public void SaveRealData(string expect, string Picked, int winnumber, bool whetherwin, decimal balance, int bet)
        {
            //_client = new MongoClient("mongodb://121.42.139.20:27017");
            //_database = _client.GetDatabase("local");

            var document = new BsonDocument
            {
                { "Time", DateTime.Now },
                { "Expect", expect },
                { "Picked", Picked },
                { "WinNumber", winnumber },
                { "WhetherWin", whetherwin },
                { "Balance", Convert.ToInt32(balance) },
                { "Bet", bet }
            };

            var collection = _database.GetCollection<BsonDocument>("RealData");
            collection.InsertOneAsync(document);
        }

        public void updateStopLoss(string expectName)
        {
            var collection = _database.GetCollection<BsonDocument>("RealData");
            var filter = Builders<BsonDocument>.Filter.Eq("Expect", expectName);
            var update = Builders<BsonDocument>.Update.Set("StopLoss", true);
            var result = collection.UpdateOneAsync(filter, update);
        }

        public void test()
        {
            var document = new BsonDocument
            {
                { "borough", "Manhattan" },
                { "cuisine", "Italian" },
                { "name", "Vella" },
                { "restaurant_id", "41704620" }
            };

            var collection = _database.GetCollection<BsonDocument>("New");
            collection.InsertOneAsync(document);
        }
    }
}
