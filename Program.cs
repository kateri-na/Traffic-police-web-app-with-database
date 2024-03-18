using C__laba_2_console_traffic_police.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using web_police.Data;

namespace web_police
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.AddControllersWithViews();

			string connection = builder.Configuration.GetConnectionString("TrafficConnection");
			builder.Services.AddDbContext<TrafficPoliceContext>(options => options.UseSqlServer(connection));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapRazorPages();

			using (var scope = app.Services.CreateScope())
			{
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				var roles = new[] { "Admin", "Manager", "User" };
				foreach (var role in roles)
				{
					if(!await roleManager.RoleExistsAsync(role))
					{
						await roleManager.CreateAsync(new IdentityRole(role));
					}
				}
			}

            using (var scope = app.Services.CreateScope())
            {
                var userManagerA = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
				string emailA = "admin@mail.ru";
				string passwordA = "admiN#4";
                if(await userManagerA.FindByEmailAsync(emailA) == null)
				{
					var user = new IdentityUser();
					user.UserName = emailA;
					user.Email = emailA;
					user.EmailConfirmed = true;

					await userManagerA.CreateAsync(user, passwordA);
					await userManagerA.AddToRoleAsync(user, "Admin");
				}

                var userManagerM = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                string emailM = "manager@mail.ru";
                string passwordM = "manageR#4";
                if (await userManagerM.FindByEmailAsync(emailM) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = emailM;
                    user.Email = emailM;
                    user.EmailConfirmed = true;

                    await userManagerM.CreateAsync(user, passwordM);
                    await userManagerM.AddToRoleAsync(user, "Manager");
                }
            }

            app.Run();
		}
	}
}