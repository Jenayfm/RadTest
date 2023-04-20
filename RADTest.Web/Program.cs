using RADTest.Domain.Context;
using RADTest.Domain.Domains;
using RADTest.Domain.Domains.Interfaces;
using RADTest.Domain.Factories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient(typeof(IAccountDomain), typeof(AccountDomain));

builder.Services.AddTransient(typeof(ITransactionDomain), typeof(TransactionDomain));
builder.Services.AddTransient(typeof(ITransactionDomain), typeof(TransactionDomain));

builder.Services.AddTransient(typeof(IDepositTransactionFactory), typeof(DepositTransactionFactory));
builder.Services.AddTransient(typeof(IWithdrawTransactionFactory), typeof(WithdrawTransactionFactory));

builder.Services.AddSingleton(typeof(IContext), typeof(Context));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
