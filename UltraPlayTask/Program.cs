using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UltraPlayTask.Infrastructure;
using UltraPlayTask.Infrastructure.IRepositories;
using UltraPlayTask.Infrastructure.Repositories;
using UltraPlayTask.Interfaces;
using UltraPlayTask.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<UltraPlayTaskDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("UltraPlayTaskDB"));
});
// Register XmlFeedService
builder.Services.AddHttpClient<XmlFeedService>(); // Registering with HttpClient
builder.Services
    .AddScoped<XmlFeedService>()
    .AddScoped<IMatchService, MatchService>()
    .AddScoped<IMatchRepository,MatchRepository>();
//builder.Services.AddSingleton<UpdateNotificationService>();
//builder.Services.AddHostedService<FetchFeedHostedService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UltraPlayTaskDBContext>();

    if (!dbContext.Database.CanConnect())
    {
        throw new NotImplementedException("Can't connect to DB");
    }
}
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
