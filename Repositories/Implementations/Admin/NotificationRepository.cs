using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Implementations
{


    public class NotificationRepository : INotificationInterface
    {
        private readonly NpgsqlConnection _conn;

        public NotificationRepository(NpgsqlConnection conn)
        {
            _conn = conn;
        }

        public async Task<List<t_notification>> GetAll()
        {
            List<t_notification> notifications = new List<t_notification>();
            try
            {
                if (_conn.State != ConnectionState.Open)
                {
                    _conn.Open();
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand(@"Select * from t_notifications", _conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notifications.Add(new t_notification
                            {
                                c_notification_id = reader.GetInt32(0),
                                c_title_name = reader.GetString(1),
                                c_title_description = reader.GetString(2),
                                c_receiver = reader.GetString(3),
                                c_notification_date = reader.GetDateTime(4)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error In notification repository : " + ex.Message);
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
            return notifications;
        }

        public async Task<int> Add(t_notification data)
        {
            int status = 0;
            try
            {
                if (_conn.State != ConnectionState.Open)
                {
                    _conn.Open();
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand(@"Insert Into t_notifications (c_title_name,c_title_description,c_receiver,c_notification_date) values (@c_title_name,@c_title_description,@c_receiver,@c_notification_date)", _conn))
                {
                    cmd.Parameters.AddWithValue("@c_title_name", data.c_title_name);
                    cmd.Parameters.AddWithValue("@c_title_description", data.c_title_description);
                    cmd.Parameters.AddWithValue("@c_receiver", data.c_receiver);
                    cmd.Parameters.AddWithValue("@c_notification_date", data.c_notification_date);
                    status = cmd.ExecuteNonQuery();
                    status = 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error In notification repositry : " + ex.Message);
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
            return status;
        }

        public async Task<int> Delete(int id)
        {
            int status = 0;
            try
            {
                if (_conn.State != ConnectionState.Open)
                {
                    _conn.Open();
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand(@"Delete from t_notifications where c_notification_id=@c_notification_id", _conn))
                {
                    cmd.Parameters.AddWithValue("@c_notification_id", id);
                    status = cmd.ExecuteNonQuery();
                    status = 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error In notification repository : " + ex.Message);
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
            return status;
        }
    }
}