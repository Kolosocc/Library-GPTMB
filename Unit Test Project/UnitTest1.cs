using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library_GPNTB.Context;
using Library_GPNTB.Models;
using System;

namespace Unit_Test_Project
{
    [TestClass]
    public class LibraryOperationsTests
    {
        private LibraryContext context;

        [TestInitialize]
        public void Setup()
        {
            context = new LibraryContext();
        }

        [TestMethod]
        public void IssueBook_WhenBookAvailable_ShouldIssueBook()
        {
            Book book = context.Books.Find(b => b.Id == 2);
            Assert.IsNotNull(book, "Книга не найдена");
            Assert.AreEqual(BookStatus.Available, book.Status, "Книга не доступна для выдачи");

            User user = context.Users.Find(u => u.UserId == 2);
            Assert.IsNotNull(user, "Пользователь не найден");

            book.Reader = user;
            book.Status = BookStatus.Issued;
            book.DateIssued = DateTime.Now;
            book.DateReturn = DateTime.Now.AddDays(14);

            Assert.AreEqual(BookStatus.Issued, book.Status, "Статус книги не изменился на 'Issued'");
            Assert.AreEqual(user, book.Reader, "Пользователь не назначен");
            Assert.IsNotNull(book.DateIssued, "Дата выдачи не установлена");
            Assert.IsNotNull(book.DateReturn, "Дата возврата не установлена");
        }

        [TestMethod]
        public void ReturnBook_WhenBookIssued_ShouldReturnBook()
        {
            Book book = context.Books.Find(b => b.Id == 1);
            Assert.IsNotNull(book, "Книга не найдена");
            Assert.AreEqual(BookStatus.Issued, book.Status, "Книга не выдана");

            book.Reader = null;
            book.Status = BookStatus.Available;
            book.DateIssued = null;
            book.DateReturn = null;

            Assert.AreEqual(BookStatus.Available, book.Status, "Статус книги не изменился на 'Available'");
            Assert.IsNull(book.Reader, "Пользователь не был удалён");
            Assert.IsNull(book.DateIssued, "Дата выдачи не сброшена");
            Assert.IsNull(book.DateReturn, "Дата возврата не сброшена");
        }
    }
}


