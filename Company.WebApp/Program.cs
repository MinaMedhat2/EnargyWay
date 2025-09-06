using Company.WebApp.Components;
using Company.WebApp.Auth;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// --- Services ---
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7185")
});

// ✅✅✅ هذا هو التصحيح النهائي لنظام المصادقة ✅✅✅
builder.Services.AddAuthentication(options =>
{
    // نخبر النظام أن المخطط الافتراضي للمصادقة والتحدي هو "apiauth"
    options.DefaultScheme = "apiauth";
    options.DefaultChallengeScheme = "apiauth";
})
.AddCookie("apiauth", options =>
{
    // نحدد مسار صفحة تسجيل الدخول لهذا المخطط
    options.LoginPath = "/login";
});
// ✅✅✅ نهاية التصحيح ✅✅✅

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// --- Application Pipeline ---
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// الترتيب هنا مهم جداً
app.UseRouting(); // أضف هذا السطر لضمان عمل الأنابيب بشكل صحيح
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
