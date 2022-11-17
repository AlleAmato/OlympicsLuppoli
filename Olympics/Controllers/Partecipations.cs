using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Olympics.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;

namespace Olympics.Controllers
{
    internal static class Partecipations
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        }
        public static List<string> GetAllSex()
        {
            return GetAllStrings("SELECT DISTINCT Sex FROM Athletes ORDER BY 1");
        }
        public static List<string> GetAllGames()
        {
            return GetAllStrings("SELECT DISTINCT Games FROM Athletes ORDER BY 1");
        }
        public static List<string> GetAllSports(string games)
        {
            if (string.IsNullOrEmpty(games) || string.IsNullOrWhiteSpace(games)) return new List<string>();

            return GetAllStrings("SELECT DISTINCT Sport FROM Athletes WHERE Games=@games ORDER BY 1",new SqlParameter("@games",games));
        }
        public static List<string> GetAllEvents(string games, string sport)
        {
            if (string.IsNullOrEmpty(sport) || string.IsNullOrWhiteSpace(sport) || string.IsNullOrEmpty(games) || string.IsNullOrWhiteSpace(games)) return new List<string>();

            return GetAllStrings("SELECT DISTINCT Event FROM Athletes WHERE Games=@games AND Sport=@sport ORDER BY 1",new SqlParameter("@games",games),new SqlParameter("@sport", sport));
        }
        public static List<string> GetAllMedals()
        {
            return GetAllStrings("SELECT DISTINCT Medal FROM Athletes Where Medal IS NOT NULL ORDER BY 1");
        }
        public static List<string> GetAllStrings(string q, SqlParameter p1 = null, SqlParameter p2 = null)
        { 
            List<string> retVal = new List<string>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(q, connection);
                    if (p1 != null) cmd.Parameters.Add(p1);
                    if (p2 != null) cmd.Parameters.Add(p2);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            retVal.Add(reader.GetString(0));
                        }
                    }
                    return retVal;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        internal static PaginatedList<Partecipation> GetPartecipations(string nameFilter, string sexFilter, string gamesFilter, string sportFilter, string eventFilter, string medalFilter, int page, int pageSize)
        {
            List<Partecipation> data = new List<Partecipation>();
            int totalCount = 0;

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                try
                {
                    connection.Open();

                    //Filtri
                    List<string> filters = new List<string>();
                    if (nameFilter != null) filters.Add("Name LIKE @nameFilter");
                    if (sexFilter != null) filters.Add("Sex = @sexFilter");
                    if (gamesFilter != null) filters.Add("Games = @gamesFilter");
                    if (sportFilter != null) filters.Add("Sport = @sportFilter");
                    if (eventFilter != null) filters.Add("Event = @eventFilter");
                    if (medalFilter != null) filters.Add("Medal = @medalFilter");

                    //Prelevo i dati
                    if (page <= 1) page = 1;
                    int offset = (page - 1)*pageSize;
                    string q = $"SELECT * FROM Athletes ";
                    if (filters.Count > 0)
                    {
                        q += " WHERE " + string.Join(" AND ", filters);
                    }
                    q += " ORDER BY Id OFFSET @offset ROWS FETCH NEXT @pageSize  ROWS ONLY";

                    SqlCommand cmd = new SqlCommand(q, connection);
                    cmd.Parameters.AddWithValue("@nameFilter", nameFilter==null ? "" : $"%{nameFilter}%");
                    cmd.Parameters.AddWithValue("@sexFilter", sexFilter ?? "");
                    cmd.Parameters.AddWithValue("@gamesFilter", gamesFilter ?? "");
                    cmd.Parameters.AddWithValue("@sportFilter", sportFilter ?? "");
                    cmd.Parameters.AddWithValue("@eventFilter", eventFilter ?? "");
                    cmd.Parameters.AddWithValue("@medalFilter", medalFilter ?? "");
                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data.Add(new Partecipation
                            {
                                Id = (long)reader["Id"],
                                IdAthlete = (long)reader["IdAthlete"],
                                Name = (string)reader["Name"],
                                Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? null : (int?)reader["Age"],
                                Height = reader.IsDBNull(reader.GetOrdinal("Height")) ? null : (int?)reader["Height"],
                                Weight = reader.IsDBNull(reader.GetOrdinal("Weight")) ? null : (int?)reader["Weight"],
                                Sex = (string)reader["Sex"],
                                NOC = (string)reader["NOC"],
                                Games = (string)reader["Games"],
                                Season = (string)reader["Season"],
                                City = (string)reader["City"],
                                Year = reader.IsDBNull(reader.GetOrdinal("Year")) ? null : (int?)reader["Year"],
                                Sport = (string)reader["Sport"],
                                Event = (string)reader["Event"],
                                Medal = reader.IsDBNull(reader.GetOrdinal("Medal")) ? "" : (string)reader["Medal"],
                            });
                        }
                    }

                    //Conto le righe totali
                    q = $"SELECT COUNT(*) FROM Athletes ";
                    if (filters.Count > 0)
                    {
                        q += " WHERE " + string.Join(" AND ", filters);
                    }
                    cmd.CommandText = q;
                    totalCount = (int)cmd.ExecuteScalar();

                    return new PaginatedList<Partecipation>(totalCount, data);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
