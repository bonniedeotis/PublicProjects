﻿namespace LibraryManagement.Core.Interfaces.Application
{
    public interface IAppConfiguration
    {
        string GetConnectionString();
        DatabaseMode GetDatabaseMode();
    }
}
