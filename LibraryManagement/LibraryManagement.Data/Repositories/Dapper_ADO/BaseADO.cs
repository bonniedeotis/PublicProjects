using Microsoft.Data.SqlClient;

namespace LibraryManagement.Data.Repositories.DapperAndADO
{
    public abstract class BaseADO
    {
        protected readonly SqlConnection _dbContext;

        public BaseADO(string connectionString)
        {
            _dbContext = new SqlConnection(connectionString);
        }
    }
}
