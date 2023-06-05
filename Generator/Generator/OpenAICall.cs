using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using System.ComponentModel;

namespace OpenAI.Playground.TestHelpers;

internal static class OpenAICall
{
    public static async Task<string> CallOpenAIAPI(IOpenAIService sdk, string prompt)
    {
        string result = "";

        try
        {
            Console.WriteLine("Generating...");
            var completionResult = await sdk.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem(prompt),

            },
                MaxTokens = 2048,
                Model = Models.ChatGpt3_5Turbo
            });

            if (completionResult.Successful)
            {
                result = completionResult.Choices.First().Message.Content;
                Console.WriteLine(result);
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                result = $"{completionResult.Error.Code}: {completionResult.Error.Message}";
                Console.WriteLine(result);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return result;
    }
}