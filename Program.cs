using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GASTOS_REEMBOLSABLES_VMICA.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using GASTOS_REEMBOLSABLES_VMICA.services;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;

var builder = WebApplication.CreateBuilder(args);


//Configuramos la conexi n a sql server
//builder.Services.AddDbContext<AppDbContext>(opciones =>
//    opciones.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string  not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

//azure 
builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd");
builder.Services.Configure<OpenIdConnectOptions>(
    AzureADDefaults.OpenIdScheme, options =>
    {
        options.Authority = options.Authority;
        options.TokenValidationParameters.ValidateIssuer = false;
    }
    );

builder.Services.AddAuthorization(
    options =>
    {
        options.AddPolicy("GastosReembolsablesAdmin", policyBuilder => policyBuilder.RequireClaim("groups", "53385490-af6f-4897-99a4-3d0e88d1c29e"));
        options.AddPolicy("GeneralUser", policyBuilder => policyBuilder.RequireClaim("groups", "22cfaf17-fb63-4320-a8f3-964989aeeb21"));
    }
    );
builder.Services.AddMvc(option =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    option.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();


builder.Services.AddHttpContextAccessor();//very importante, or the app can not work
//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();


//builder.Services.AddDefaultIdentity<IdentityUser>()
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<AppDbContext>();



builder.Services.AddTransient<IServicioEmailSendGrid, ServicioEmailSendGrid>();
builder.Services.AddTransient<IServicioUsuarios, ServicioUsuarios>();
builder.Services.AddTransient<IRepositorioOperaciones, RepositorioOperaciones>();
builder.Services.AddTransient<IRepositorioColaboradores, RepositorioColaboradores>();
builder.Services.AddTransient<IRepositorioClientes, RepositorioClientes>();
builder.Services.AddTransient<IRepositorioProductos, RepositorioProductos>();
builder.Services.AddTransient<IRepositorioCategorias, RepositorioCategorias>();
builder.Services.AddTransient<IRepositorioCuentas, RepositorioCuentas>();
builder.Services.AddTransient<IRepositorioEmpresas, RepositorioEmpresas>();
builder.Services.AddTransient<IRepositorioPerfiles, RepositorioPerfiles>();
builder.Services.AddTransient<IRepositorioProductos, RepositorioProductos>();
builder.Services.AddTransient<IRepositorioProyectos, RepositorioProyectos>();
builder.Services.AddTransient<IRepositorioReglas, RepositorioReglas>();
builder.Services.AddTransient<IRepositorioTipoContratos, RepositorioTipoContratos>();
builder.Services.AddTransient<IRepositorioAsignaciones, RepositorioAsignaciones>();
builder.Services.AddTransient<IRepositorioUsuarios, RepositorioUsuarios>();
builder.Services.AddTransient<IRepositorioAppUsuarios, RepositorioAppUsuarios>();
builder.Services.AddTransient<IRepositorioTipoUsuario, RepositorioTipoUsuario>();
builder.Services.AddAutoMapper(typeof(Program));





//builder.Services.AddTransient<IUserStore<Usuario>, UsuarioStore>();
//note : I was not able to do "add-migration addtableIdentityInDatabase" due to these 3 lines
//cause it should be<IdentityUser>, not<Usuario>
builder.Services.AddTransient<SignInManager<IdentityUser>>();
builder.Services.AddIdentityCore<IdentityUser>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/AppUsuarios/LogIn";
});

//builder.Services.AddAuthentication(
//    options =>
//    {
//        options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
//        options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
//        options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
//    }).AddCookie(IdentityConstants.ApplicationScheme,
//    options =>
//    {
//        options.LoginPath = "/AppUsuarios/LogIn";
//    });

//opciones de configuracion del identity

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = false);






var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
