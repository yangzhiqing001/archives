using System;
using archives.service.dal.Entity;
using Microsoft.EntityFrameworkCore;

namespace archives.service.dal
{
    public class ArchivesContext : DbContext
    {
        public ArchivesContext(DbContextOptions<ArchivesContext> options)
            : base(options)
        {
        }
        public DbSet<ArchivesInfo> ArchivesInfo { get; set; }
    }

}