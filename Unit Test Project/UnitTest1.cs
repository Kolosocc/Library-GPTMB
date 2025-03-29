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
            Assert.IsNotNull(book, "����� �� �������");
            Assert.AreEqual(BookStatus.Available, book.Status, "����� �� �������� ��� ������");

            User user = context.Users.Find(u => u.UserId == 2);
            Assert.IsNotNull(user, "������������ �� ������");

            book.Reader = user;
            book.Status = BookStatus.Issued;
            book.DateIssued = DateTime.Now;
            book.DateReturn = DateTime.Now.AddDays(14);

            Assert.AreEqual(BookStatus.Issued, book.Status, "������ ����� �� ��������� �� 'Issued'");
            Assert.AreEqual(user, book.Reader, "������������ �� ��������");
            Assert.IsNotNull(book.DateIssued, "���� ������ �� �����������");
            Assert.IsNotNull(book.DateReturn, "���� �������� �� �����������");
        }

        [TestMethod]
        public void ReturnBook_WhenBookIssued_ShouldReturnBook()
        {
            Book book = context.Books.Find(b => b.Id == 1);
            Assert.IsNotNull(book, "����� �� �������");
            Assert.AreEqual(BookStatus.Issued, book.Status, "����� �� ������");

            book.Reader = null;
            book.Status = BookStatus.Available;
            book.DateIssued = null;
            book.DateReturn = null;

            Assert.AreEqual(BookStatus.Available, book.Status, "������ ����� �� ��������� �� 'Available'");
            Assert.IsNull(book.Reader, "������������ �� ��� �����");
            Assert.IsNull(book.DateIssued, "���� ������ �� ��������");
            Assert.IsNull(book.DateReturn, "���� �������� �� ��������");
        }
    }
}


