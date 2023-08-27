using System.Reflection;
using FundealSlack;
using SlackAPI;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// API 키 점검
var slackApiToken = config["Slack:ApiToken"];
if (slackApiToken is not { Length: > 0 })
{
    throw new SlackConnectionException();
}

var slackClient = new SlackTaskClient(slackApiToken);
var loginResponseAsync = slackClient.ConnectAsync();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddTransient(async (services) =>
{
    var loginResponse = await slackClient.ConnectAsync();
    if (!loginResponse.ok)
        throw new SlackConnectionException(loginResponse.error);
    return slackClient;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var loginResponse = await loginResponseAsync;
if (!loginResponse.ok)
{
    throw new SlackConnectionException(loginResponse.error);
}

app.Run();