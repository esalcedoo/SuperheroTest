using Bot.Builder.Community.Adapters.Alexa.Integration.AspNet.Core;
using Bot.Builder.Community.Adapters.Alexa.Middleware;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SuperheroTest.Bots;
using SuperheroTest.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AlexaServiceCollectionExtension
    {
        public static IServiceCollection AddAlexa(this IServiceCollection services)
        {
            // The Dialog that will be run by the bot.
            services.TryAddTransient<HeroDialog>();

            // Create the Alexa bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.TryAddTransient<AlexaHeroBot<HeroDialog>>();
            
            services.TryAddSingleton<IAlexaHttpAdapter>(_ =>
            {
                var alexaHttpAdapter = new AlexaHttpAdapter(validateRequests: true)
                {
                    OnTurnError = async (context, exception) =>
                    {
                        await context.SendActivityAsync("<say-as interpret-as=\"interjection\">boom</say-as>, explotó.");
                    },
                    ShouldEndSessionByDefault = false,
                    ConvertBotBuilderCardsToAlexaCards = false,
                };
                alexaHttpAdapter.Use(new AlexaIntentRequestToMessageActivityMiddleware(transformPattern: RequestTransformPatterns.MessageActivityTextFromSinglePhraseSlotValue));
                return alexaHttpAdapter;
            });

            return services;
        }
    }
}
