using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.OpenApi;
using ServeyBasket.Services.Answers;
using ServeyBasket.Services.Authentications.EmailSender;
using ServeyBasket.Services.BackgroundJobNotification;
using ServeyBasket.Services.Results;
using ServeyBasket.Services.Votes;
using ServeyBasket.Settings;


namespace ServeyBasket;

public static class DependancyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
        
        services.AddHybridCache();

        services
            .AddDbContext(configuration)
            .AddBackgrounJobsServices(configuration)
            .AddAuthenticationServices(configuration)
            .AddMappesterServices()
            .AddFluentValidationServices();

        services.AddCors(options => 
            options.AddDefaultPolicy(builder => 
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            )
        );

        services.AddScoped<IAuthServices, AuthServices>();
        services.AddScoped<IPollServices, PollServices>();
        services.AddScoped<IQuestionServices, QuestionServices>();
        services.AddScoped<IAnswerServices, AnswerServices>();
        services.AddScoped<IVoteServices, VoteServices>();
        services.AddScoped<IResultServices, ResultServices>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<INotificationService, NotificationService>();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddHttpContextAccessor();

        services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

        return services;
    }
    private static IServiceCollection AddFluentValidationServices(this IServiceCollection services)
    {
        services
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddFluentValidationAutoValidation();

        return services;
    }
    private static IServiceCollection AddMappesterServices(this IServiceCollection services)
    {
        var mapconfig = TypeAdapterConfig.GlobalSettings;
        mapconfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mapconfig));

        return services;
    }
    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
            throw new InvalidOperationException("DataBase 'DefaultConnection' Is Not Found!");

        services.AddDbContext<ServeyBasketDbContext>(options =>
            options.UseSqlServer(connectionString));


        return services;
    }
    private static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ServeyBasketDbContext>()
            .AddDefaultTokenProviders();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ServeyBasket",
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter JWT token like this: Bearer {your token}"
            });
        });

        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtOptions?.Issuer,
                    ValidAudience = jwtOptions?.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.Key!))
                };
            });

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
        });

            return services;
    }
    private static IServiceCollection AddBackgrounJobsServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddHangfire(config => config
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")))
        // Add the processing server as IHostedService
        .AddHangfireServer();
    }
}
