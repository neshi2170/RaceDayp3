var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o => { o.Cookie.HttpOnly = true; o.Cookie.IsEssential = true; o.IdleTimeout = TimeSpan.FromMinutes(30); });
builder.Services.AddHttpClient("RaceDayApi", c => c.BaseAddress = new Uri(builder.Configuration["Api:BaseUrl"] ?? "http://localhost:8080/"));
builder.Services.AddScoped<RaceDay.Mvc.Services.ApiClient>();
var app = builder.Build();
if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Home/Error"); app.UseHsts(); }
app.UseHttpsRedirection(); app.UseStaticFiles(); app.UseRouting(); app.UseSession(); app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}"); app.Run();
