using Garage3.Data;
using Microsoft.EntityFrameworkCore;

namespace Garage3
{
    // Initial commit to Github Kasleet
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add AutoMapper Kasleet
            builder.Services.AddAutoMapper(typeof(Program));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add DbContext Kasleet
            builder.Services.AddDbContext<ParkingDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Configuration for capcities and rates Xiahui

           
           //builder.Services.Configure<GarageSettings>(Configuration.GetSection(GarageSettings.VehicleRates));

            //builder.Services.Configure<GarageSettings>(GarageSettings.Capacity,
            //    builder.Configuration.GetSection("GarageSettings:Capacity"));

            builder.Services.Configure<GarageSettings>(GarageSettings.VehicleRates,
               builder.Configuration.GetSection("GarageSettings:VehicleRates"));

            //builder.Services.AddGarageSettings(Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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

            app.Run();
        }
    }
}
