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

        public DbSet<BorrowRegister> BorrowRegister { get; set; }

        public DbSet<BorrowRegisterDetail> BorrowRegisterDetail { get; set; }

        public DbSet<OperationLog> OperationLog { get; set; }

        public DbSet<FileStorage> FileStorage { get; set; }

        public DbSet<AdminUser> AdminUser { get; set; }

        public DbSet<Project> Project { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<Receiver> Receiver { get; set; }
    }

}