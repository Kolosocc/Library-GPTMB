using Library_GPNTB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_GPNTB.Context
{
    public class LibraryContext
    {
        public List<User> Users { get; } = new List<User>
        {
            new User { UserId = 1, Name = "Иван" },
            new User { UserId = 2, Name = "Гойда" },
            new User { UserId = 3, Name = "Макc" },
            new User { UserId = 4, Name = "Человек" }
        };

        public List<Book> Books { get; }

        public LibraryContext()
        {
            Books = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Name = "Война и мир",
                    Genre = "Роман",
                    Description = "Классика",
                    DateIssued = new DateTime(2025, 1, 10),
                    DateReturn = new DateTime(2025, 1, 20),
                    Status = BookStatus.Issued,
                    Reader = Users[0]
                },
                new Book
                {
                    Id = 2,
                    Name = "Преступление и наказание",
                    Genre = "Роман",
                    Description = "Кровища",
                    DateIssued = null,
                    DateReturn = null,
                    Status = BookStatus.Available,
                    Reader = null
                },
                new Book
                {
                    Id = 3,
                    Name = "3",
                    Genre = "Фан роман",
                    Description = "Мистика .",
                    DateIssued = new DateTime(2025, 2, 5),
                    DateReturn = new DateTime(2025, 2, 15),
                    Status = BookStatus.Issued,
                    Reader = Users[1]
                },
                new Book
                {
                    Id = 4,
                    Name = "4",
                    Genre = "5",
                    Description = "5",
                    DateIssued = null,
                    DateReturn = null,
                    Status = BookStatus.Maintenance,
                    Reader = null
                },
            };
        }
    }
}