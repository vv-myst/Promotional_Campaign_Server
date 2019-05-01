#region Auto generated information. Please do not modify

// DunnhumbyHomeWork DunnhumbyHomeWork Program.cs
// BILA007 Bilangi, Vivek-Vardhan, IT Collection International
// 2019-04-29 15:21
// 2019-04-28 19:34

#endregion

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DunnhumbyHomeWork
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost
                   .CreateDefaultBuilder(args)
                   .UseStartup<Startup>();
        }
    }
}
