using System;
using archives.service.biz.ifs;
using archives.service.biz.impl;
using Microsoft.Extensions.DependencyInjection;

namespace archives.service.biz
{
    public static class Bizinjection
    {
        public static void AddBizService(this IServiceCollection services)
        {
            services.AddScoped<IArchivesInfoService, ArchivesInfoService>();
        }
    }
}
