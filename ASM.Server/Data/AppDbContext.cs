using ASM.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASM.Server.Data
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Food> Foods { get; set; }
		public DbSet<Combo> Combos { get; set; }
		public DbSet<ComboFood> ComboFoods { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartDetail> CartDetails { get; set; }
		public DbSet<Voucher> Vouchers { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<Conversation> Conversations { get; set; }
		public DbSet<Message> Messages { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Product>()
				.HasDiscriminator<string>("ProductType")
				.HasValue<Food>("Food")
				.HasValue<Combo>("Combo");

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
				.HasOne(cf => cf.Combo)
				.WithMany(c => c.ComboFoods)
				.HasForeignKey(cf => cf.ComboId)
				.OnDelete(DeleteBehavior.Restrict); 

			modelBuilder.Entity<ComboFood>()
				.HasOne(cf => cf.Food)
				.WithMany(f => f.ComboFoods)
				.HasForeignKey(cf => cf.FoodId)
				.OnDelete(DeleteBehavior.Restrict); 


			modelBuilder.Entity<Category>()
				.HasMany(c => c.Foods)
				.WithOne(f => f.Category)
				.HasForeignKey(f => f.CategoryId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Order>()
				.HasOne(o => o.Voucher)
				.WithMany()             
				.HasForeignKey(o => o.VoucherId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Review>()
				.HasOne(r => r.Product)
				.WithMany()
				.HasForeignKey(r => r.ProductId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Conversation>()
				.HasMany(c => c.Messages)
				.WithOne(m => m.Conversation)
				.HasForeignKey(m => m.ConversationId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Message>()
				.HasOne(m => m.Sender)
				.WithMany()
				.HasForeignKey(m => m.SenderId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Conversation>()
				.HasOne(c => c.Customer)
				.WithMany()
				.HasForeignKey(c => c.CustomerId)
				.OnDelete(DeleteBehavior.Restrict);


			modelBuilder.Entity<Conversation>()
				.HasMany(c => c.Messages)
				.WithOne(m => m.Conversation)
				.HasForeignKey(m => m.ConversationId)
				.OnDelete(DeleteBehavior.Cascade);


		}
	}
}
