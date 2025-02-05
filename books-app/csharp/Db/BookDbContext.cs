using Microsoft.EntityFrameworkCore;
using Model;
using System;

namespace Db;

public class BookDbContext : DbContext {

  public string DbPath { get; }

  public BookDbContext()
  {
    var folder = Environment.SpecialFolder.MyDocuments;
    var path = Environment.GetFolderPath(folder);
    DbPath = System.IO.Path.Join(path, "book.db");
  }

  public DbSet<DBBook> Books { get; set; }
  public DbSet<DBReview> Reviews { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder options)
     => options.UseSqlite($"Data Source={DbPath}");
}
