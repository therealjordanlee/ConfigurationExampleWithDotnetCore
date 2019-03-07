using System;
using ConfigurationExampleWithDotnetCore.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConfigurationExampleWithDotnetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();


            var accountConfig = new AccountConfiguration();
            var projectConfig = new ProjectConfiguration();
            
            config.GetSection("Account").Bind(accountConfig);
            config.GetSection("Project").Bind(projectConfig);
            
            Console.WriteLine($"{accountConfig.Username} {accountConfig.Email} {accountConfig.Website}");
            Console.WriteLine($"{projectConfig.ProjectName} {projectConfig.GithubUrl}");
            
            var services = new ServiceCollection()
                .AddOptions()
                .Configure<AccountConfiguration>(config.GetSection("Account"))
                .Configure<ProjectConfiguration>(config.GetSection("Project"))
                .AddScoped(cfg => cfg.GetService<IOptions<AccountConfiguration>>().Value)
                .AddScoped(cfg => cfg.GetService<IOptions<ProjectConfiguration>>().Value)
                .BuildServiceProvider();


            var accountConfig2 = services.GetService<AccountConfiguration>();
            var projectConfig2 = services.GetService<ProjectConfiguration>();
            
            Console.WriteLine($"{accountConfig2.Username} {accountConfig2.Email} {accountConfig2.Website}");
            Console.WriteLine($"{projectConfig2.ProjectName} {projectConfig2.GithubUrl}");
        }
    }
}