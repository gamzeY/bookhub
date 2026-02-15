using System;

namespace backend.Models;

public class Book
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Isbn { get; set; }
    public int? Rating { get; set; }
    public string? Comments { get; set; }
    public string? CoverImageUrl { get; set; }
}
