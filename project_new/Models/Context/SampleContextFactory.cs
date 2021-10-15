using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

//Класс SampleContextFactory применяет интерфейс IDesignTimeDbContextFactory,
//который типизируется типом контекста данных - в данном случае класс ApplicationContext.
//Данный интерфейс содержит один метод CreateDbContext(), который должен возвращать созданный
//объект контекста данных.
namespace project_new.Models.Context
{
    public class SampleContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        //В данном случае также получаем конфигурацию из файла appsettings.json
        //и извлекаем из ее строку подключения и таким образом создаем контекст.
        public ApplicationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

            // получаем конфигурацию из файла appsettings.json
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();

            // получаем строку подключения из файла appsettings.json
            string connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}
