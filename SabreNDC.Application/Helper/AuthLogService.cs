using MongoDB.Driver;
using SabreNDC.Application.Dtos.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabreNDC.Application.Helper;

public class AuthLogService
{
    private readonly IMongoCollection<AuthLog> _authLogs;
    public AuthLogService(MDBSetting mdbSettings)
    {
        var mongoClient = new MongoClient(
        mdbSettings.Connection);

        var mongoDatabase = mongoClient.GetDatabase(
            mdbSettings.DatabaseName);

        _authLogs = mongoDatabase.GetCollection<AuthLog>("AuthLog");
    }
    public async Task<List<AuthLog>> GetAsync() =>
        await _authLogs.Find(_ => true).ToListAsync();

    public async Task<AuthLog?> GetAsync(string id) =>
        await _authLogs.Find(x => true).FirstOrDefaultAsync();

    public async Task<AuthLog?> GetLastAsync()
    {
        var result = await _authLogs.Find(x => true)
            .Sort(Builders<AuthLog>.Sort.Descending("_id"))
            .Limit(1).FirstOrDefaultAsync();
        return result;
    }

    public async Task<AuthLog?> GetLastWithUserNameAsync(string userName, bool isLive = false)
    {
        var result = await _authLogs.Find(x => x.userName == userName && x.IsLive == isLive)
            .Sort(Builders<AuthLog>.Sort.Descending("_id"))
            .Limit(1).FirstOrDefaultAsync();
        return result;
    }

    public async Task CreateAsync(AuthLog newBook) =>
        await _authLogs.InsertOneAsync(newBook);

    public async Task RemoveAsync(string id) =>
        await _authLogs.DeleteOneAsync(x => x.Id == id);
}
