using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SignalRSharedModels
{
    public class HeartBeatModelContext : DbContext
    {
        // only for sqlite to save to db on disk
        private string DbPath { get; }
        public HeartBeatModelContext(IConfiguration configuration)
        {
            // replace the filepath cruft when the project is built so it always uses our db inside at 
            // AccessingInMemoryDBFromConsoleApp/AccessingInMemoryDBFromConsoleApp/test.db
            DbPath = configuration["contentRoot"];
            
            // display the filepath it's using for debugging purposes and so you can find it.
            Console.WriteLine(DbPath);
        }

        // What we'll access when we want to query the context
        public DbSet<HeartBeatDbModel> HeartBeats { get; set; }

        // Used for creating our table in the db during migrations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HeartBeatDbModel>().ToTable("HeartBeatInfo");
        }
        
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}