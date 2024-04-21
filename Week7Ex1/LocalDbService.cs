using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week7Ex1
{
    public class LocalDbService
    {
        private const string DB_NAME = "demo_local_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory,DB_NAME));
            _connection.CreateTableAsync<Customer>();
        
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await _connection.Table<Customer>().ToListAsync();
        }
        public async Task<Customer> GetById(int id)
        {
            return await _connection.Table<Customer>().Where(x=>x.Id==id).FirstOrDefaultAsync();
        }
        public async Task Create(Customer customer)
        {
            await _connection.InsertAsync(customer);
        }

        public async Task<bool> CreateOrUpdate(Customer customer)
        {
            try
            {
                if (customer.Id == 0)
                    await _connection.InsertAsync(customer);
                else
                    await _connection.UpdateAsync(customer);
                return true;
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(Customer customer)
        {
            try
            {
                await _connection.DeleteAsync(customer);
                return true;
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }


    }
}
