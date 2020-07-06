﻿using System;

using HttpReports;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IHttpReportsInitializer InitHttpReports(this IApplicationBuilder app)
        {
            return new HttpReportsInitializer(app);
        }

        public static IHttpReportsInitializer InitStorage(this IHttpReportsInitializer initializer)
        {
            var storage = initializer.ApplicationBuilder.ApplicationServices.GetRequiredService<IHttpReportsStorage>() ?? throw new ArgumentNullException("Storage Service Not Found");
            storage.InitAsync().Wait();

            return initializer;
        }

        public static IHttpReportsBuilder UseDirectlyReportsTransport(this IHttpReportsBuilder builder)
        {  
            builder.Services.RemoveAll<IReportsTransport>();
            builder.Services.AddSingleton<IReportsTransport,DirectlyReportsTransport>();
            builder.Services.RemoveAll<IModelCreator>();
            builder.Services.AddSingleton<IModelCreator, DefaultModelCreator>(); 

            return builder;
        } 
    }
}