using Itinero;
using Itinero.IO.Osm;
using Itinero.Osm.Vehicles;
using Malloc.Data;
using Malloc.Data.AutocompleteModel;
using Malloc.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

//var AA = JsonConvert.DeserializeObject<LocationJSON>(File.ReadAllText("startPoint.json"));
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<XRouteService>().AddHttpClient("komoto", (a) =>
{
    a.BaseAddress =  new Uri("http://172.17.0.134:5431");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler()
    {
        AllowAutoRedirect = true,
        UseDefaultCredentials = true,
        MaxConnectionsPerServer = 500,
        UseCookies = true,
    };
});

builder.Services.AddDbContextFactory<nominatimContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("AutocompleteConn")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
