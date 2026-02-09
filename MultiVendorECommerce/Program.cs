using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MultiVendorECommerce.Areas.Admin.Services;
using MultiVendorECommerce.Areas.Auth.Services;
using MultiVendorECommerce.Areas.Customer.Services;
using MultiVendorECommerce.Authorization;
using MultiVendorECommerce.Constants;
using MultiVendorECommerce.Data;
using MultiVendorECommerce.Filters ;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Services;
using MultiVendorECommerce.Shared.Repositories.Implementations;
using MultiVendorECommerce.Shared.Repositories.Interfaces;
using MultiVendorECommerce.Shared.Services.Implementation;
using MultiVendorECommerce.Shared.Services.Interfaces;

namespace MultiVendorECommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<Areas.Admin.Services.VendorService>();
            builder.Services.AddScoped<Services.VendorService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<HomeService>();
            builder.Services.AddScoped<IVendorQueryService,VendorQueryService>();
            builder.Services.AddScoped<Areas.Vendor.Services.ProductService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<RoleService>();
            builder.Services.AddScoped<CustomerService>();
            builder.Services.AddScoped<CartService>();
            builder.Services.AddScoped<Areas.Customer.Services.OrderService>();
            builder.Services.AddScoped<Areas.Admin.Services.OrderService>();
            builder.Services.AddScoped<Areas.Vendor.Services.OrderService>();
            builder.Services.AddScoped<ICategoryQueryService,CategoryQueryService>();
            builder.Services.AddScoped<IProductQueryService,ProductQueryService>();
            builder.Services.AddScoped<ICartQueryService,CartQueryService>();
            builder.Services.AddScoped<IOrderQueryService,OrderQueryService>();
            builder.Services.AddScoped<IImageService,ImageService>();

            builder.Services.AddScoped<ICustomerRepository, CustomerRepo>();
            builder.Services.AddScoped<ICategoryRepository,CategoryRepo>();
            builder.Services.AddScoped<IVendorRepository,VendorRepo>();
            builder.Services.AddScoped<IProductRepository,ProductRepo>();
            builder.Services.AddScoped<IOrderRepository,OrderRepo>();
            builder.Services.AddScoped<IOrderItemRepository,OrderItemRepo>();
            builder.Services.AddScoped<IProductVariantRepository,ProductVariantRepo>();
            builder.Services.AddScoped<ICartRepository,CartRepo>();
            builder.Services.AddScoped<ICartItemRepository,CartItemRepo>();
            builder.Services.AddScoped<IOfferRepository,OfferRepo>();
            builder.Services.AddScoped<ICategoryAttributeRepository, CategoryAttributeRepo>();
            builder.Services.AddScoped<ICategoryAttributeOptionsRepository, CategoryAttributeOptionRepo>();

            // Add services to the container.
            builder.Services.AddDbContext<Data.ApplicationDb>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDb>();
            builder.Services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(1);
            });

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<TempDataToModelStateFilter>();
                options.Filters.Add<DomainExceptionFilter>();
            });
            builder.Services.ConfigureApplicationCookie(options => 
            {
                options.LoginPath = "/Auth/Account/Login";
                options.AccessDeniedPath = "/Auth/Account/AccessDenied";
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            
            ImageSettings.Configure(builder.Environment.ContentRootPath);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(
            new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(ImageSettings.UploadsRoot),
                RequestPath = ImageSettings.RequestPath
            }
                );

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}",
                defaults: new { area = "Customer" }); 

            app.Run();
        }
    }
}