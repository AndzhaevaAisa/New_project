using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using project_new.Models;
using project_new.ViewModels;
using System.Threading.Tasks;

namespace project_new.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Страница регистрации 
        /// {api/Account/register}
        /// </summary>
        /// <returns></returns>
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// метод Post регистации
        /// {api/Account/register}
        /// </summary>
        /// <param name="model">Форма регистрации</param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        /// <summary>
        /// Страница входа
        /// {api/Account/login}
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet("login")]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// метод Post  входа
        /// {api/Account/login}
        /// </summary>
        /// <param name="model">Форма входа</param>
        /// <returns></returns>
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }
        
        /// <summary>
        /// Выход
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
