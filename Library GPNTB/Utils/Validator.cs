using System;
using System.Collections.Generic;
using Library_GPNTB.Models;

namespace Library_GPNTB.Utils
{
    public static class Validator
    {
        public static bool ValidateBook(Book book, out List<string> errors)
        {
            errors = new List<string>();

            if (book == null)
            {
                errors.Add("Объект книги не должен быть null.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(book.Name))
                errors.Add("Название книги не может быть пустым.");

            if (string.IsNullOrWhiteSpace(book.Genre))
                errors.Add("Жанр книги не может быть пустым.");

            if (string.IsNullOrWhiteSpace(book.Description))
                errors.Add("Описание книги не может быть пустым.");

            // Если указаны даты, проверяем корректность
            if (book.DateIssued.HasValue && book.DateReturn.HasValue &&
                book.DateReturn < book.DateIssued)
            {
                errors.Add("Дата возврата не может быть раньше даты выдачи.");
            }

            return errors.Count == 0;
        }

        public static bool ValidateUser(User user, out List<string> errors)
        {
            errors = new List<string>();

            if (user == null)
            {
                errors.Add("Объект пользователя не должен быть null.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.Name))
                errors.Add("Имя пользователя не может быть пустым.");

            return errors.Count == 0;
        }
    }
}
