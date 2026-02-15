namespace backend.Dtos;

public record CreateBookRequest(
    string Title,
    string Author,
    string Isbn,
    int? Rating,
    string? Comments,
    string? CoverImageUrl
);
