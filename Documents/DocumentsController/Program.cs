using Application.AzureBlobService;
using Application.DocumentsService;
using Application.Mapper;
using Azure.Storage;
using Azure.Storage.Blobs;
using DocumentsRepo;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentsController;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.ConfigureSwagger();
        builder.Services.ConfigureJWT(builder.Configuration);

        builder.Services.ConfigureMongoDb();

        builder.Services.Configure<BlobServiceOptions>(builder.Configuration.GetSection("AzureBlobStorageConfig"));
        builder.Services.AddSingleton(_ => new BlobServiceClient(Environment.GetEnvironmentVariable("BLOB_CONN_STRING")));

        builder.Services.AddAutoMapper(typeof(DocumentMapper));
        builder.Services.AddScoped<IDocumentRepo, DocumentRepo>();
        builder.Services.AddScoped<IDocumentService, DocumentsService>();
        builder.Services.AddScoped<IBlobService, BlobService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseAuthentication();


        app.MapControllers();

        app.Run();
    }
}
