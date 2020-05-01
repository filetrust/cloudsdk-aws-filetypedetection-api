using System;
using System.Diagnostics.CodeAnalysis;
using Glasswall.CloudSdk.AWS.Common.Web;
using Glasswall.CloudSdk.Common;
using Glasswall.Core.Engine;
using Glasswall.Core.Engine.Common.FileProcessing;
using Glasswall.Core.Engine.Common.GlasswallEngineLibrary;
using Glasswall.Core.Engine.FileProcessing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Glasswall.CloudSdk.AWS.FileTypeDetection
{
    [ExcludeFromCodeCoverage]
    public class Startup : AwsCommonStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void ConfigureAdditionalServices(IServiceCollection services)
        {
            services.AddSingleton<IGlasswallVersionService, GlasswallVersionService>();
            services.AddSingleton<IMetricService, MetricService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IFileTypeDetector, FileTypeDetector>();

            var p = (int)Environment.OSVersion.Platform;

            if ((p == 4) || (p == 6) || (p == 128))
            {
                services.AddSingleton<IGlasswallFileOperations, LinuxEngineOperations>();
            }
            else
            {
                services.AddSingleton<IGlasswallFileOperations, WindowsEngineOperations>();
            }
        }

        protected override void ConfigureAdditional(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}