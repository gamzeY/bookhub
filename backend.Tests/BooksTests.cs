using System.Net;
using System.Net.Http.Json;
using backend.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace backend.Tests;

public class BooksTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BooksTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private static CreateBookRequest ValidBook(int i) =>
        new(
            Title: $"Book {i}",
            Author: "Author",
            Isbn: $"ISBN-{i}",
            Rating: 5,
            Comments: "Great book",
            CoverImageUrl: null
        );

    [Fact]
    public async Task Cannot_Add_More_Than_25_Books()
    {
        for (int i = 0; i < 25; i++)
        {
            var res = await _client.PostAsJsonAsync("/api/books", ValidBook(i));
            Assert.True(res.IsSuccessStatusCode);
        }

        var response = await _client.PostAsJsonAsync("/api/books", ValidBook(26));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}
