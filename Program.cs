using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QuizWebApp.Data;
using QuizWebApp.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient<IQuizApiClient, QuizApiClient>();

builder.Services.AddSingleton<IQuizCategoryService, QuizCategoryService>();

builder.Services.AddScoped<IQuizService, QuizService>();

builder.Services.AddSession();

//builder.Services.AddDbContext<QuizDbContext>(options => options.use)

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var categoryService = scope.ServiceProvider.GetRequiredService<IQuizCategoryService>();
//    await categoryService.InitializeAsync();
//}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
