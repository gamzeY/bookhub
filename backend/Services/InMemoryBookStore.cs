using System.Collections.Concurrent;
using backend.Models;

namespace backend.Services;

public sealed class InMemoryBookStore
{
    private readonly List<Book> _books = new();

    private readonly object _lock = new();

    public int Count
    {
        get { lock (_lock) return _books.Count; }
    }

    public IReadOnlyList<Book> GetAllSnapshot()
    {
        lock (_lock) return _books.ToList();
    }

    public Book? GetById(Guid id)
    {
        lock (_lock) return _books.FirstOrDefault(b => b.Id == id);
    }

    public Book Add(Book book)
    {
        lock (_lock)
        {
            _books.Add(book);
            return book;
        }
    }

    public bool Delete(Guid id)
    {
        lock (_lock)
        {
            var idx = _books.FindIndex(b => b.Id == id);
            if (idx < 0) return false;
            _books.RemoveAt(idx);
            return true;
        }
    }
}

