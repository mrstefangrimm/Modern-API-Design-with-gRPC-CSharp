using Google.Protobuf;
using System;
using System.Linq;

var msg = new Book.BookInfo { Isbn = "1234", Publisher = "Ava Orange" };

var bytes = msg.ToByteArray();

Console.WriteLine(string.Join(' ', bytes.Select(x => $"{x:X2}")));
