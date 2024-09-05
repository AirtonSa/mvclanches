using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }  

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View( new LoginViewModel(){
                ReturnUrl = returnUrl
            });
        }

        [HttpPost] //Nçao pode esquecer disso aqui pois é obrigatorio pq é um metodo assincrono
        public async Task<IActionResult> Login(LoginViewModel loginVM) //Task nos diz que retorna uma IActionReturn
        {
            if (!ModelState.IsValid)// se o MododelState não estiver Valido ele retornará uma view LoginVM
                return View(loginVM); 
                    
            var user = await _userManager.FindByNameAsync(loginVM.UserName); //caso for valido vou tentar localizar o usuario na tabela Identity
                //a palavra "await" é uma suspensão na execução do método até que a tarefa aguardada seja concluida. Tipo um trabalho em andamento

            if(user != null)//aqui o usuario existe
            {
                //aqui faço o login
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                //passo o USUARIO e a SENHA. os dois "falso" são: um pra não pesistir o Token e o outro é pra que se falhar eu não vou bloquear o usuario

                if (result.Succeeded)
                {
                    //aqui vou verificar se o login foi feito com sucesso
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl))//verifico se a Url for nulo ou vazio e se for retorno index
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(loginVM.ReturnUrl);


                }
                
            }
            ModelState.AddModelError("", "Falha ao realizar o Login!");
            return View(loginVM);

        }

        
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginViewModel registroVM)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser {UserName = registroVM.UserName};
                var result = await _userManager.CreateAsync(user, registroVM.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    this.ModelState.AddModelError("Registro", "Falaha ao registrar usuário");
                }
            }

            return View(registroVM);

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.User = null;
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

      

        public IActionResult AccessDenied()
        {
            return View();
        }
    }

    
}
