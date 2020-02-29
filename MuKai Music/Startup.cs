using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MuKai_Music.DataContext;
using MuKai_Music.Middleware;
using System;
using MuKai_Music.Model.DataEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using MuKai_Music.Model.Authentication;
using MuKai_Music.Cache;
using MuKai_Music.Middleware.ApiCache;
using MuKai_Music.Middleware.TokenManager;

namespace MuKai_Music
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,//是否验证Issuer
                ValidateAudience = true,//是否验证Audience
                ValidateLifetime = true,//是否验证失效时间
                ClockSkew = TimeSpan.FromSeconds(30),
                ValidateIssuerSigningKey = true,//是否验证SecurityKey
                ValidAudience = Configuration.GetValue<string>("Domain"),//Audience
                ValidIssuer = Configuration.GetValue<string>("Domain"),//Issuer，这两项和前面签发jwt的设置一致
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("SecurityKey")))
            };
        }


        public static IConfiguration Configuration { get; private set; }

        public static TokenValidationParameters TokenValidationParameters { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerDocument();
            services.AddControllersWithViews();
            /*
             * TODO 将音乐信息持久化保存在数据库
             */

            //数据库上下文配置
            services.AddDbContext<MusicContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PostgreSql"))
            );
            services.AddDbContext<AccountContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PostgreSql"))
            );

            services.AddHttpClient();
            //注册身份验证
            services.AddIdentityCore<UserInfo>(options =>
            {
                //设置密码安全性，这里做宽松处理，在UserInfo实体类模型中通过注解做详细限制
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;

                options.User.RequireUniqueEmail = true;
            }).AddSignInManager<SignInManager<UserInfo>>()
              .AddUserManager<AccountManager>()
              .AddEntityFrameworkStores<AccountContext>();
            //清除默认tokenHandler
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //启用json web token 做登录验证方案,防止Cookie和网易Cookie冲突
            services.AddAuthentication(options =>
             {
                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
             }
            ).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = TokenValidationParameters;

            });
            services.AddHttpContextAccessor();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "mukaiMusic/dist/muKaiMusic");
            //添加内存缓存到DI容器
            services.AddICache(option =>
            {
                option.Age = int.Parse(Configuration["cache-age"]);
                option.CacheType = Enum.Parse<CacheType>(Configuration["cache-type"]);
            });
            services.AddMvc(option =>
            {
                /*客户端缓存*/
                option.CacheProfiles.Add("default", new Microsoft.AspNetCore.Mvc.CacheProfile
                {
                    Duration = 86400 /*24小时资源缓存*/
                });
                option.CacheProfiles.Add("longTime", new Microsoft.AspNetCore.Mvc.CacheProfile
                {
                    Duration = 86400 * 7 /*7天缓存*/
                });

            }).AddJsonOptions(configure =>
                    configure.JsonSerializerOptions.PropertyNameCaseInsensitive = true);
            //RedisClient.RedisClientInstance.InitConnect(Configuration);
            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //  app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            //配置启用静态资源文件
            app.UseStaticFiles();
            app.UseOpenApi();
            //使用swagger
            app.UseSwaggerUi3();
            //启用单页面静态资源文件
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseRouting();

            app.UseTokenManager();
            //身份认证
            app.UseAuthentication();
            //授权
            app.UseAuthorization();
            //允许跨域
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
            app.UseApiCacheMiddleware();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            app.UseSpaStaticFiles();
            if (env.IsProduction())
            {
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "mukaiMusic";

                    /*if (env.IsDevelopment())
                    {
                        spa.UseAngularCliServer(npmScript: "start");
                    }*/
                });
            }
        }
    }
}
