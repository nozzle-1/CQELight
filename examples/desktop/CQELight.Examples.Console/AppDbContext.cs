﻿using CQELight.DAL.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQELight.Examples.Console
{
    class Consts
    {
        public const string CONST_CONNECTION_STRING = "Server=(localdb)\\mssqllocaldb;Database=TestApp_Base;Trusted_Connection=True;MultipleActiveResultSets=true;";
    }

    class AppDbContextConfigurator : IDatabaseContextConfigurator
    {
        public void ConfigureConnectionString(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Consts.CONST_CONNECTION_STRING);
        }
    }

    public class AppDbContext : BaseDbContext
    {
        public AppDbContext()
            : base(new AppDbContextConfigurator())
        {
        }
    }
}
