using FrontAuth.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient<ApiService>(client =>
{
    // Define la URL base de la API que vamos a consumir
    client.BaseAddress = new Uri("http://localhost:5005/api/"); //  API base (puerto cambia segun pc)
});

builder.Services.AddScoped<AuthService>();


//configuración de la autenticación de la aplicación usando cookies
builder.Services.AddAuthentication("AuthCookie")
.AddCookie("AuthCookie", options =>
{
    options.LoginPath = "/Auth/Login";   // Aquí siempre envia a la vista login
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
})
.AddGitHub(options =>
{
    options.ClientId = "Ov23li9wcxHhAXYkGvco";
    options.ClientSecret = "4a5d6036d1e5500c8671917fffe133555d4918e0";
    options.CallbackPath = new PathString("/auth/github-callback");
    options.SaveTokens = true;
});


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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
