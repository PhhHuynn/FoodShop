using ASM.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASM.Server.Data
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Food> Foods { get; set; }
		public DbSet<Combo> Combos { get; set; }
		public DbSet<ComboFood> ComboFoods { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Cart> Cart { get; set; }
		public DbSet<CartDetail> CartDetails { get; set; }
		public DbSet<Conversation> Conversations { get; set; }
		public DbSet<Message> Messages { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Conversation>()
				.HasOne(c => c.Customer)
				.WithMany(u => u.CustomerConversations)
				.HasForeignKey(c => c.CustomerId)
				.OnDelete(DeleteBehavior.Restrict);


			modelBuilder.Entity<Conversation>()
				.HasOne(c => c.Employee)
				.WithMany(u => u.EmployeeConversations)
				.HasForeignKey(c => c.EmployeeId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<OrderDetail>()
				.HasOne(d => d.Order)
				.WithMany(o => o.OrderDetails)
				.HasForeignKey(d => d.OrderId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<CartDetail>()
				.HasOne(d => d.Cart)
				.WithMany(o => o.CartDetails)
				.HasForeignKey(d => d.CartId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<ComboFood>()
				.HasOne(d => d.Combo)
				.WithMany(o => o.ComboFoods)
				.HasForeignKey(d => d.ComboId)
				.OnDelete(DeleteBehavior.Cascade);


		}
	}
}
