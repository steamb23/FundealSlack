using System.Text;
using FundealSlack.Models;
using Microsoft.AspNetCore.Mvc;
using SlackAPI;

namespace FundealSlack.Controllers;

[ApiController]
public class PollController : Controller
{
    private ILogger<PollController> Logger { get; }
    private IConfiguration Config { get; }
    private SlackTaskClient SlackClient { get; }

    public PollController(ILogger<PollController> logger, IConfiguration config, SlackTaskClient slackTaskClient)
    {
        Logger = logger;
        Config = config;
        SlackClient = slackTaskClient;
    }

    [HttpPost]
    public async Task<IActionResult> PostIndex(SlackSlashCommand model)
    {
        if (model.Text == null)
            return BadRequest();

        var parameter = ParseApiParameter(model.Text);
        if (parameter == null)
            return BadRequest();

        var blocks = MakeBlocks(parameter);
        if (blocks == null)
            return BadRequest();

        var postMessageResponse = await SlackClient.PostMessageAsync(model.ChannelId, $"투표: {parameter.Title}",
            botName: "투표맨",
            icon_emoji: "📊",
            blocks: blocks);

        if (!postMessageResponse.ok)
            return BadRequest();

        return Ok();
    }

    private const string MarkdownType = "mrkdwn";
    private const string PlainTextType = "plain_text";

    public IBlock[]? MakeBlocks(PollApiParameter parameter)
    {
        var (title, items, peopleList) = parameter;

        title = string.IsNullOrEmpty(title) ? "투표" : $"투표: {title}";

        if (items is not { Count: > 0 })
            return null;

        var blocks = new List<IBlock>
        {
            new HeaderBlock
            {
                text = new Text
                {
                    type = MarkdownType,
                    text = title
                }
            }
        };

        for (var i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var people = peopleList?[i];

            string? peopleMessage = null;
            string? peopleValue = null;
            if (people != null)
            {
                peopleMessage = string.Join(' ', people.Select(s => $"<@{s}>"));
                peopleValue = string.Join(' ', people);
            }

            var sectionBlock = new SectionBlock
            {
                text = new Text
                {
                    type = MarkdownType,
                    text = $"{i + 1}. {item}\n{peopleMessage}",
                    emoji = true
                },
                accessory = new ButtonElement()
                {
                    text = new Text
                    {
                        type = PlainTextType,
                        text = (i + 1).ToString(),
                        emoji = true
                    },
                    value = peopleValue,
                    action_id = $"poll_{i}"
                }
            };

            blocks.Add(sectionBlock);
        }

        return blocks.ToArray();
    }

    public static PollApiParameter? ParseApiParameter(string rawText)
    {
        var args = ParseHelper.ParseArguments(rawText);

        return args.Count > 0
            ? new PollApiParameter(args[0], args.Count > 1
                    ? args.GetRange(1, args.Count - 1)
                    : null,
                null)
            : null;
    }

    public record PollApiParameter(string Title, IList<string>? Items, IList<IList<string>>? PeopleList);
}