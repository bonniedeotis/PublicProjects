using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
