namespace backend.Validation;

public static class BookRules
{
    public const int MaxBooks = 25;

    public const int TitleMax = 120;
    public const int AuthorMax = 80;
    public const int IsbnMax = 20;
    public const int CommentsMax = 500;
    public const int CoverUrlMax = 300;

    public static bool ContainsForbiddenWord(string? text)
        => !string.IsNullOrWhiteSpace(text)
           && text.Contains("horrible", StringComparison.OrdinalIgnoreCase);
}
