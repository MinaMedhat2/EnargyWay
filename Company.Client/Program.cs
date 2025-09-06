// Using the correct namespace for your project components
using Company.Client.Components;
using Company.Client;
using Company.Client.Services; // <-- إضافة هذا السطر مهم أيضًا

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register HttpClient service to communicate with the API
builder.Services.AddHttpClient("WebAPI", client =>
{
    // The correct address from your launchSettings.json file
    client.BaseAddress = new Uri("https://localhost:7185/");
});

// --- تسجيل الخدمات ---
// Register AuthService as a Singleton to be shared across the entire application
builder.Services.AddSingleton<AuthService>();

// Register OrderStateService as Scoped for the checkout process
builder.Services.AddScoped<OrderStateService>();

// --- START: هذا هو السطر الحاسم الذي يحل المشكلة ---
// Register CartService as Scoped to maintain a cart per user session
builder.Services.AddScoped<CartService>();
// --- END: هذا هو السطر الحاسم الذي يحل المشكلة ---


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
