using LanchesMac.Areas.Admin.Servicos;
using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace LanchesMac;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<IdentityUser, IdentityRole>() //serviços do Microsoft.Identity
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options => //esse serviço serve para atribuir as senhs no registro os caracteres da senha
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;   
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });


        services.AddTransient<IPedidoRepository, PedidoRepository>();
        services.AddTransient<ILancheRepository, LanchesRepository>();
        services.AddTransient<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
        services.AddScoped<RelatorioVendasService>();
        services.AddScoped<GraficoVendasService>();

        services.AddAuthorization(option =>
        {
            option.AddPolicy("Admin",
                politica =>
                {
                    politica.RequireRole("Admin");
                });




        });

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //uma instancia para acessar os recursos de HttpContex
        services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));           // e obter informações do response, request e sobre autenticação
                                                                                   //usamos no model de CARRINHO COMPRA

        services.AddControllersWithViews();

        services.AddPaging(options =>
        {
            options.ViewName = "Bootstrap4";
            options.PageParameterName = "pageindex";
        });
         
        services.AddMemoryCache(); //configurando o mindway
        services.AddSession(); //junto com esse que configura a Session 

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeedUserRoleInitial seedUserRoleInitial)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();


        app.UseStaticFiles();
        app.UseRouting();
       
        seedUserRoleInitial.SeedRoles(); //cria os perfils
        seedUserRoleInitial.SeedUsers(); //cria os usuário e atribui ao perfil
        
        app.UseSession();//coloco este para a Session ser usada na minha aplicação


        app.UseAuthentication(); //serviço do Microsoft.Identity. ele sempre tem que vir antes do AUTHORIZATION
        app.UseAuthorization();



        //este aqui é apenas um roteamento como exemplo
        /*endpoints.MapControllerRoute( 
            name: "teste", 
            pattern: "testeme",
            defaults: new { controller = "teste", Action = "index" }); //deve-se colocar os mesmo nome do método que estão na controller

        endpoints.MapControllerRoute(
            name: "admin",
            pattern: "admin/{action= index}/{id?}", //aqui o id é opcional
            defaults: new { controller ="admin"} //aqui coloca os controles com o mesmo nome do método no controller
            );*/
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
            );


            endpoints.MapControllerRoute(
           name: "categoriaFiltro",
           pattern: "lanche/{action}/{categoria?}", //lanche //list //categoria
           defaults: new { controller = "lanche", action = "list" });


            endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");




        });

           





    }
}