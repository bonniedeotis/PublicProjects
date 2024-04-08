using Dapper;
using Dapper.Transaction;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Application.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data.Repositories.DapperAndADO
{
    public class DapperCheckoutRepository : ICheckoutRepository
    {
        private string _cs;

        public DapperCheckoutRepository(string connectionString)
        {
            _cs = connectionString;
        }

        public List<CheckoutLog> AllCheckedOutItems()
        {
            using (var cn = new SqlConnection(_cs))
            {
                List<CheckoutLog> checkedOut = [];

                var sql = new SqlCommand(@"
                SELECT cl.MediaID, m.Title, cl.CheckoutDate, cl.DueDate, b.FirstName, b.LastName, b.Email
                FROM CheckoutLog cl 
                    INNER JOIN Media m ON cl.MediaID = m.MediaID
                    INNER JOIN Borrower b ON cl.BorrowerID = b.BorrowerID
                WHERE ReturnDate IS NULL;", cn);

                cn.Open();

                using (var dr = sql.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        CheckoutLog log = new CheckoutLog();

                        log.MediaID = (int)dr["MediaID"];
                        log.Media.Title = (string)dr["Title"];
                        log.CheckoutDate = (DateTime)dr["CheckoutDate"];
                        log.DueDate = (DateTime)dr["DueDate"];
                        log.Borrower.FirstName = (string)dr["FirstName"];
                        log.Borrower.LastName = (string)dr["LastName"];
                        log.Borrower.Email = (string)dr["Email"];

                        checkedOut.Add(log);
                    }
                }
                cn.Close();

                return checkedOut;
            }
        }

        public DateTime? Checkout(int itemID, Borrower borrower)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = @"
                INSERT INTO CheckoutLog (BorrowerID, MediaID, CheckoutDate, DueDate, ReturnDate) 
                VALUES (@BorrowerID, @MediaID, @CheckoutDate, @DueDate, @ReturnDate);";

                var dateSql = @"
                SELECT DueDate FROM CheckoutLog WHERE MediaID = @MediaID AND ReturnDate IS NULL;";

                var parameters = new
                {
                    borrower.BorrowerID,
                    MediaID = itemID,
                    CheckoutDate = DateTime.Today,
                    DueDate = DateTime.Today.AddDays(7),
                    ReturnDate = (DateTime?)null
                };

                cn.Execute(sql, parameters);

                return cn.QuerySingle<DateTime>(dateSql, parameters);
            }
        }

        public List<Media> GetAvailableMedia()
        {
            using (var cn = new SqlConnection(_cs))
            {
                List<Media> media = [];

                var sql = new SqlCommand(@"
                SELECT m.MediaID, m.Title, mt.MediaTypeName
                FROM Media m
                    LEFT JOIN CheckoutLog cl ON m.MediaID = cl.MediaID
                    INNER JOIN MediaType mt ON m.MediaTypeID = mt.MediaTypeID
                WHERE m.IsArchived = 0 
                    AND m.MediaID = ANY(SELECT MediaID FROM Checkoutlog WHERE ReturnDate IS NOT NULL) 
                    OR cl.CheckoutDate IS NULL 
                ORDER BY m.MediaTypeID;", cn);

                cn.Open();

                using (var dr = sql.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Media item = new Media();

                        item.MediaID = (int)dr["MediaID"];
                        item.Title = (string)dr["Title"];
                        item.MediaType.MediaTypeName = (string)dr["MediaTypeName"];

                        media.Add(item);
                    }
                }
                cn.Close();

                return media;
            }
        }

        public Borrower? GetBorrowerByEmail(string email)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = @"SELECT * FROM Borrower WHERE Email = @Email";

                var parameter = new
                {
                    Email = email
                };

                return cn.QuerySingleOrDefault<Borrower>(sql, parameter);
            }
        }

        public List<CheckoutLog> GetBorrowerCheckoutLog(Borrower borrower)
        {
            using (var cn = new SqlConnection(_cs))
            {
                List<CheckoutLog> checkedOut = [];

                var sql = new SqlCommand(@"
            SELECT cl.MediaID, m.Title, cl.CheckoutDate, cl.DueDate, b.FirstName, b.LastName, b.Email
            FROM CheckoutLog cl 
                INNER JOIN Media m ON cl.MediaID = m.MediaID
                INNER JOIN Borrower b ON cl.BorrowerID = b.BorrowerID
            WHERE cl.BorrowerID = @BorrowerID AND ReturnDate IS NULL;", cn);

                sql.Parameters.AddWithValue("@BorrowerID", borrower.BorrowerID);

                cn.Open();

                using (var dr = sql.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        CheckoutLog log = new CheckoutLog();

                        log.MediaID = (int)dr["MediaID"];
                        log.Media.Title = (string)dr["Title"];
                        log.CheckoutDate = (DateTime)dr["CheckoutDate"];
                        log.DueDate = (DateTime)dr["DueDate"];
                        log.Borrower.FirstName = (string)dr["FirstName"];
                        log.Borrower.LastName = (string)dr["LastName"];
                        log.Borrower.Email = (string)dr["Email"];

                        checkedOut.Add(log);
                    }
                }
                cn.Close();

                return checkedOut;
            }
        }

        public CheckoutLog? GetCheckedOutItem(int itemID)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = @"SELECT * 
                FROM CheckoutLog cl 
                WHERE MediaID = @MediaID AND ReturnDate IS NULL";

                var parameter = new
                {
                    MediaID = itemID
                };

                return cn.QuerySingleOrDefault<CheckoutLog>(sql, parameter);
            }
        }

        public bool IsItemCheckedOut(int itemID)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = @"SELECT *
                FROM CheckoutLog cl
                WHERE MediaID = @MediaID AND ReturnDate IS NULL";

                var parameter = new
                {
                    MediaID = itemID
                };

                var result = cn.QuerySingleOrDefault<bool>(sql, parameter) == null ? true : false;

                return result;
            }
        }

        public bool HasOverdueItems(Borrower borrower)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = @"SELECT *
                FROM CheckoutLog cl 
                WHERE BorrowerID = @BorrowerID AND cl.DueDate < GETDATE()";

                var parameter = new
                {
                    borrower.BorrowerID
                };

                return cn.Query<List<CheckoutLog>>(sql, parameter).Any();
            }
        }

        public bool IsAtMaxItems(Borrower borrower)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = @"
                SELECT COUNT(MediaID) AS ItemsCheckedOut
                FROM CheckoutLog 
                WHERE BorrowerID = @BorrowerID AND ReturnDate IS NULL
                ORDER BY ItemsCheckedOut";

                var parameter = new
                {
                    borrower.BorrowerID
                };

                return cn.ExecuteScalar<int>(sql, parameter) >= 3 ? true : false;
            }
        }

        public void Return(int itemID)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = @"UPDATE CheckoutLog SET ReturnDate = @ReturnDate WHERE MediaID = @MediaID;";

                var parameter = new
                {
                    ReturnDate = DateTime.Today,
                    MediaID = itemID
                };

                cn.ExecuteScalar(sql, parameter);

            }
        }
    }
}
