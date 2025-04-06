using AIChatApplication.Hubs;
using AIChatApplication.Plugins;
using AIChatApplication.Services;
using AIChatApplication.WievModels;
using Microsoft.SemanticKernel;
using System.ClientModel;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services
    .AddKernel()
    .AddOpenAIChatCompletion(
        modelId: "google/gemini-2.5-pro-exp-03-25:free",
        openAIClient: new OpenAI.OpenAIClient(
            credential: new ApiKeyCredential("*****"),
            options: new OpenAI.OpenAIClientOptions
            {
                Endpoint = new Uri("https://openrouter.ai/api/v1")
            }
            )

    )
    .Plugins.AddFromType<CalculatorPlugin>()
    ;

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy.AllowAnyMethod()
                                       .AllowAnyHeader()
                                       .AllowCredentials()
                                       .SetIsOriginAllowed(s => true)));

builder.Services.AddSignalR();
builder.Services.AddScoped<AIService>();

var app = builder.Build();

app.UseCors();

app.MapPost("/chat", async (AIService aiService, ChatRequestVM chatRequest, CancellationToken cancellationToken)
    => await aiService.GetMessageStreamAsync(chatRequest.Prompt, chatRequest.ConnectionId, cancellationToken));

app.MapGet("/", () => "Hello World");

app.MapHub<AIHub>("ai-hub");

app.Run();
