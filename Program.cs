using EmailSenderProject.Helper.Email.EmailSender;
using EmailSenderProject.Helper.ViewRender;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();


builder.Services.AddTransient<IViewRenderService, ViewRenderService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IEmailSenderSerivce, EmailSenderSerivce>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseCors(options => {
    options.AllowAnyOrigin();
});

app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();





