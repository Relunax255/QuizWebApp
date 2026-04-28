using Microsoft.EntityFrameworkCore;
using QuizWebApp.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient<IQuizApiClient, QuizApiClient>();

builder.Services.AddSingleton<IQuizCategoryService, QuizCategoryService>();

builder.Services.AddScoped<IQuizService, QuizService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var categoryService = scope.ServiceProvider.GetRequiredService<IQuizCategoryService>();
    await categoryService.InitializeAsync();
}
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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
