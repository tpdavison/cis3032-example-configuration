using System;
using Microsoft.EntityFrameworkCore;

namespace LaMaCo.Comments.Api.Data;

public class CommentsContext : DbContext
{
    public DbSet<Comment> Comments { get; set; } = null!;

    public CommentsContext(DbContextOptions<CommentsContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Comment>(x =>
        {
            x.Property(c => c.AuthorName).IsRequired();
            x.Property(c => c.Text).IsRequired();
        });
    }
}
