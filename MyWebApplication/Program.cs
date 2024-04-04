//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddHostedService<IHostedService>();
//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();


/*
 * 在 ASP.NET Core 应用程序中，通常使用 WebHostBuilder 或 HostBuilder 来构建主机。 WebHostBuilder 用于构建 Web 主机，而 HostBuilder 用于构建通用的主机，可以承载各种类型的应用程序，包括 Web 应用程序和非 Web 应用程序。
 */
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MyWebApplication;
using MyWebApplication.Util;
using NLog.Web;
using static System.Net.Mime.MediaTypeNames;

public class Program
{
    public static void Main(string[] args)
    {
        Test();
        CreateHostBuilder(args).Build().Run();
    }
    static void Test()
    {
        // 初始化 FileInfo 对象
        FileInfo iFile = new FileInfo("C:\\example\\file.txt");

        // 获取目录路径
        string sourcePath = iFile.Directory.FullName;

        // 创建备份目录路径
        string backupPath = Path.Combine(sourcePath, "backup");

        // 打印备份目录路径
        System.Console.WriteLine($"备份目录路径为: {backupPath}");

    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices(serviceCollection => {
                // config Service util
                ServiceUtil.Initialize(serviceCollection.BuildServiceProvider());
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>(); // 配置 Startup 类
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();  // 移除其他日志提供程序
                logging.AddConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
                    options.Format = Microsoft.Extensions.Logging.Console.ConsoleLoggerFormat.Systemd;
                });
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
            })
            .UseNLog(); // 使用 NLog
}
// NLog.Web.AspNetCore
/*
 * 在上面的示例中，ConfigureWebHostDefaults 方法用于配置 Web 主机，我们通过调用 UseStartup<Startup>() 方法来指定使用 Startup 类进行应用程序的启动配置。这样，在应用程序启动时，ASP.NET Core 就会自动调用 Startup 类中的配置方法，比如 ConfigureServices 和 Configure 方法。

如果你希望在非 Web 应用程序中使用 Startup 类，你可以使用 HostBuilder，并调用 ConfigureServices 和 Configure 方法来手动配置应用程序。
 */


/*
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // 手动配置服务
                services.AddHostedService<MyBackgroundService>();
                // 添加其他服务
            })
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                // 配置应用程序配置
            })
            .ConfigureLogging((hostContext, logging) =>
            {
                // 配置日志记录
            })
            .ConfigureHostConfiguration(config =>
            {
                // 配置主机配置
            })
            .ConfigureServices((hostContext, services) =>
            {
                // 配置应用程序服务
            })
            .ConfigureLogging(logging =>
            {
                // 配置日志记录
            });
}
 */

/*
在这个示例中，我们手动配置了应用程序的服务，而不是使用 Startup 类。通过 ConfigureServices 方法，我们可以添加应用程序所需的各种服务。
 */
