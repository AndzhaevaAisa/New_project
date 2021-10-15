using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//класс контекста данных ApplicationContext для взаимодействия с ms sql server
namespace project_new.Models.Context
{
    public class ApplicationContext : IdentityDbContext<User>
    {   //мы передаем параметры подключения
        //в контекст данных извне через конструктор с параметром типа DbContextOptions:
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            
        }
        

    }
}
