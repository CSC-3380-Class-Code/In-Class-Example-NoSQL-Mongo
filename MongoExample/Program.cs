using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoExampleDocs;

var client = new MongoClient("mongodb://localhost:27017");
var database = client.GetDatabase("Grades");
IMongoCollection<Person> collection = database.GetCollection<Person>("Students");

async Task populateTable(){
	await collection.InsertOneAsync(new Person { Name = "Jack" });
	await collection.InsertOneAsync(new Person { Name = "Jill" });
	await collection.InsertOneAsync(new Person { Name = "Stacy" });
	await collection.InsertOneAsync(new Person { Name = "John" });
	await collection.InsertOneAsync(new Person { Name = "Gerald" });
	await collection.InsertOneAsync(new Person { Name = "David" });
	await collection.InsertOneAsync(new Person { Name = "Zacharias" });
	await collection.InsertOneAsync(new Person { Name = "Sam" });
}

for(int i = 0; i < 20; i++){
	await populateTable();
}

var list = await collection.Find(person => person.Name == "Jack")
	.ToListAsync();

foreach(var person in list)
{
	Console.WriteLine(person.ToBsonDocument());
}

Console.WriteLine("====================================");

list = await collection.Find(x => true)
	.ToListAsync();

foreach(var person in list)
{
	Console.WriteLine(person.ToBsonDocument());
}

Console.WriteLine("====================================");

var filter = Builders<Person>.Filter
    .Eq(person => person.Grade, 0.0);

Random rng = new Random();

var update = Builders<Person>.Update
    .Set(person => person.Grade, 100*rng.NextDouble());

await collection.UpdateManyAsync(filter, update);

list = await collection.Find(x => true)
	.ToListAsync();

foreach(var person in list)
{
	Console.WriteLine(person.ToBsonDocument());
}

Console.WriteLine("============J names with good grades============");

var filterMaker = Builders<Person>.Filter;
filter = filterMaker.And(
			filterMaker.Regex(person => person.Name, "^J"),
			filterMaker.Gte(person => person.Grade, 70)
		);

list = await collection.Find(filter).ToListAsync();

foreach(var person in list)
{
	Console.WriteLine(person.ToBsonDocument());
}

Console.WriteLine("===========Repopulate==============");

for(int i = 0; i < 20; i++){
	await populateTable();
}

list = collection.Find(filter).ToList();

foreach(var person in list)
{
	Console.WriteLine(person.ToBsonDocument());
}

Console.WriteLine("==========Better RNG=============");

filter = Builders<Person>.Filter
    .Eq(person => person.Grade, 0.0);

while(await collection.Find(filter).CountDocumentsAsync() > 0){
	update = Builders<Person>.Update
    	.Set(person => person.Grade, 100*rng.NextDouble());
	await collection.UpdateOneAsync(filter, update);
}

list = await collection.Find(x => true).ToListAsync();

foreach(var person in list)
{
	Console.WriteLine(person.ToBsonDocument());
}

Console.WriteLine("=========J names with good grades again========");

filterMaker = Builders<Person>.Filter;
filter = filterMaker.And(
			filterMaker.Regex(person => person.Name, "^J"),
			filterMaker.Gte(person => person.Grade, 70)
		);

list = collection.Find(filter).ToList();

foreach(var person in list)
{
	Console.WriteLine(person.ToBsonDocument());
}