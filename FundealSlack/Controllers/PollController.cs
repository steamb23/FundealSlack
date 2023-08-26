using Microsoft.AspNetCore.Mvc;

namespace FundealSlack.Controllers;

[ApiController]
public class PollController : Controller
{
    [HttpPost]
    public IActionResult PostIndex([FromForm] string? text, [FromForm(Name = "channel_id")] string? channelId)
    {
        return Ok();
    }

    public static PollApiParameter? ParseApiParameter(string rawText)
    {
        var args = ParseHelper.ParseArguments(rawText);

        return args.Count > 0 ? new PollApiParameter(args[0], args.GetRange(1, args.Count - 1)) : null;
    }

    // private PollApiParameter ParseCommandParameterWithQuote(string rawText)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // private PollApiParameter ParseCommandParameterWithoutQuote(string rawText)
    // {
    //     var splitText = rawText.Split(' ', '\t', StringSplitOptions.RemoveEmptyEntries);
    //     return new PollApiParameter(splitText[0], splitText[1..]);
    // }

    public record PollApiParameter(string Name, IList<string>? Items);
}