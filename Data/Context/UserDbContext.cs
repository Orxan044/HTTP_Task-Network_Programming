using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;
using static System.Net.Mime.MediaTypeNames;

namespace Data.Context;

public class UserDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["constrUser"].ConnectionString);
        optionsBuilder.UseSqlServer("Data Source = ORXAN; Initial Catalog = UserDbHTTP; Integrated Security = True; Connect Timeout = 30; Encrypt = False; Trust Server Certificate = False; Application Intent = ReadWrite; Multi Subnet Failover = False");
    }

    public DbSet<User> Users { get; set; }
}
