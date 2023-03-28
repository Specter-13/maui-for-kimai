using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Persistence;
public class FavouriteTimesheetService : IFavouritesTimesheetService
{
     private SQLiteAsyncConnection _db;

    async Task Init()
    {
        if (_db is not null)
            return;

         var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.FAVOURITES_DB_NAME);
        _db = new SQLiteAsyncConnection(dbPath);
        var result = await _db.CreateTableAsync<TimesheetFavouriteEntity>();
    }

    public async Task<TimesheetFavouriteEntity> Create(TimesheetFavouriteEntity entity)
    {
        await Init();
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
        var numOfDeleteTRows = await _db.Table<TimesheetFavouriteEntity>().DeleteAsync(x=> x.Id == id);
        return numOfDeleteTRows > 0;
    }

    public async Task<ICollection<TimesheetFavouriteEntity>> GetAll()
    {
        await Init();
        return await _db.Table<TimesheetFavouriteEntity>().ToListAsync();
    }

    public async Task<TimesheetFavouriteEntity> Read(int id)
    {
        await Init();
        return await _db.GetAsync<TimesheetFavouriteEntity>(id);
    }

    public async Task<TimesheetFavouriteEntity> Update(TimesheetFavouriteEntity entity)
    {
        await Init();
        var numOfUpdatedTRows = await _db.UpdateAsync(entity);
        if (numOfUpdatedTRows > 0)
        {
            return entity;
        }
        return null;
    }
}
