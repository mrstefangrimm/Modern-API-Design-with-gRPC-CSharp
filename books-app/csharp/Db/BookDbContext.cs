﻿using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.IO;

namespace Db;

public class BookDbContext : DbContext
{

    public string DbPath { get; }

    public BookDbContext()
    {
        var folder = Environment.SpecialFolder.MyDocuments;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "book.db");

        if (!File.Exists(DbPath))
        {
           throw new InvalidOperationException($"{DbPath} is missing. Copy book.db to the Documents folder.");
        }
    }

    public DbSet<DBBook> Books { get; set; }
    public DbSet<DBReview> Reviews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
       => options.UseSqlite($"Data Source={DbPath}");
}
