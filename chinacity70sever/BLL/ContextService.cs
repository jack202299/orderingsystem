using chinacity70sever.Models;
using Microsoft.EntityFrameworkCore;

namespace chinacity70sever.BLL
{
    public class ContextService
    {

        public static IServiceProvider ServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
            var connstring = dbManager.Gethosturl("myconnstring");             
            services.AddDbContext<china70Context>(options => options.UseMySql(connstring, ServerVersion.AutoDetect(connstring)));
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static china70Context GetContext(IServiceProvider services)
        {
            var sqliteContext = services.GetService<china70Context>();
            return sqliteContext;
        }

        /// <summary>
        /// 获取上下文
        /// </summary>
        public static china70Context GetContext()
        {
            var services = ServiceProvider();
            var sqliteContext = services.GetService<china70Context>();
            return sqliteContext;
        }
    }
}
