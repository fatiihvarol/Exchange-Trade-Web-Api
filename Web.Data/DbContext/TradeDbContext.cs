using Web.Data.Entity;

namespace Web.Data.DbContext;
using Microsoft.EntityFrameworkCore;

public class TradeDbContext:DbContext
{
    public TradeDbContext(DbContextOptions<TradeDbContext> options): base(options)
    {
    
    }   
    
    
    public DbSet<User> Users { get; set; }
    public DbSet<Share> Shares { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<TradeLog> TradeLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Client - Portfolio Relationship
        modelBuilder.Entity<User>()
            .HasOne(c => c.Portfolio)
            .WithOne(p => p.User)
            .HasForeignKey<Portfolio>(p => p.UserId);

        // Portfolio - TradeLog Relationship
        modelBuilder.Entity<Portfolio>()
            .HasMany(p => p.TradeLogs)
            .WithOne(t => t.Portfolio)
            .HasForeignKey(t => t.PortfolioId);
        
        modelBuilder.Entity<Portfolio>()
            .HasMany(p => p.PortfolioItems)
            .WithOne(t => t.Portfolio)
            .HasForeignKey(t => t.PortfolioId);

        
        modelBuilder.Entity<Share>()
            .HasMany(s => s.TradeLogs)
            .WithOne(t => t.Share)
            .HasForeignKey(t => t.ShareId);


        base.OnModelCreating(modelBuilder);
    }
}