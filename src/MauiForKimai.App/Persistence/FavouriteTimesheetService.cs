using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Persistence;
public class FavouriteTimesheetService
{
     private SQLiteAsyncConnection _db;

    async Task Init()
    {
        if (_db is not null)
            return;

         var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.FAVOURITES_DB_NAME);
        _db = new SQLiteAsyncConnection(dbPath);
        var result = await _db.CreateTableAsync<ServerEntity>();
    }

    public async Task<ServerEntity> Create(ServerModel model)
    {
        await Init();
        var entity = (ServerEntity) model;
        var numberOfaddedRows = await _db.InsertAsync(entity);
        if (numberOfaddedRows > 0)
        {
            return entity;
        }
        return null;
    }

    public async Task<bool> Delete(int id)
    {
        await Init();
        var numOfDeleteTRows = await _db.Table<ServerEntity>().DeleteAsync(x=> x.Id == id);
        return numOfDeleteTRows > 0;
    }

    public async Task<ICollection<ServerEntity>> GetAll()
    {
        await Init();
        return await _db.Table<ServerEntity>().ToListAsync();
    }

    public async Task<ServerEntity> Read(int id)
    {
        await Init();
        return await _db.GetAsync<ServerEntity>(id);
    }

    public async Task<ServerEntity> Update(ServerModel model)
    {
        await Init();
        var entity = (ServerEntity) model;
        var numOfUpdatedTRows = await _db.UpdateAsync(entity);
        if (numOfUpdatedTRows > 0)
        {
            return entity;
        }
        return null;
    }
}
