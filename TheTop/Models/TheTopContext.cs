using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TheTop.Models
{
    public partial class TheTopContext : DbContext
    {
        public TheTopContext()
        {
        }

        public TheTopContext(DbContextOptions<TheTopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attendance> Attendance { get; set; }
        public virtual DbSet<CartItem> CartItem { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Challenge> Challenge { get; set; }
        public virtual DbSet<Coupon> Coupon { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeeTasks> EmployeeTasks { get; set; }
        public virtual DbSet<FavoriteProduct> FavoriteProduct { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<PaymentDetails> PaymentDetails { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Rate> Rate { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Testimonial> Testimonial { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=MuradAwad;Database=TheTop;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.ToTable("attendance");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Day)
                    .HasColumnName("day")
                    .HasColumnType("date");

                entity.Property(e => e.EmpId).HasColumnName("emp_id");

                entity.Property(e => e.LastLogin)
                    .HasColumnName("last_login")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastLogout)
                    .HasColumnName("last_logout")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.Attendance)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK__attendanc__emp_i__59FA5E80");
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("cart_item");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CartName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cartphoto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductId).HasColumnName("Product_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CartItem)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__cart_item__Produ__06CD04F7");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CartItem)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__cart_item__user___05D8E0BE");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Category1)
                    .HasColumnName("category")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryPhoto)
                    .HasColumnName("categoryPhoto")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Challenge>(entity =>
            {
                entity.HasKey(e => e.TaskId)
                    .HasName("PK__task__716846B5D856B955");

                entity.ToTable("challenge");

                entity.Property(e => e.TaskId).HasColumnName("Task_id");

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmpId).HasColumnName("emp_id");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.Challenge)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("FK__task__emp_id__38996AB5");
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Picture)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmpId)
                    .HasName("PK__employee__1299A86170936EC5");

                entity.ToTable("employee");

                entity.Property(e => e.EmpId).HasColumnName("emp_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.DiscountsDescription)
                    .HasColumnName("Discounts_Description")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Month)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Salary)
                    .HasColumnName("salary")
                    .HasColumnType("decimal(8, 3)");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Year)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__employee__user_i__35BCFE0A");
            });

            modelBuilder.Entity<EmployeeTasks>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.EmployeeTasks)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__EmployeeT__role___3C34F16F");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EmployeeTasks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__EmployeeT__user___3B40CD36");
            });

            modelBuilder.Entity<FavoriteProduct>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsFavorite).HasColumnName("isFavorite");

                entity.Property(e => e.ProductId).HasColumnName("Product_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FavoriteProduct)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__FavoriteP__Produ__0D7A0286");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoriteProduct)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__FavoriteP__user___0E6E26BF");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ProductId).HasColumnName("Product_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Notification)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Notificat__Produ__6FB49575");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notification)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Notificat__user___70A8B9AE");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("decimal(8, 3)");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentDetailsId).HasColumnName("paymentDetails_id");

                entity.Property(e => e.PaymentMonth)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentType)
                    .HasColumnName("Payment_type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentYear)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.PaymentDetails)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.PaymentDetailsId)
                    .HasConstraintName("FK_user_payment_paymentDetails");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__user_paym__user___4222D4EF");
            });

            modelBuilder.Entity<PaymentDetails>(entity =>
            {
                entity.ToTable("paymentDetails");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Balance)
                    .HasColumnName("balance")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Expiry)
                    .HasColumnName("expiry")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("Product_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Expire).HasColumnType("date");

                entity.Property(e => e.Location)
                    .HasColumnName("location")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Photo)
                    .HasColumnName("photo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Product__categor__3D5E1FD2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Product_users");
            });

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ProductId).HasColumnName("Product_id");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Rate)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Rate__Product_id__4F47C5E3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rate)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Rate__user_id__503BEA1C");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Role1)
                    .HasColumnName("role")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Testimonial>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Approved).HasColumnName("approved");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__users__B9BE370F5A4FE1FD");

                entity.ToTable("users");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_at")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UsarName)
                    .HasColumnName("usarName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserPhoto).IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__users__role_id__30F848ED");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
