using System.Text.RegularExpressions;
using NovaCore.Common.Extensions;

namespace NovaCore.Common.CLI;

public static class CommandParser
{
    private static readonly Regex SanitizePattern = new(@"[^\w\.@-_""''\s]", RegexOptions.Compiled);
        
    private static readonly Regex TokenizePattern = 
        new("(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)", RegexOptions.Compiled);
        
    public enum ParserResult
    {
        Empty,
        SanitizeFailed,
        TokenizeFailed,
        Success
    }
        
    public static ParserResult Parse(string input, out Command? command)
    {
        command = null;
            
        // Ignore a blank input
        if (input.IsNullOrEmpty())
        {
            return ParserResult.Empty;
        }
            
        // Remove all invalid characters
        string sanitizedInput = Sanitize(input);
        if (sanitizedInput.IsNullOrEmpty())
        {
            return ParserResult.SanitizeFailed;
        }
            
        // Break the input into tokens (command, arguments, ect.)
        if (Tokenize(sanitizedInput) is not { } tokens)
        {
            return ParserResult.TokenizeFailed;
        }

        // Formatting input into the Unix CLI Format
        command = new Command(tokens);

        return ParserResult.Success;
    }

    public static string Sanitize(string input)
    {
        return SanitizePattern.Remove(input);
    }

    // https://stackoverflow.com/questions/4780728/regex-split-string-preserving-quotes/4780801#4780801
    // https://stackoverflow.com/questions/14655023/split-a-string-that-has-white-spaces-unless-they-are-enclosed-within-quotes
    public static string[] Tokenize(string input)
    {
        return TokenizePattern.Split(input);
    }
}