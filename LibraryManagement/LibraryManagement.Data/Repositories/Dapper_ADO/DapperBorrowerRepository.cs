using Dapper;
using Dapper.Transaction;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Application.Repositories;
using Microsoft.Data.SqlClient;

namespace LibraryManagement.Data.Repositories.DapperAndADO
{
    public class DapperBorrowerRepository : IBorrowerRepository
    {
        private string _cs;
        public DapperBorrowerRepository(string connectionString)
        {
            _cs = connectionString;
        }

        public int Add(Borrower borrower)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = @"INSERT INTO Borrower (FirstName, LastName, Email, Phone) 
                        VALUES (@FirstName, @LastName, @Email, @Phone)";

                var parameters = new
                {
                    borrower.FirstName,
                    borrower.LastName,
                    borrower.Email,
                    borrower.Phone
                };

                cn.Open();

                using (var transaction = cn.BeginTransaction())
                {
                    transaction.Execute(sql, parameters);
                    transaction.Commit();
                }
                cn.Close();

                var sqlID = "SELECT BorrowerID FROM Borrower WHERE Email = @Email;";
                var idParam = new
                {
                    borrower.Email
                };

                return cn.QuerySingle<int>(sqlID, idParam);
            }
        }

        public void Delete(Borrower borrower)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql1 = @"DELETE FROM CheckoutLog WHERE BorrowerID = @BorrowerID";
                var sql2 = @"DELETE FROM Borrower WHERE BorrowerID = @BorrowerID";

                var parameter = new
                {
                    borrower.BorrowerID
                };

                cn.Open();

                using (var transaction = cn.BeginTransaction())
                {
                    transaction.Execute(sql1, parameter);
                    transaction.Execute(sql2, parameter);
                    transaction.Commit();
                }
            }
        }

        public List<Borrower> GetAll()
        {
            using (var cn = new SqlConnection(_cs))
            {
                List<Borrower> borrowers = [];

                var sql = "SELECT * FROM Borrower";

                return cn.Query<Borrower>(sql).ToList();
            }
        }

        public Borrower? GetByEmail(string email)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = $"SELECT * FROM Borrower WHERE Email = @Email";

                var parameter = new
                {
                    Email = email
                };

                return cn.QuerySingleOrDefault<Borrower>(sql, parameter);
            }
        }

        public Borrower? GetById(int id)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = $"SELECT * FROM Borrower WHERE BorrowerID = @ID";

                var parameter = new
                {
                    ID = id
                };
                return cn.QuerySingleOrDefault<Borrower>(sql, parameter);
            }
        }

        public void Edit(Borrower borrower)
        {
            using (var cn = new SqlConnection(_cs))
            {
                var sql = @"UPDATE Borrower SET FirstName = @FirstName, LastName = @LastName, Email = @Email WHERE BorrowerID = @BorrowerID";

                var parameters = new
                {
                    borrower.FirstName,
                    borrower.LastName,
                    borrower.Email,
                    borrower.BorrowerID
                };

                cn.Open();

                using (var transaction = cn.BeginTransaction())
                {
                    transaction.Execute(sql, parameters);
                    transaction.Commit();
                }
                cn.Close();
            }
        }

        public List<CheckoutLog> GetBorrowedItems(string email)
        {
            using (var cn = new SqlConnection(_cs))
            {
                List<CheckoutLog> List = [];

                var cmd = new SqlCommand(
                    @"SELECT cl.MediaID, cl.CheckoutDate, cl.DueDate, b.BorrowerID, m.Title, b.Email
                FROM CheckoutLog cl 
                    INNER JOIN Borrower b ON cl.BorrowerID = b.BorrowerID
                    INNER JOIN Media m ON cl.MediaID = m.MediaID
                WHERE Email = @Email AND ReturnDate IS NULL;", cn);

                cmd.Parameters.AddWithValue("@Email", email);

                cn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        CheckoutLog Log = new CheckoutLog();

                        Log.BorrowerID = (int)dr["BorrowerID"];
                        Log.MediaID = (int)dr["MediaID"];
                        Log.CheckoutDate = (DateTime)dr["CheckoutDate"];
                        Log.DueDate = (DateTime)dr["DueDate"];
                        Log.Media.Title = (string)dr["Title"];
                        Log.Borrower.Email = (string)dr["Email"];

                        List.Add(Log);
                    }
                }
                cn.Close();

                return List;
            }
        }
    }
}
