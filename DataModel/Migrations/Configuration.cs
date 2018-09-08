
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DataModel.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataModel.EfContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    } 
}