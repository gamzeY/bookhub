namespace backend.Dtos;

public record UpdateBookRequest(
    int Rating,
    string? Comments
);
