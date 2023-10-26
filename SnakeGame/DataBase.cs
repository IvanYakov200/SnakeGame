using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using static System.Formats.Asn1.AsnWriter;

namespace SnakeConsole
{
    internal class DataBase
    {
        static public List<Users> LeaderBoard(SqliteConnection connection)
        {
            var Leaders = new List<Users>();
            int count = 0;
            string sqlExpression = "SELECT * FROM LeaderBoard ORDER BY Score DESC";
            string sqlExpressionCount = "SELECT * FROM LeaderBoard WHERE id!= 0";
            using (connection)
            {
                connection.Open();

                SqliteCommand command1 = new SqliteCommand(sqlExpressionCount, connection);
                using (SqliteDataReader reader = command1.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            count++;
                        }
                    }
                }

                SqliteCommand command2 = new SqliteCommand(sqlExpression, connection);
                if (count >= 5)
                {
                    using (SqliteDataReader reader = command2.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                reader.Read();
                                String Name = reader.GetString(1);
                                int Score = reader.GetInt32(2);

                                Leaders.Add(new Users(Name, Score));
                            }
                        }
                    }
                }
                else
                {
                    using (SqliteDataReader reader = command2.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            { 
                                String Name = reader.GetString(1);
                                int Score = reader.GetInt32(2);

                                Leaders.Add(new Users(Name, Score));
                            }
                        }
                    }
                }
            }
            return Leaders;
        }
        
        static public bool AddUser(SqliteConnection connection, string NickName, int Score)
        {
            if (string.IsNullOrEmpty(NickName))
            {
                string sqlExpression = $"INSERT INTO LeaderBoard (Name, Score) VALUES ('Player', {Score})";
                using (connection)
                {
                    connection.Open();

                    SqliteCommand command = new SqliteCommand(sqlExpression, connection);

                    command.ExecuteNonQuery();
                }
                return true;
            }
            else
            {
                string sqlExpression = $"INSERT INTO LeaderBoard (Name, Score) VALUES (:name, {Score})";
                using (connection)
                {
                    connection.Open();

                    SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                    command.Parameters.AddWithValue("name", NickName);

                    command.ExecuteNonQuery();
                }
                return true;
            }

            return false;
        }

    }
}
