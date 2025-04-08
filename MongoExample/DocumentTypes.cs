using MongoDB.Bson;

namespace MongoExampleDocs;
public class Person
{
    public ObjectId Id { get; set; }
    public required string Name { get; set; }
    public double Grade { get; set; }
}

