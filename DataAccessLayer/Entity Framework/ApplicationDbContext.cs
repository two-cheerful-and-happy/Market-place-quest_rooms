using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Helpers;
using DataAccessLayer.EF;

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
    public DbSet<Photo> PhotoTable { get; set; }
    public DbSet<Comment> CommentTable { get; set; }
    public DbSet<RequestOnChangingRole> RequestOnChangingRoleTable { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
                .Property(x => x.Address)
                .HasColumnType("VARCHAR(500)")
                .HasColumnName("Address")
                .IsRequired();

            buildAction
                .Property(x => x.PhoneNumber)
                .HasColumnType("VARCHAR(30)")
                .HasColumnName("PhoneNumber")
                .IsRequired();

            buildAction
                .Property(x => x.Birthday)
                .HasColumnName("Birthday")
                .HasConversion<DateOnlyConverter, DateOnlyComparer>();

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
                .HasMany(x => x.CommentsCreatedByAccount)
                .WithMany(x => x.CommentsOfAccount);

            buildAction
                .HasData(new Account
                {
                    Id = 1,
                    AccountConfirmed = true,
                    Email = "denyschk@gmail.com",
                    Login = "Goose",
                    Password = HashPasswordHelper.HashPassowrd("12345678"),
                    Role = Domain.Enums.Role.Admin,
                    Address = "Dom",
                    Birthday = new DateOnly(2022, 1, 1),
                    PhoneNumber = "+0502689846"

                }); 
        });

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

            buildAction
                .HasMany(x => x.CommentsOfLocation)
                .WithMany(x => x.CommentsOfLocation);
        });

        modelBuilder.Entity<RequestOnChangingRole>(buildAction =>
        {
            buildAction
                .ToTable("RequestOnChangingRoleAccount_Table");

            buildAction
                .HasKey(x => x.Id)
                .HasName("index_of_request");

            buildAction
                .Property(x => x.RequestedRole)
                .HasColumnType("VARCHAR(20)")
                .HasColumnName("RequestedRole")
                .IsRequired();

            buildAction
                .Property(x => x.DescriptionOrReason)
                .HasColumnType("TEXT")
                .HasColumnName("DescriptionOrReason")
                .IsRequired();

            buildAction
                .HasOne(x => x.Account)
                .WithOne(x => x.RequestOnChangingRole);
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

        modelBuilder.Entity<Photo>(buildAction =>
        {
            buildAction.
                ToTable("Photo_Table");

            buildAction
                .HasKey(x => x.Id)
                .HasName("Index_of_Location_of_user");

            buildAction
                .Property(x => x.Value)
                .HasColumnType("VARBINARY(MAX)")
                .HasColumnName("Value")
                .IsRequired();

            buildAction
                .HasOne(x => x.LocationId)
                .WithMany(x => x.Photos);
        });
    }
}
