using MauiForKimai.Interfaces;

using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Services;
public class ServerService : IServerService<ServerModel>
{

	private readonly SQLiteAsyncConnection _db;
	public ServerService()
	{
		 var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mauiforkimai.db");
		 _db = new SQLiteAsyncConnection(dbPath);
		 _db.CreateTableAsync<ServerModel>();
	}


    public async Task<ServerModel> Create(ServerModel model)
    {
        var numberOfaddedRows = await _db.InsertAsync(model);
        if (numberOfaddedRows > 0)
        {
            return model;
        }
        return null; 
    }

    public async Task<bool> Delete(int id)
    {
        var numOfDeleteTRows = await _db.DeleteAsync(id);
        return numOfDeleteTRows > 0;
    }

    public async Task<ICollection<ServerModel>> GetAll()
    {
         return await _db.Table<ServerModel>().ToListAsync();
    }

    public async Task<ServerModel> Read(int id)
    {
        return await _db.GetAsync<ServerModel>(id);
    }

    public async Task<ServerModel> Update(ServerModel model)
    {
        var numOfUpdatedTRows = await _db.UpdateAsync(model);
        if (numOfUpdatedTRows > 0)
        {
            return model;
        }
        return null; 
    }
}
