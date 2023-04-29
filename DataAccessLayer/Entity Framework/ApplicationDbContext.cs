using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace DataAccessLayer.Entity_Framework;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context)
        : base(context)
    {
        Database.EnsureCreated();
    }

    public DbSet<Location> LocationTable { get; set; }
    public DbSet<Account> AccountTable { get; set; }
    public DbSet<LocationOfUser> LocationOfUserTable { get; set; }
    public DbSet<Comment> CommentTable { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>(buildAction =>
        {
            buildAction.
                ToTable("Location_Table");

            buildAction
                .HasKey(x => x.Id)
                .HasName("Index_of_location_record");

            buildAction
                .Property(x => x.Name)
                .HasColumnType("CHAR(50)")
                .HasColumnName("Name")
                .IsRequired();

            buildAction
                .Property(x => x.Address)
                .HasColumnType("NVARCHAR(MAX)")
                .HasColumnName("Address")
                .IsRequired();

            buildAction
                .Property(x => x.Description)
                .HasColumnName("Description")
                .HasColumnType("VARCHAR(500)")
                .IsRequired(false);

            buildAction
                .Property(x => x.LocationConfirmed)
                .HasColumnName("LocationConfirmed")
                .HasColumnType("BIT");

            buildAction
                .Property(x => x.Latitude)
                .HasColumnType("float")
                .HasColumnName("Latitude");

            buildAction
                .Property(x => x.Longitude)
                .HasColumnType("float")
                .HasColumnName("Longitude");

            //buildAction.HasData(new Location
            //{
            //    Id = 1,
            //    Name = "s",
            //    Address = "s",
            //    Description = "s",
            //    LocationConfirmed = true,
            //    Author = new Account { Id = 1 },
            //    Latitude = 49.473826,
            //    Longitude = 10.446910
            //});
        });

        modelBuilder.Entity<Account>(buildAction =>
        {
            buildAction.
                ToTable("Account_Table");

            buildAction
                .HasKey(x => x.Id)
                .HasName("Index_of_account");

            buildAction
                .HasAlternateKey(x => x.Login);

            buildAction
                .Property(x => x.Email)
                .HasColumnType("VARCHAR(50)")
                .HasColumnName("Email")
                .IsUnicode()
                .IsRequired();

            buildAction
                .Property(x => x.Login)
                .HasColumnType("VARCHAR(25)")
                .HasColumnName("Login")
                .IsUnicode()
                .IsRequired();

            buildAction
                .Property(x => x.Password)
                .HasColumnType("VARCHAR(MAX)")
                .HasColumnName("Password")
                .IsRequired();

            buildAction
                .Property(x => x.Role)
                .HasColumnType("VARCHAR(20)")
                .HasColumnName("Role")
                .IsRequired();

            buildAction
                .Property(x => x.AccountConfirmed)
                .HasColumnName("AccountConfirmed")
                .HasColumnType("BIT");

            buildAction
                .HasMany(x => x.LocationsCreatedByAccount)
                .WithOne(x => x.Author);

            buildAction
                .HasMany(x => x.LocationsOfUser)
                .WithMany(x => x.LocationsOfUser);

            buildAction
                .HasMany(x => x.CommentsCreatedByAccount)
                .WithMany(x => x.CommentsOfAccount);
        });

        modelBuilder.Entity<Comment>(buildAction =>
        {
            buildAction.
                ToTable("Comment_Table");

            buildAction
                .HasKey(x => x.Id)
                .HasName("Index_of_comment");

            buildAction
                .Property(x => x.Text)
                .HasColumnType("TEXT")
                .HasColumnName("Text")
                .IsRequired();

            buildAction
                .Property(x => x.Mark)
                .HasColumnType("VARCHAR(15)")
                .HasColumnName("Mark");
        });

        modelBuilder.Entity<LocationOfUser>(buildAction =>
        {
            buildAction.
                ToTable("LocationOfUser_Table");

            buildAction
                .HasKey(x => x.Id)
                .HasName("Index_of_Location_of_user");

            buildAction
                .Property(x => x.Name)
                .HasColumnType("CHAR(50)")
                .HasColumnName("Name")
                .IsRequired();

            buildAction
                .Property(x => x.Address)
                .HasColumnType("VARCHAR(MAX)")
                .HasColumnName("Address")
                .IsRequired();

            buildAction
                .Property(x => x.Description)
                .HasColumnType("CHAR(500)")
                .HasColumnName("Description")
                .IsRequired(false);

            buildAction
                .Property(x => x.Latitude)
                .HasColumnType("FLOAT")
                .HasColumnName("Latitude")
                .IsRequired();

            buildAction
                .Property(x => x.Longitude)
                .HasColumnType("FLOAT")
                .HasColumnName("Longitude")
                .IsRequired();
        });
    }
}
