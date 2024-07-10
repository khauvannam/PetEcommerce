using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json;

namespace Product_API.Helpers;

public class BsonDocumentSerializer : SerializerBase<object>
{
    public override object Deserialize(
        BsonDeserializationContext context,
        BsonDeserializationArgs args
    )
    {
        var serializer = BsonSerializer.LookupSerializer(typeof(BsonDocument));
        var document = serializer.Deserialize(context, args);

        var bsonDocument = document.ToBsonDocument();

        var result = bsonDocument.ToJson();
        return JsonConvert.DeserializeObject<IDictionary<string, object>>(result)!;
    }

    public override void Serialize(
        BsonSerializationContext context,
        BsonSerializationArgs args,
        object value
    )
    {
        var jsonDocument = JsonConvert.SerializeObject(value);
        var bsonDocument = BsonSerializer.Deserialize<BsonDocument>(jsonDocument);

        var serializer = BsonSerializer.LookupSerializer(typeof(BsonDocument));
        serializer.Serialize(context, bsonDocument.AsBsonValue);
    }
}
