using StudentAdmissions.Application;
using StudentAdmissions.Application.Implentations;
using StudentAdmissions.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Configuration.AddJsonFile("appSettings.json").AddUserSecrets<Program>(true);
var conStrBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
conStrBuilder.Password = builder.Configuration["StudentAdmissions:DbPassword"];

builder.Services.AddDbContext<AcademyContext>(
    option => option.UseSqlServer(conStrBuilder.ConnectionString));

builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<IStudentWeaknessService, StudentWeaknessService>();
builder.Services.AddTransient<IStudentPowerService, StudentPowerService>();
builder.Services.AddTransient<IPowerService, PowerService>();
builder.Services.AddTransient<IWeaknessService, WeaknessService>();
builder.Services.AddTransient<IStudentSectionService, StudentSectionService>();
builder.Services.AddTransient<ICourseService, CourseService>();
builder.Services.AddTransient<IInstructorService, InstructorService>();
builder.Services.AddTransient<ISectionService, SectionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
