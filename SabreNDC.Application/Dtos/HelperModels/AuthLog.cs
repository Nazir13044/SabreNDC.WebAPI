using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabreNDC.Application.Dtos.HelperModels;

public class AuthLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string token { get; set; }
    public string userName { get; set; }
    public DateTime validUntil { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool? IsLive { get; set; }
}
