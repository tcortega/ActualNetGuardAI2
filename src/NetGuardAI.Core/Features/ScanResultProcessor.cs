using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

namespace NetGuardAI.Core.Features;

[RegisterScoped]
public class ScanResultProcessor(IOpenAIService service)
{
    public async Task<ProcessedResult> ProcessScanResultAsync(string nmapResult, string? httpContent)
    {
        var formattedPrompt = Constants.DefaultPrompts.Replace("{report}", nmapResult)
            .Replace("{responseBody}", httpContent ?? "No HTTP response body was provided.");
        var messages = new List<ChatMessage>
        {
            ChatMessage.FromSystem(
                "You are a server admin and you just received a scan result from your network scanner. You need to analyze it and write a report."),
            ChatMessage.FromUser(formattedPrompt)
        };
        
        var completionResult = await service.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = messages,
            MaxTokens = 3000,
            Model = Models.Gpt_4_1106_preview
        });

        if (!completionResult.Successful)
        {
            throw new Exception("TODO: Handle this error");
        }

        return new ProcessedResult(nmapResult, completionResult.Choices.First().Message.Content);
    }

    private static class Constants
    {
        public const string DefaultPrompts = """
                                             Perform an analysis on the provided NMAP scan result and HTTP Response Body.
                                             Your output MUST be returned in a JSON format according to the provided standards.
                                             The data must be accurate in regards towards a report.
                                             The data will not include vulnerability details.
                                             The data must be according to the following guidelines:
                                             1) The scan analysis must be done from a server admin point of view.
                                             2) The final output must be informational according to the format given.
                                             3) If a value is not found or you are not able to fill in a specific json field, fill with an empty string.
                                             4) Analyze everything, even the smallest of data.
                                             5) Completely analyze the data provided and give a confirm answer using the output format.
                                             6) The HTTP Response Body can be of VERY IMPORTANCE as it highlights the server's response to the scan.
                                             8) Your answer must ALWAYS be a clean and raw json in the specified format. You can not reply in code blocks.
                                             9) Your output must be in BRAZILIAN PORTUGUESE.
                                             
                                             Output format:
                                             {
                                               "host_ip": "string",
                                               "host_port": "int",
                                               "scan_datetime": "YYYY-MM-DDThh:mm:ssZ",
                                               "service_name": "string",
                                               "service_version": "string",
                                               "state": "string (open/closed/filtered)",
                                               "os_details": {
                                                 "os_name": "string",
                                                 "os_accuracy": "string",
                                                 "os_family": "string"
                                               },
                                               "host_classification": "string",
                                               "additional_info": {
                                                 "mac_address": "string",
                                                 "vendor": "string",
                                                 "uptime": "string",
                                                 "last_reboot": "string"
                                               },
                                               "additional_notes": "string" // In here you'd add your own notes predicting what the server probably is, or any insights you might have
                                             }
                                             
                                             HTTP Response Body: {responseBody} 
                                             Nmap XML report: {report}
                                             """;
    }
}

public record ProcessedResult(string RawInput, string Output);