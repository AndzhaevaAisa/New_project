using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


//модель пользователя - класс User
namespace project_new.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}
