using AIManHua.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIManHua.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<ComicTask> ComicTasks => Set<ComicTask>();
    public DbSet<Storyboard> Storyboards => Set<Storyboard>();
    public DbSet<GeneratedImage> GeneratedImages => Set<GeneratedImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).ValueGeneratedNever();
            entity.Property(u => u.Username).HasMaxLength(64).IsRequired();
            entity.Property(u => u.Email).HasMaxLength(256).IsRequired();
            entity.Property(u => u.PasswordHash).HasMaxLength(256).IsRequired();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Username).IsUnique();
        });

        modelBuilder.Entity<ComicTask>(entity =>
        {
            entity.HasKey(ct => ct.Id);
            entity.Property(ct => ct.Id).ValueGeneratedNever();
            entity.Property(ct => ct.Title).HasMaxLength(256).IsRequired();
            entity.Property(ct => ct.Style).HasMaxLength(64).IsRequired();
            entity.Property(ct => ct.Status).HasMaxLength(32).IsRequired();
            entity.HasOne(ct => ct.User)
                  .WithMany(u => u.Tasks)
                  .HasForeignKey(ct => ct.UserId);
            entity.HasIndex(ct => ct.Status);
            entity.HasIndex(ct => ct.UserId);
        });

        modelBuilder.Entity<Storyboard>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).ValueGeneratedNever();
            entity.Property(s => s.SceneDescription).HasMaxLength(2048).IsRequired();
            entity.Property(s => s.Dialogue).HasMaxLength(1024).IsRequired();
            entity.Property(s => s.LayoutType).HasMaxLength(32).IsRequired();
            entity.HasOne(s => s.ComicTask)
                  .WithMany(ct => ct.Storyboards)
                  .HasForeignKey(s => s.ComicTaskId);
            entity.HasIndex(s => s.ComicTaskId);
        });

        modelBuilder.Entity<GeneratedImage>(entity =>
        {
            entity.HasKey(gi => gi.Id);
            entity.Property(gi => gi.Id).ValueGeneratedNever();
            entity.Property(gi => gi.ImageUrl).HasMaxLength(1024).IsRequired();
            entity.Property(gi => gi.MinioObjectKey).HasMaxLength(512).IsRequired();
            entity.Property(gi => gi.ContentType).HasMaxLength(64).IsRequired();
            entity.HasOne(gi => gi.ComicTask)
                  .WithMany(ct => ct.Images)
                  .HasForeignKey(gi => gi.ComicTaskId);
            entity.HasIndex(gi => gi.ComicTaskId);
        });
    }
}
