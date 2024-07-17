
using API.Context;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;



using FluentValidation;
using FluentValidation.AspNetCore;
using System.Data.Common;
using System;
using API.Entities;
using API.Validation;
using API.DTOs;
using API.Repository;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("local");
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddScoped<IProductServices,ProductService>();
            builder.Services.AddScoped<IValidator<CreateUpdateProductDTO>, ProductValidation>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            builder.Services.AddValidatorsFromAssemblyContaining<ProductValidation>();
            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
