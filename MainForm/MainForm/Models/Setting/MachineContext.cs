using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.Models.Setting
{
    public class MachineContext
    {
        public string ConnectionString { get; set; }

        public MachineContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

     /*   private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Machine> GetAllAlbums()
        {
            List<Machine> list = new List<Machine>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from wo.sys_machine", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Machine()
                        {
                            No = reader["ma_no"].ToString(),
                            GR1 = reader["gr_1"].ToString(),
                            GR2 = reader["gr_2"].ToString(),
                            GR3 = reader["gr_3"].ToString(),
                            GR4 = reader["gr_4"].ToString(),
                            GR5 = reader["gr_5"].ToString(),
                            Name = reader["ma_name"].ToString(),
                            Enable = (bool)reader["is_enable"],
                            C_By = reader["create_by"].ToString(),
                            C_Date = reader["create_date"].ToString(),
                            U_By = reader["last_update_by"].ToString(),
                            U_Date = reader["last_update_date"].ToString()
                        });
                    }
                }
            }
            return list;
        }*/
    }
}
