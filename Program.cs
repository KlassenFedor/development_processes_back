using dev_processes_backend.Data;
using dev_processes_backend.Models;
using dev_processes_backend.Services;
using dev_processes_backend.StartConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.SameSite = SameSiteMode.None;
});

builder.Services.AddScoped<FilesService>();
builder.Services.AddScoped<CompaniesService>();
builder.Services.AddScoped<VacanciesService>();
builder.Services.AddScoped<DownloadableDocumentsService>();
builder.Services.AddScoped<InterviewsService>();
builder.Services.AddScoped<PracticesService>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddControllers();
builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager<SignInManager<User>>()
    .AddUserManager<UserManager<User>>()
    .AddRoleManager<RoleManager<Role>>();

var app = builder.Build();

using var serviceScope = app.Services.CreateScope();
var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>(); 
// auto migration
context?.Database.Migrate();
app.UseCors(builder => builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .WithOrigins("http://localhost:3001") // http://localhost:3000 где запущен фронт
        .AllowCredentials());


await app.ConfigureIdentityAsync();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
