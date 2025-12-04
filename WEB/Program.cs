using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; 
        options.LogoutPath = "/Logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(1); 
        options.SlidingExpiration = true;
    });

builder.Services.AddRazorPages();

builder.Services.AddHttpClient("BelanjaYukApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5187");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

