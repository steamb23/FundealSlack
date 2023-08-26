namespace FundealSlack;

public static class ParseHelper
{
    private static readonly char[] WhiteSpace = { ' ', '\t' };

    public static List<string> ParseArguments(string rawText)
    {
        // 화이트스페이스 제거
        if (rawText is { Length: > 0 } && rawText[0] is ' ' or '\t')
            rawText = rawText.Trim(WhiteSpace);

        var results = new List<string>();

        var quoteMark = '\0';
        for (int i = 0; i < rawText.Length; i++)
        {
            var character = rawText[i];
            if (quoteMark == '\0')
            {
                // 따옴표 모드가 아닐 때
                // 따옴표 체크
                if (character is '\'' or '\"' &&
                    (i < 1 || rawText[i - 1] != '\\'))
                {
                    quoteMark = character;
                    continue;
                }

                // 따옴표가 아닌 경우 다음 공백까지 파싱
                var whiteSpaceIndex = rawText.IndexOfAny(WhiteSpace, i);
                if (whiteSpaceIndex == i)
                {
                    // 같은 문자를 가리키고 있을 경우 넘김
                    continue;
                }

                if (whiteSpaceIndex < 0)
                    whiteSpaceIndex = rawText.Length;
                // 다음 공백까지 파싱
                results.Add(rawText.Substring(i, whiteSpaceIndex - i));
                i = whiteSpaceIndex;
            }
            else
            {
                // 따옴표 모드 일때
                // 다음 따옴표 파악
                var nextQuoteMarkIndex = rawText.IndexOf(quoteMark, i);
                if (nextQuoteMarkIndex < 0)
                    nextQuoteMarkIndex = rawText.Length;
                // 다음 따옴표나 문자열 끝까지 파싱
                results.Add(rawText.Substring(i, nextQuoteMarkIndex - i));
                quoteMark = '\0';

                i = nextQuoteMarkIndex;
            }
        }

        return results;
    }
}