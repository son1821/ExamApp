﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Admin.Configuration.Test
{
	public class StartupTest : Startup
    {
        public StartupTest(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration)
        {
        }

        public  void ConfigureUIOptions(IdentityServer4AdminUIOptions options)
        {
            base.ConfigureUIOptions(options);

            // Use staging DbContexts and auth services.
            options.Testing.IsStaging = true;
        }
    }
}
