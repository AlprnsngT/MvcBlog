namespace MvcBlog.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class mvcBlogDB : DbContext
    {
        public mvcBlogDB()
            : base("name=mvcBlogDB")
        {
        }

        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Authority> Authorities { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasMany(e => e.Articles)
                .WithMany(e => e.Tickets)
                .Map(m => m.ToTable("ArticleTicket").MapLeftKey("TicketId").MapRightKey("ArticleId"));
        }
    }
}
