using Google.Protobuf;
using System;
using System.Linq;

var msg = new Person.Person { Name = "GreeterClient", Age = 29 };
msg.PhoneNumbers.Add("9012345678");
msg.PhoneNumbers.Add("9087654321");

var bytes = msg.ToByteArray();

Console.WriteLine(string.Join(' ', bytes.Select(x => $"{x:X2}")));
