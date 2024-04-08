using Dapper;
using Dapper.Transaction;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Application.Repositories;
using Microsoft.Data.SqlClient;

namespace LibraryManagement.Data.Repositories.DapperAndADO
{
    public class DapperMediaRepository : IMediaRespository
    {
        private string _cs;

        public DapperMediaRepository(string connectionString)
        {
            _cs = connectionString;
        }

        public int Add(Media media)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = @"INSERT INTO Media (MediaTypeID, Title, IsArchived) 
                        VALUES (@MediaTypeID, @Title, @IsArchived)";

                var parameters = new
                {
                    media.MediaTypeID,
                    media.Title,
                    media.IsArchived
                };

                cn.Execute(sql, parameters);

                var sqlID = "SELECT MediaID FROM Media WHERE Title = @Title;";
                var idParam = new
                {
                    media.Title
                };

                return cn.QuerySingle<int>(sqlID, idParam);
            }
        }

        public void ArchiveMedia(Media media)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = @"UPDATE Media SET IsArchived = 1 WHERE MediaID = @MediaID AND IsArchived = 0";

                var parameters = new
                {
                    media.MediaID
                };

                cn.ExecuteScalar(sql, parameters);
            }
        }
        public bool IsCheckedOut(int id)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = @"SELECT * FROM CheckoutLog WHERE MediaID = @MediaID AND ReturnDate IS NULL";

                var parameters = new
                {
                    MediaID = id
                };

                return cn.QueryFirstOrDefault<CheckoutLog>(sql, parameters) != null ? true : false;
            }
        }

        public void Edit(Media media)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = @"UPDATE Media SET Title = @Title, MediaTypeID = @MediaTypeID WHERE MediaID = @MediaID";

                var parameters = new
                {
                    media.MediaID,
                    media.Title,
                    media.MediaTypeID
                };

                cn.ExecuteScalar(sql, parameters);
            }
        }

        public List<Media> GetAll()
        {
            using (var cn = new SqlConnection(_cs))
            {
                List<Media> media = [];

                var sql = new SqlCommand(@"
                SELECT m.MediaID, m.Title, m.IsArchived, mt.MediaTypeName
                FROM Media m 
                    INNER JOIN MediaType mt ON m.MediaTypeID = mt.MediaTypeID", cn);

                cn.Open();

                using (var dr = sql.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Media item = new Media();

                        item.MediaID = (int)dr["MediaID"];
                        item.Title = (string)dr["Title"];
                        item.IsArchived = (bool)dr["IsArchived"];
                        item.MediaType.MediaTypeName = (string)dr["MediaTypeName"];

                        media.Add(item);
                    }
                }
                cn.Close();

                return media;
            }
        }

        public List<Media> GetArchive()
        {
            using (var cn = new SqlConnection(_cs))
            {
                List<Media> media = [];

                var sql = new SqlCommand(@"
                SELECT m.MediaID, m.Title, m.IsArchived, mt.MediaTypeName
                FROM Media m 
                    INNER JOIN MediaType mt ON m.MediaTypeID = mt.MediaTypeID
                WHERE IsArchived = 1
                ORDER BY mt.MediaTypeName, m.Title;", cn);

                cn.Open();

                using (var dr = sql.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Media item = new Media();

                        item.MediaID = (int)dr["MediaID"];
                        item.Title = (string)dr["Title"];
                        item.IsArchived = (bool)dr["IsArchived"];
                        item.MediaType.MediaTypeName = (string)dr["MediaTypeName"];

                        media.Add(item);
                    }
                }
                cn.Close();

                return media;
            }
        }

        public List<Media> GetByType(Media type)
        {
            using (var cn = new SqlConnection(_cs))
            {
                List<Media> media = [];

                var sql = new SqlCommand(@"SELECT * FROM Media m 
            INNER JOIN MediaType mt ON m.MediaTypeID = mt.MediaTypeID 
            WHERE m.MediaTypeID = @MediaTypeID", cn);

                sql.Parameters.AddWithValue("@MediaTypeID", type.MediaTypeID);

                cn.Open();

                using (var dr = sql.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Media item = new Media();

                        item.MediaID = (int)dr["MediaID"];
                        item.Title = (string)dr["Title"];
                        item.MediaType.MediaTypeName = (string)dr["MediaTypeName"];
                        item.IsArchived = (bool)dr["IsArchived"];

                        media.Add(item);
                    }
                }
                cn.Close();

                return media;
            }
        }

        public Media? GetItem(int id)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = $"SELECT * FROM Media WHERE MediaID = @ID";

                var parameter = new
                {
                    ID = id
                };
                return cn.QuerySingleOrDefault<Media>(sql, parameter);
            }
        }

        public List<Media> GetNonArchivedMedia(Media type)
        {
            using (var cn = new SqlConnection(_cs))
            {
                List<Media> media = [];

                var sql = new SqlCommand(@"SELECT * FROM Media m 
            INNER JOIN MediaType mt ON m.MediaTypeID = mt.MediaTypeID 
            WHERE m.MediaTypeID = @MediaTypeID AND IsArchived = 0", cn);

                sql.Parameters.AddWithValue("@MediaTypeID", type.MediaTypeID);

                cn.Open();

                using (var dr = sql.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Media item = new Media();

                        item.MediaID = (int)dr["MediaID"];
                        item.Title = (string)dr["Title"];
                        item.MediaType.MediaTypeName = (string)dr["MediaTypeName"];
                        item.IsArchived = (bool)dr["IsArchived"];

                        media.Add(item);
                    }
                }
                cn.Close();

                return media;
            }
        }

        public Dictionary<string, int> Top3CheckedOutItems()
        {
            using (var cn = new SqlConnection(_cs))
            {
                Dictionary<string, int> top3 = [];

                var sql = new SqlCommand(
                    @"SELECT TOP 3 cl.MediaID, Title, COUNT(cl.MediaID) AS NumOfCheckOuts
                FROM CheckoutLog cl
                    INNER JOIN Media m ON cl.MediaID = m.MediaID
                GROUP BY cl.MediaID, Title
                ORDER BY NumOfCheckOuts DESC;", cn);

                cn.Open();

                using (var dr = sql.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string title = "";
                        int checkouts = 0;

                        title = (string)dr["Title"];
                        checkouts = (int)dr["NumOfCheckOuts"];

                        top3.Add(title, checkouts);
                    }
                }
                cn.Close();

                return top3;
            }
        }
    }
}
