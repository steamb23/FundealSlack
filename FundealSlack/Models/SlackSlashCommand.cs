using Microsoft.AspNetCore.Mvc;

namespace FundealSlack.Models;

public record SlackSlashCommand(
    [FromForm(Name = "team_id")] string? TeamId,
    [FromForm(Name = "team_domain")] string? TeamDomain,
    [FromForm(Name = "enterprise_id")] string? EnterpriseId,
    [FromForm(Name = "enterprise_name")] string? EnterpriseName,
    [FromForm(Name = "channel_id")] string? ChannelId,
    [FromForm(Name = "channel_name")] string? ChannelName,
    [FromForm(Name = "user_id")] string? UserId,
    [FromForm(Name = "user_name")] string? UserName,
    [FromForm(Name = "command")] string? Command,
    [FromForm(Name = "text")] string? Text,
    [FromForm(Name = "response_url")] string? ResponseUrl,
    [FromForm(Name = "trigger_id")] string? TriggerId,
    [FromForm(Name = "api_app_id")] string? ApiAppId
);