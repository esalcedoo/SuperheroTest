using Bot.Builder.Community.Adapters.Alexa.Directives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperheroTest.QnA
{
    public static class QnAAnswerAlexaExtensions
    {
        internal static IAlexaDirective ToAlexaDirective(this QnAAnswerModel qnAAnswer)
        {
            var directive = new DisplayDirective();
            if (qnAAnswer.IsFinal())
            {
                directive.Template = qnAAnswer.GenerateImageTemplate();
            }
            else
            {
                List<ListItem> listItems = new List<ListItem>();
                ListItem listItem;

                IEnumerable<string> choices = qnAAnswer.Context?.Prompts?.Select(p => p.DisplayText) ?? Enumerable.Empty<string>();

                foreach (string choice in choices)
                {
                    listItem = new ListItem()
                    {
                        Token = choice,
                        TextContent = new TextContent()
                        {
                            PrimaryText = new InnerTextContent()
                            {
                                Text = choice,
                                Type = TextContentType.RichText
                            }
                        }
                    };
                    listItems.Add(listItem);
                }

                directive.Template = qnAAnswer.GenerateDisplayListTemplate(listItems);
            }
            return directive;
        }

        private static DisplayRenderBodyTemplate1 GenerateImageTemplate(this QnAAnswerModel qnAAnswer)
        {
            var displayTemplate = new DisplayRenderBodyTemplate1()
            {
                BackButton = BackButtonVisibility.HIDDEN,
                Title = string.Empty,
                Token = "string",
            };
            displayTemplate.BackgroundImage = new Image()
            {
                ContentDescription = "background",
                Sources = new ImageSource[]
                    {
                        new ImageSource()
                        {
                            Url = qnAAnswer.Answer.Image
                        }
                    }
            };
            return displayTemplate;
        }

            private static DisplayRenderListTemplate1 GenerateDisplayListTemplate(this QnAAnswerModel qnAAnswer, List<ListItem> listItems)
        {
            var displayListTemplate = new DisplayRenderListTemplate1()
            {
                BackButton = BackButtonVisibility.HIDDEN,
                Title = string.Empty,
                Token = "string",
            };

            if (listItems.Count > 0)
            {
                displayListTemplate.ListItems = listItems;
            }

            if (string.IsNullOrEmpty(qnAAnswer.Answer.Image))
            {
                qnAAnswer.Answer.Image = "https://esalcedoost.blob.core.windows.net/superhero/dc-comics-universe.jpg";
            };

            displayListTemplate.BackgroundImage = new Image()
            {
                ContentDescription = "background",
                Sources = new ImageSource[]
                    {
                        new ImageSource()
                        {
                            Url = qnAAnswer.Answer.Image
                        }
                    }
            };

            return displayListTemplate;
        }
    }
}
