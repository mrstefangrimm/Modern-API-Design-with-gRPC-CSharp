namespace Repo;

using Db;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Collections.Generic;
using System.Linq;

public class BookRepository
{
  private readonly BookDbContext _dbConn;

  public BookRepository(BookDbContext dbConn)
  {
    _dbConn = dbConn;
  }

  public void AddBook(DBBook book)
  {
    _dbConn.Books.Add(book);
    _dbConn.SaveChanges();
  }

  public void UpdateBook(DBBook book)
  {
    _dbConn.Books.ExecuteUpdateAsync(b => b.SetProperty(n => n.Name, n =>  book.Name));
    _dbConn.Books.ExecuteUpdateAsync(b => b.SetProperty(n => n.Publisher, n => book.Publisher));
  }

  public DBBook GetBook(int isbn)
  {
    return _dbConn.Books.Where(b => b.Isbn == isbn).First();
  }

  public IEnumerable<DBBook> GetAllBooks()
  {
    return _dbConn.Books;
  }

  public void RemoveBook(int isbn)
  {
    _dbConn.Books.Remove(new DBBook { Isbn = isbn });
    _dbConn.SaveChanges();
  }

  public void AddReview(DBReview review)
  {
    _dbConn.Reviews.Add(review);
    _dbConn.SaveChanges();
  }

  public IEnumerable<DBReview> GetAllReviews(int isbn)
  {
    return _dbConn.Reviews.Where(r => r.Isbn == isbn);
  }
}
