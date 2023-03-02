using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WomsAuth.Models;

namespace WomsAuth.Database
{
    public class AuthDatabase
    {
        SQLiteAsyncConnection Database;

        public AuthDatabase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            _ = await Database.CreateTableAsync<AuthModel>();
        }

        public async Task<List<AuthModel>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<AuthModel>().ToListAsync();
        }

        public async Task<int> SaveItemAsync(AuthModel item)
        {
            await Init();

            return await Database.InsertOrReplaceAsync(item);
        }

        public async Task<int> DeleteItemAsync(AuthModel item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
