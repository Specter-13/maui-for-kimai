using MauiForKimai.Core;
using MauiForKimai.Core.Entities;
using MauiForKimai.Interfaces;
using Microsoft.VisualBasic;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Persistence;
public class ServerService : IServerService
{
    private ApiStateProvider _asp;
    public ServerService(ApiStateProvider asp)
    {
        _asp = asp;
    }

    private SQLiteAsyncConnection _db;

    public async Task Init()
    {
        if (_db is not null)
            return;

        var name = $"maui_for_kimai_db_{_asp.ServerId}";
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), name);
        _db = new SQLiteAsyncConnection(dbPath);
        var result = await _db.CreateTableAsync<ServerEntity>();
    }

    public async Task<ServerEntity> Create(ServerModel model)
    {
        await Init();
        var entity = model.ToServerEntity();
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
        var entity = model.ToServerEntity();
        var numOfUpdatedTRows = await _db.UpdateAsync(entity);
        if (numOfUpdatedTRows > 0)
        {
            return entity;
        }
        return null;
    }

    public async Task<ServerEntity> GetDefaultServer()
    {
        await Init();

        return await _db.Table<ServerEntity>().FirstOrDefaultAsync(x=> x.IsDefault == true);
    
    }

    public async Task<ServerEntity> UnsetDefaultPropertyExceptOne(int id)
    {
        await Init();
        var servers = await _db.Table<ServerEntity>().ToListAsync();

        foreach (var server in servers) 
        {
            if(server.IsDefault == false || server.Id == id)
            { 
                continue;
            }

            server.IsDefault = false;
            await _db.UpdateAsync(server);
        }
        return await _db.Table<ServerEntity>().FirstOrDefaultAsync(x=> x.IsDefault == true);
    
    }
}
