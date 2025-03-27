using ContactDetails_Api.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace ContactDetails_Api.Data
{
    public class AppDbContex:DbContext
    {
        public AppDbContex(DbContextOptions<AppDbContex> options) : base(options) { }
        public DbSet<Contact> Contacts { get; set; }
    }
}
