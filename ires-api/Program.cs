using ires.AppService;
using ires.Domain.Contracts;
using ires.Infrastructure;
using ires.Infrastructure.Data;
using ires.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("corspolicy", b =>
    {
        b.WithOrigins(builder.Configuration.GetSection("uiBaseURLs").Get<string[]>()).AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "")),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IRESConnection"),
    b =>
    {
        b.MigrationsAssembly(typeof(_ForInfrastructureAssembyLoadOnly).Assembly.GetName().Name);
    })
);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        //options.SerializerSettings.DateFormatString = "yyyy-MM-dd hh:mm tt";
    });
builder.Services.AddAutoMapper(typeof(_ForAppServiceAssembyLoadOnly).Assembly);
builder.Services.AddScoped<IAppService, AppRepository>();
builder.Services.AddScoped<ICompanyService, CompanyRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeRepository>();
builder.Services.AddScoped<IMailService, MailRepository>();
builder.Services.AddScoped<IClientService, ClientRepository>();
builder.Services.AddScoped<ISurveyService, SurveyRepository>();
builder.Services.AddScoped<IChargeService, ChargeRepository>();
builder.Services.AddScoped<IPaymentService, PaymentRepository>();
builder.Services.AddScoped<IAccountService, AccountRepository>();
builder.Services.AddScoped<IBillService, BillRepository>();
builder.Services.AddScoped<IBankService, BankRepository>();
builder.Services.AddScoped<IExpenseService, ExpenseRepository>();
builder.Services.AddScoped<IFileService, FileRepository>();
builder.Services.AddScoped<ILogService, LogRepository>();
builder.Services.AddScoped<IOtherChargeService, OtherChargeRepository>();
builder.Services.AddScoped<IPettyCashService, PettyCashRepository>();
builder.Services.AddScoped<IProjectService, ProjectRepository>();
builder.Services.AddScoped<IRentalService, RentalRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("corspolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();
