using Application.Interfaces.Management;
using Application.Interfaces.Managements;
using Application.Interfaces.Setting;
using Domain.Models;
using Domain.Models.Auth;
using Infrastructure.Data;
using Infrastructure.Repositories.Implementation.Management;
using Infrastructure.Repositories.Implementation.Setting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
	public static class ServicesContainer
	{
		public static IServiceCollection BGSServie(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<BGSDbContext>(options =>
			{
				var connectString = config.GetConnectionString("Default") ??
					throw new InvalidOperationException("Thieu đường dẫn đến CSDL rồi kìa :)) Vui long check trong appsetting.json file");
				options.UseSqlServer(connectString);
			});

			services.AddIdentityCore<User>()
			  .AddRoles<IdentityRole>()
			  .AddRoleManager<RoleManager<IdentityRole>>()
			  .AddUserManager<UserManager<User>>()
			  .AddEntityFrameworkStores<BGSDbContext>()
			  .AddSignInManager<SignInManager<User>>()
			  .AddDefaultTokenProviders();


			services.AddAuthentication();
			services.AddAuthorization();
			services.AddRazorPages();

			// Services
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IInitService, InitService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IHomeService, HomeService>();
			services.AddScoped<ICartService, CartService>();
			services.AddScoped<IStockService, StockService>();

			return services;
		}
	}
}
