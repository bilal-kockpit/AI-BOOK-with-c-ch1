using ConsoleApp1;
using Microsoft.SemanticKernel;

// Load the settings. You will need to replace the OpenAI API key in settings.json with a Gemini API key.
// The orgId is not needed for Gemini, but we keep it here so Settings.LoadFromFile doesn't break.
var (apiKey, orgId) = Settings.LoadFromFile();

// Using Gemini Pro! 
// Note: gemini-1.5-flash is the fast/free one, and gemini-1.5-pro is the advanced one.
//Kernel kernel = Kernel.CreateBuilder()
//                        .AddGoogleAIGeminiChatCompletion("gemini-3.6-flash", apiKey)
//                        .Build();

Kernel kernel = Kernel.CreateBuilder()
                        .AddGoogleAIGeminiChatCompletion("gemini-flash-lite-latest", apiKey)
                        .Build();

string prompt = "Tell me about tej mahal ";

var joke = await kernel.InvokePromptAsync(prompt);

Console.WriteLine(joke);
