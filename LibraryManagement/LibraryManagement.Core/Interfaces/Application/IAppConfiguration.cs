using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Core.Interfaces.Application
{
    public interface IAppConfiguration
    {
        string GetConnectionString();
        DatabaseMode GetDatabaseMode();
    }
}
