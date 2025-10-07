using Frontend.Services;
using Frontend;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register services as Scoped (one instance per circuit/user)
builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<PhoneService>();

// Configure HttpClient to call a backend API
var backendBaseUrl = builder.Configuration["Backend:BaseUrl"]
    ?? throw new InvalidOperationException("Backend:BaseUrl is not configured.");

builder.Services.AddHttpClient<PersonService>(client =>
{
    client.BaseAddress = new Uri(backendBaseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
