using System;
using System.Threading;
using System.Threading.Tasks;
using Bot.Builder.Community.Adapters.Alexa;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using SuperheroTest.QnA;

namespace SuperheroTest.Dialogs
{
    public class HeroDialog : ComponentDialog
    {
        private readonly QnAService _qnaService;
        private QnAAnswerModel _qnaResult;

        public HeroDialog(QnAService qnaService) : base(nameof(HeroDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                GetQnAAsync,
                SendQuestionAsync
            }));

            InitialDialogId = nameof(WaterfallDialog);
            _qnaService = qnaService;
        }


        public async Task<DialogTurnResult> GetQnAAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
        {
            // Call QnA Maker and get results.
            _qnaResult = await _qnaService.GenerateAnswer(stepContext.Context.Activity.Text, stepContext.Context.Activity.Locale);

            if (_qnaResult == null)
            {
                // No answer found.
                await stepContext.Context.SendActivityAsync("No contemplamos esa opción.");
                return await stepContext.EndDialogAsync();
            }
            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> SendQuestionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_qnaResult.Answer.Text))
            {
                if (stepContext.Context.Activity.ChannelId.Equals("Alexa", StringComparison.InvariantCultureIgnoreCase)
                        && stepContext.Context.AlexaDeviceHasDisplay())
                {
                    stepContext.Context.AlexaResponseDirectives().Add(_qnaResult.ToAlexaDirective());
                }
                await stepContext.Context.SendActivityAsync(_qnaResult.ToActivity(), cancellationToken);
            }
            return await stepContext.EndDialogAsync();
        }
    }
}
