using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WordSearch;
using WordSearch.Services;
using WordSearch.Services.Contracts;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<IGridService, GridService>();
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<IWordListService, WordListService>();
builder.Services.AddScoped<IWordManagementService, WordManagementService>();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
