namespace FundealSlack;

public class SlackConnectionException : Exception
{
    private const string CommonMessage = "Unable to connect to Slack, check your api token settings.";

    public SlackConnectionException()
        : base(CommonMessage)
    {
    }

    public SlackConnectionException(string? message)
        : base($"{CommonMessage}{(string.IsNullOrEmpty(message) ? null : ' ')}{message}")
    {
    }

    public SlackConnectionException(string? message, Exception? inner)
        : base($"{CommonMessage}{(string.IsNullOrEmpty(message) ? null : ' ')}{message}", inner)
    {
    }
}