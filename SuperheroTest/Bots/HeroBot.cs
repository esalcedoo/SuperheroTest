using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace SuperheroTest.Bots
{
    public class HeroBot<T> : ActivityHandler where T : Dialog
    {
        private ILogger<HeroBot<T>> _logger;
        private ConversationState _conversationState;
        private T _dialog;

        public HeroBot(T dialog, ILogger<HeroBot<T>> logger, ConversationState conversationState)
        {
            _logger = logger;
            _conversationState = conversationState;
            _dialog = dialog;
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            DialogSet dialogSet = new DialogSet(_conversationState.CreateProperty<DialogState>("StoryState"));

            dialogSet.Add(_dialog);
            DialogContext dialogContext =
                await dialogSet.CreateContextAsync(turnContext, cancellationToken);

            DialogTurnResult results =
                await dialogContext.ContinueDialogAsync(cancellationToken);

            if (results.Status == DialogTurnStatus.Empty)
            {
                await dialogContext.BeginDialogAsync(_dialog.Id, null, cancellationToken);
            }
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello .NET Conf!"), cancellationToken);
                }
            }
        }
    }
}
