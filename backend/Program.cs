using backend.Dtos;
using backend.Models;
using backend.Services;
using backend.Validation;
using FluentValidation;
using FluentValidation.Results;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (frontend bağlanınca rahat)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// DI
builder.Services.AddSingleton<InMemoryBookStore>();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookRequestValidator>();

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

static IResult ValidationProblem(ValidationResult result)
{
    var errors = result.Errors
        .GroupBy(e => e.PropertyName)
        .ToDictionary(
            g => g.Key,
            g => g.Select(e => e.ErrorMessage).ToArray()
        );

    return Results.ValidationProblem(errors);
}

// GET all with pagination + search + sort
app.MapGet("/api/books", (InMemoryBookStore store,
    string? search,
    string? sort,
    int page = 1,
    int pageSize = 10) =>
{
    if (page < 1) page = 1;
    if (pageSize < 1) pageSize = 10;
    if (pageSize > 50) pageSize = 50;

    var query = store.GetAllSnapshot().AsQueryable();

    if (!string.IsNullOrWhiteSpace(search))
    {
        var s = search.Trim();
        query = query.Where(b =>
            b.Title.Contains(s, StringComparison.OrdinalIgnoreCase) ||
            b.Author.Contains(s, StringComparison.OrdinalIgnoreCase));
    }

    // sort by title (default)
    var sortKey = (sort ?? "title").Trim().ToLowerInvariant();
    query = sortKey switch
    {
        "title" => query.OrderBy(b => b.Title),
        _ => query.OrderBy(b => b.Title)
    };

    var totalCount = query.Count();
    var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

    var items = query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

    return Results.Ok(new PagedResult<Book>(items, page, pageSize, totalCount, totalPages));
});

// GET by id
app.MapGet("/api/books/{id:guid}", (InMemoryBookStore store, Guid id) =>
{
    var book = store.GetById(id);
    return book is null ? Results.NotFound(new { message = "Book not found." }) : Results.Ok(book);
});

// POST create
app.MapPost("/api/books", async (InMemoryBookStore store,
    IValidator<CreateBookRequest> validator,
    CreateBookRequest request) =>
{
    if (store.Count >= BookRules.MaxBooks)
        return Results.Conflict(new { message = $"You can only store up to {BookRules.MaxBooks} books." });

    var validation = await validator.ValidateAsync(request);
    if (!validation.IsValid) return ValidationProblem(validation);

    var book = new Book
    {
        Title = request.Title.Trim(),
        Author = request.Author.Trim(),
        Isbn = request.Isbn.Trim(),
        Rating = request.Rating,
        Comments = string.IsNullOrWhiteSpace(request.Comments) ? null : request.Comments.Trim(),
        CoverImageUrl = string.IsNullOrWhiteSpace(request.CoverImageUrl) ? null : request.CoverImageUrl.Trim()
    };

    store.Add(book);
    return Results.Created($"/api/books/{book.Id}", book);
});

// PUT update (rating/comments only)
app.MapPut("/api/books/{id:guid}", async (InMemoryBookStore store,
    IValidator<UpdateBookRequest> validator,
    Guid id,
    UpdateBookRequest request) =>
{
    var book = store.GetById(id);
    if (book is null) return Results.NotFound(new { message = "Book not found." });

    var validation = await validator.ValidateAsync(request);
    if (!validation.IsValid) return ValidationProblem(validation);

    book.Rating = request.Rating;
    book.Comments = string.IsNullOrWhiteSpace(request.Comments) ? null : request.Comments.Trim();

    return Results.Ok(book);
});

// DELETE
app.MapDelete("/api/books/{id:guid}", (InMemoryBookStore store, Guid id) =>
{
    var deleted = store.Delete(id);
    return deleted ? Results.NoContent() : Results.NotFound(new { message = "Book not found." });
});

app.Run();

// for tests (WebApplicationFactory)
public partial class Program { }
