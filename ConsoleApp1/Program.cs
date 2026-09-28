using ConsoleApp1;
using Microsoft.SemanticKernel;
using System.IO;
using System;

// Load the settings. 
var (apiKey, orgId) = Settings.LoadFromFile();

Kernel kernel = Kernel.CreateBuilder()
                        .AddGoogleAIGeminiChatCompletion("gemini-flash-lite-latest", apiKey)
                        .Build();

// 1. Load our Semantic Functions from the directory!
string pluginPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins", "jokes");
if (!Directory.Exists(pluginPath)) 
{
    pluginPath = Path.Combine(AppContext.BaseDirectory, "Plugins", "jokes");
}
var jokesPlugin = kernel.CreatePluginFromPromptDirectory(pluginPath, "JokesPlugin");

// 2. Load our C# Native Function into the Kernel
var showManagerPlugin = kernel.ImportPluginFromObject(new ConsoleApp1.Plugins.ShowManager(), "ShowManager");


Console.WriteLine("==================================================");
Console.WriteLine("CONCEPT 1: MANUAL CHAINING (Like a Micromanager)");
Console.WriteLine("==================================================");

// 3. Ask the Kernel to run our C# Native Function to get a random word
var result = await kernel.InvokeAsync(showManagerPlugin["RandomTheme2"]);
Console.WriteLine("I will tell a joke about " + result);

// 4. Pass the random word to the Knock Knock Semantic Function
var arguments = new KernelArguments() { ["input"] = result };
var joke = await kernel.InvokeAsync(jokesPlugin["knock_knock_joke"], arguments);
Console.WriteLine(joke);

// 5. Pass the resulting joke to the Explain Joke Semantic Function
var explainArguments = new KernelArguments() { ["input"] = joke.ToString() };
var explanation = await kernel.InvokeAsync(jokesPlugin["explain_joke"], explainArguments);
Console.WriteLine(explanation);


Console.WriteLine();
Console.WriteLine("==================================================");
Console.WriteLine("CONCEPT 2: AUTOMATIC FUNCTION CALLING (The Planner)");
Console.WriteLine("==================================================");

try 
{
    // Enable AUTOMATIC FUNCTION CALLING!
    var executionSettings = new PromptExecutionSettings
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
    };

    string userGoal = "Use the ShowManager to get a random word, then write a knock-knock joke about it, and finally explain why it is funny.";
    
    // Give the AI a single goal in plain English, and watch it figure out the steps!
    var autoResult = await kernel.InvokePromptAsync(userGoal, new KernelArguments(executionSettings));
    Console.WriteLine(autoResult);
}
catch (Exception ex)
{
    Console.WriteLine("Auto Function Calling failed. (Note: The Gemini Alpha connector often throws a 400 Bad Request here because it is still in preview and doesn't fully support this SK feature yet!)");
    Console.WriteLine("Error Details: " + ex.Message);
}
