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
builder.Services.AddHttpClient<PersonService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5095/");
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
