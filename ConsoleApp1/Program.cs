using ConsoleApp1;
using Microsoft.SemanticKernel;

var (apiKey, orgId) = Settings.LoadFromFile();

Kernel kernel = Kernel.CreateBuilder()
    .AddGoogleAIGeminiChatCompletion("gemini-flash-lite-latest", apiKey)
    .Build();

// 1. Point to the plugins directory
string pluginsDirectory = Path.Combine(AppContext.BaseDirectory, "Plugins", "prompt_engineering");

// 2. Load the plugin into the kernel
var travelPlugin = kernel.CreatePluginFromPromptDirectory(pluginsDirectory);

// 3. Set the multiple arguments (fills in {{$city}}, {{$n_days}}, {{$likes}}, {{$dislikes}}, {{$n_attractions}})
var arguments = new KernelArguments()
{
    ["city"] = "Paris",
    ["n_days"] = "3",
    ["likes"] = "art, architecture, and French pastries",
    ["dislikes"] = "crowds and heights",
    ["n_attractions"] = "3"
};

// 4. Run the function!
var result = await kernel.InvokeAsync(travelPlugin["attractions_multiple_variables"], arguments);

Console.WriteLine(result);