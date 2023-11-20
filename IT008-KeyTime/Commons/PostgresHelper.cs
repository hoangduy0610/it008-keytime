using Dapper;
using Dapper.Contrib.Extensions;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IT008_KeyTime.Commons
{
    internal class PostgresHelper
    {
        public static List<T> Query<T>(string statement) where T : class
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(Constants.PostgresConnection);
            var dataSource = dataSourceBuilder.Build();

            using (var connection = dataSource.OpenConnection())
            {

                try
                {
                    return connection.Query<T>(statement).ToList();
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }

        public static T QueryFirst<T>(string statement) where T : class
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(Constants.PostgresConnection);
            var dataSource = dataSourceBuilder.Build();

            using (var connection = dataSource.OpenConnection())
            {

                try
                {
                    return connection.QueryFirst<T>(statement);
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }

        public static T GetById<T>(int id) where T : class
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(Constants.PostgresConnection);
            var dataSource = dataSourceBuilder.Build();

            using (var connection = dataSource.OpenConnection())
            {

                try
                {
                    return connection.Get<T>(id);
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }

        public static List<T> GetAll<T>() where T : class
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(Constants.PostgresConnection);
            var dataSource = dataSourceBuilder.Build();

            using (var connection = dataSource.OpenConnection())
            {

                try
                {
                    return connection.GetAll<T>().ToList();
                }
                catch (Exception ex)
                {
                    return new List<T>();
                }

            }
        }

        public static long Insert<T>(T entityToInsert) where T : class
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(Constants.PostgresConnection);
            var dataSource = dataSourceBuilder.Build();

            using (var connection = dataSource.OpenConnection())
            {
                long re = 0;
                try
                {
                    re = connection.Insert(entityToInsert);
                }
                catch (Exception ex)
                {
                    re = 0;
                    MessageBox.Show(ex.Message);
                }
                return re;
            }
        }

        public static bool Update<T>(T entityToUpdate) where T : class
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(Constants.PostgresConnection);
            var dataSource = dataSourceBuilder.Build();

            using (var connection = dataSource.OpenConnection())
            {
                var result = false;
                try
                {
                    result = connection.Update(entityToUpdate);
                }
                catch (Exception ex)
                {
                    result = false;
                    MessageBox.Show(ex.Message);
                }
                return result;
            }
        }

        public static bool Delete<T>(T entityToDelete) where T : class
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(Constants.PostgresConnection);
            var dataSource = dataSourceBuilder.Build();

            using (var connection = dataSource.OpenConnection())
            {
                bool success = false;
                try
                {
                    success = connection.Delete(entityToDelete);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                return success;
            }
        }
    }
}
