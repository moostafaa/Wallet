using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WalletService.Domain.Interfaces;
using WalletService.Infrastructure.EventSourcing;
using WalletService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register EventStore and SnapshotStore
builder.Services.AddSingleton<IEventStore, EventStore>();
builder.Services.AddSingleton<ISnapshotStore, SnapshotStore>();

// Register repositories
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IWalletAccountRepository, WalletAccountRepository>();
builder.Services.AddScoped<IWithdrawalRequestRepository, WithdrawalRequestRepository>();
builder.Services.AddScoped<ISettlementBatchRepository, SettlementBatchRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();