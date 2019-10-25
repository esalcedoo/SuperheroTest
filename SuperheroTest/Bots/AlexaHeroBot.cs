using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using SuperheroTest.Alexa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SuperheroTest.Bots
{
    public class AlexaHeroBot<T> : AlexaActivityHandler where T : Dialog
    {
        public T _dialog { get; }
        public ILogger<HeroBot<T>> _logger { get; }
        public ConversationState _conversationState { get; }

        public AlexaHeroBot(T dialog, ILogger<HeroBot<T>> logger, ConversationState conversationState)
        {
            _dialog = dialog;
            _logger = logger;
            _conversationState = conversationState;
        }

        protected override async Task OnMessageActivityAsync(DelegatingTurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await _dialog.RunAsync(turnContext, _conversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(DelegatingTurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var messageActivity = MessageFactory.Text("¡¡¡NO HAY SIESTA .NET Conf BCN!!!");
            await turnContext.SendActivityAsync(messageActivity, cancellationToken);
        }
    }
}
