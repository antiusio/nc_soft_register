namespace DataBase
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class NcSoftBase : DbContext
    {
        public NcSoftBase()
            : base("name=NcSoftBase")
        {
        }

        public virtual DbSet<account> accounts { get; set; }
        public virtual DbSet<country> countrys { get; set; }
        public virtual DbSet<email> emails { get; set; }
        public virtual DbSet<open_socks_tunnels> open_socks_tunnels { get; set; }
        public virtual DbSet<proxy> proxys { get; set; }
        public virtual DbSet<results_using_proxy> results_using_proxy { get; set; }
        public virtual DbSet<setting> settings { get; set; }
        public virtual DbSet<statuses_registration> statuses_registration { get; set; }
        public virtual DbSet<types_proxy> types_proxy { get; set; }
        public virtual DbSet<user_agents> user_agents { get; set; }
        public virtual DbSet<created_accounts> created_accounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<country>()
                .HasMany(e => e.emails)
                .WithOptional(e => e.country)
                .HasForeignKey(e => e.country_id);

            modelBuilder.Entity<country>()
                .HasMany(e => e.proxys)
                .WithOptional(e => e.country)
                .HasForeignKey(e => e.country_id);

            modelBuilder.Entity<email>()
                .HasMany(e => e.accounts)
                .WithRequired(e => e.email)
                .HasForeignKey(e => e.email_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proxy>()
                .HasMany(e => e.accounts)
                .WithRequired(e => e.proxy)
                .HasForeignKey(e => e.proxy_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proxy>()
                .HasMany(e => e.open_socks_tunnels)
                .WithOptional(e => e.proxy)
                .HasForeignKey(e => e.proxy_id);

            modelBuilder.Entity<statuses_registration>()
                .HasMany(e => e.accounts)
                .WithOptional(e => e.statuses_registration)
                .HasForeignKey(e => e.status_id);

            modelBuilder.Entity<types_proxy>()
                .HasMany(e => e.proxys)
                .WithOptional(e => e.types_proxy)
                .HasForeignKey(e => e.type_id);

            modelBuilder.Entity<user_agents>()
                .HasMany(e => e.accounts)
                .WithOptional(e => e.user_agents)
                .HasForeignKey(e => e.user_agent_id);
        }
    }
}
