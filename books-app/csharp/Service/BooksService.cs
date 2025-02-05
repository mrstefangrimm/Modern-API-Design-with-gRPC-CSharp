using Model;
using Repo;
using System.Collections.Generic;

namespace Service;

public class BooksService
{
  private readonly BookRepository _repo;

  public BooksService(BookRepository repo)
  {
    _repo = repo;
  }

  public void AddBook(DBBook book)
  {
    _repo.AddBook(book);
  }

  public DBBook GetBook(int isbn)
  {
    var book = _repo.GetBook(isbn);
    if (book != null)
    {
      return book;
    }
    return new DBBook();
  }

  public IEnumerable<DBBook> GetAllBooks()
  {
    var books = _repo.GetAllBooks();
    if (books != null)
    {
      return books;
    }
    return new List<DBBook>();
  }

  public void RemoveBook(int isbn)
  {
    _repo.RemoveBook(isbn);
  }

  public void AddReview(DBReview review)
  {
    _repo.AddReview(review);
  }

  public IEnumerable<DBReview> GetAllReviews(int isbn)
  {
    var reviews = _repo.GetAllReviews(isbn);
    if (reviews != null)
    {
      return reviews;
    }
    return new List<DBReview>();
  }
}
