using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using DewCore.Database.MySQL;

namespace DewCore.AspNetCore.Middlewares
{
    /// <summary>
    /// Middleware DewMySQLClient class
    /// </summary>
    public class DatabaseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MySQLConnectionString _cs;
        private readonly string _tp;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next">Next middleware</param>
        /// <param name="cs">Connection string</param>
        /// <param name="tp">Table prefix</param>
        public DatabaseMiddleware(RequestDelegate next, MySQLConnectionString cs)
        {
            _next = next;
            _cs = cs;
            _tp = string.Empty;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next">Next middleware</param>
        /// <param name="cs">Connection string</param>
        /// <param name="tp">Table prefix</param>
        public DatabaseMiddleware(RequestDelegate next, MySQLConnectionString cs, string tp)
        {
            _next = next;
            _cs = cs;
            _tp = tp;
        }
        /// <summary>
        /// Invoke method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                MySQLClient.DebugOn = true;
                MySQLClient.SetDebugger(new Logger.DewConsole());
            }
            context.Items.Add("DewDatabaseConnectionString", _cs);
            context.Items.Add("DewDatabaseTablePrefix", _tp);
            await _next(context);
        }
    }
    /// <summary>
    /// HTTPContext DewMySQLClient Extension class
    /// </summary>
    public static class DewMySQLClientHttpContextExtension
    {
        /// <summary>
        /// Return the connection strings from items
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static MySQLConnectionString GetMySQLConnectionString(this HttpContext context)
        {
            var data = context.Items.FirstOrDefault(x => x.Key as string == "DewDatabaseConnectionString");
            return data.Equals(default(KeyValuePair<object, object>)) ? null : data.Value as MySQLConnectionString;
        }
        /// <summary>
        /// Returns the tableprefix from items
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetMySQLTablePrefix(this HttpContext context)
        {
            var data = context.Items.FirstOrDefault(x => x.Key as string == "DewDatabaseTablePrefix");
            return data.Equals(default(KeyValuePair<object, object>)) ? null : data.Value as string;
        }
    }
    /// <summary>
    /// DewMySQLClient pipeline builder extension
    /// </summary>
    public static class DewMySQLClientBuilderExtension
    {
        /// <summary>
        /// Builder method
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="cs"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDewMySQLClient(
           this IApplicationBuilder builder, MySQLConnectionString cs, string tp = null)
        {
            return builder.UseMiddleware<DatabaseMiddleware>(cs, tp);
        }
    }
}
