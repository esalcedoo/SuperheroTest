using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Linq;

namespace SuperheroTest.QnA
{
    public static class QnAAnswerExtensions
    {
        internal static IActivity ToActivity(this QnAAnswerModel qnAAnswer)
        {
            IEnumerable<string> suggestedActions = qnAAnswer.Context?.Prompts?.Select(p => p.DisplayText);
            IMessageActivity activity;

            if (suggestedActions != null)
            {
                activity = MessageFactory.SuggestedActions(suggestedActions, qnAAnswer.Answer.Text, qnAAnswer.Answer.SSML);
            }
            else
            {
                activity = MessageFactory.Text(qnAAnswer.Answer.Text, qnAAnswer.Answer.SSML);
            }

            activity.InputHint = qnAAnswer.IsFinal() ? InputHints.IgnoringInput : InputHints.ExpectingInput;

            return activity;
        }

        internal static bool IsFinal(this QnAAnswerModel qnAAnswer)
        {
            return !qnAAnswer.Answer.Image.Equals("https://esalcedoost.blob.core.windows.net/superhero/dc-comics-universe.jpg");
        }
    }
}
