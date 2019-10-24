using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace SuperheroTest.Dialogs
{
    public class HeroDialog : ComponentDialog
    {
        public HeroDialog() : base(nameof(HeroDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                Echo
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> Echo(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            IMessageActivity activity = MessageFactory.Text($"Echo: {stepContext.Context.Activity.Text}");

            return await stepContext.PromptAsync(nameof(TextPrompt),
                    new PromptOptions()
                    {
                        Prompt = activity as Activity
                    },
                    CancellationToken.None);
        }

    }
}
