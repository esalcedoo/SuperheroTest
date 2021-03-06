﻿using Microsoft.Bot.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SuperheroTest.Dialogs;
using SuperheroTest.QnA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class QnAServiceCollectionExtension
    {
        public static IServiceCollection AddQnAService(this IServiceCollection services, Action<QnAMakerService> setup = null)
        {
            services.AddOptions();
            if (!(setup is null))
            {
                services.Configure(setup);
            }

            services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IPostConfigureOptions<QnAMakerService>, QnAPostConfigureOptions>());

            services.AddHttpClient<QnAService>((provider, client) =>
            {
                var qnAMakerServiceConfig = provider.GetRequiredService<IOptions<QnAMakerService>>().Value;

                client.BaseAddress = new Uri($"{qnAMakerServiceConfig.Hostname}/knowledgebases/{qnAMakerServiceConfig.KbId}/");
                client.DefaultRequestHeaders.Add("Authorization", $"EndpointKey {qnAMakerServiceConfig.EndpointKey}");
            });

            services.AddSingleton<HeroDialog>();

            return services;
        }

    }
}
