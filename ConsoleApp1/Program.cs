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

// 1. Load our Semantic Functions from the directory!
string pluginPath = Path.Combine(Directory.GetCurrentDirectory(), "plugins", "jokes");
if (!Directory.Exists(pluginPath)) 
{
    pluginPath = Path.Combine(AppContext.BaseDirectory, "plugins", "jokes");
}
var jokesPlugin = kernel.CreatePluginFromPromptDirectory(pluginPath);

// 2. Load our C# Native Function into the Kernel
var showManagerPlugin = kernel.ImportPluginFromObject(new ConsoleApp1.Plugins.ShowManager());

// 3. Ask the Kernel to run our C# Native Function to get a random word
// (Using RandomTheme2 since you just added it!)
var result = await kernel.InvokeAsync(showManagerPlugin["RandomTheme2"]);
Console.WriteLine("I will tell a joke about " + result);

// 4. Pass the random word to the Knock Knock Semantic Function
var arguments = new KernelArguments() { ["input"] = result };
var joke = await kernel.InvokeAsync(jokesPlugin["knock_knock_joke"], arguments);

Console.WriteLine();
Console.WriteLine("--- JOKE ---");
Console.WriteLine(joke);

// 5. Pass the resulting joke to the Explain Joke Semantic Function
var explainArguments = new KernelArguments() { ["input"] = joke.ToString() };
var explanation = await kernel.InvokeAsync(jokesPlugin["explain_joke"], explainArguments);

Console.WriteLine();
Console.WriteLine("--- EXPLANATION ---");
Console.WriteLine(explanation);