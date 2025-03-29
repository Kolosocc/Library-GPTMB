using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Library_GPNTB.Context;
using Library_GPNTB.Models;
using Library_GPNTB.Utils;

namespace Library_GPNTB
{
    public partial class MainWindow : Window
    {
        LibraryContext context = new LibraryContext();
        Book selectedBook;
        List<string> validationErrors;

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            ListViewBooks.ItemsSource = context.Books.ToList();
            ListViewUsers.ItemsSource = context.Users.ToList();
        }

        private void User_add_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = ListViewUsers.SelectedItem as User;
            if (selectedBook == null)
            {
                MessageBox.Show("Выберите книгу для выдачи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (selectedUser == null)
            {
                MessageBox.Show("Выберите пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!Validator.ValidateUser(selectedUser, out var userErrors))
            {
                MessageBox.Show(string.Join(Environment.NewLine, userErrors), "Ошибка валидации пользователя", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (selectedBook.Status != BookStatus.Available)
            {
                MessageBox.Show("Данная книга не доступна для выдачи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            selectedBook.Reader = selectedUser;
            selectedBook.Status = BookStatus.Issued;
            selectedBook.DateIssued = DateTime.Now;
            selectedBook.DateReturn = DateTime.Now.AddDays(14);
            if (!Validator.ValidateBook(selectedBook, out validationErrors))
            {
                MessageBox.Show(string.Join(Environment.NewLine, validationErrors), "Ошибка валидации книги", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            RefreshBookList();
            MessageBox.Show($"Книга \"{selectedBook.Name}\" выдана пользователю \"{selectedUser.Name}\".", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void User_dell_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBook == null)
            {
                MessageBox.Show("Выберите книгу для возврата.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (selectedBook.Status != BookStatus.Issued)
            {
                MessageBox.Show("Эта книга не выдана пользователю.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show($"Книга \"{selectedBook.Name}\" возвращена в библиотеку.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            selectedBook.Reader = null;
            selectedBook.Status = BookStatus.Available;
            selectedBook.DateIssued = null;
            selectedBook.DateReturn = null;
            if (!Validator.ValidateBook(selectedBook, out validationErrors))
            {
                MessageBox.Show(string.Join(Environment.NewLine, validationErrors), "Ошибка валидации книги", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            RefreshBookList();
        }

        private void Remove_Maintenance_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBook == null)
            {
                MessageBox.Show("Выберите книгу для завершения обслуживания.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (selectedBook.Status != BookStatus.Maintenance)
            {
                MessageBox.Show("Статус книги должен быть 'Обслуживание'.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            selectedBook.Name = EditNameTextBox.Text;
            selectedBook.Genre = EditGenreTextBox.Text;
            selectedBook.Description = EditDescriptionTextBox.Text;
            if (!Validator.ValidateBook(selectedBook, out validationErrors))
            {
                MessageBox.Show(string.Join(Environment.NewLine, validationErrors), "Ошибка валидации книги", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            selectedBook.Status = BookStatus.Available;
            selectedBook.DateIssued = null;
            selectedBook.DateReturn = null;
            RefreshBookList();
            MessageBox.Show($"Книга \"{selectedBook.Name}\" возвращена в библиотеку.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBook == null)
            {
                MessageBox.Show("Выберите книгу для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show($"Книга \"{selectedBook.Name}\" удалена.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            context.Books.Remove(selectedBook);
            RefreshBookList();
        }

        private void ChangeToMaintenance_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBook == null)
            {
                MessageBox.Show("Выберите книгу для перевода в обслуживание.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (selectedBook.Status != BookStatus.Available)
            {
                MessageBox.Show("Данная книга не доступна для перевода в обслуживание.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!Validator.ValidateBook(selectedBook, out validationErrors))
            {
                MessageBox.Show(string.Join(Environment.NewLine, validationErrors), "Ошибка валидации книги", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            selectedBook.Status = BookStatus.Maintenance;
            RefreshBookList();
            MessageBox.Show($"Книга \"{selectedBook.Name}\" переведена в режим обслуживания.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ListViewBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBook = ListViewBooks.SelectedItem as Book;
            if (selectedBook != null)
            {
                EditNameTextBox.Text = selectedBook.Name;
                EditGenreTextBox.Text = selectedBook.Genre;
                EditDescriptionTextBox.Text = selectedBook.Description;
            }
        }

        private void RefreshBookList() => ListViewBooks.ItemsSource = context.Books.ToList();

        private void Sort_Book_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = Sort_Book_ComboBox.SelectedItem as ComboBoxItem;
            var propertyName = selectedItem?.Content.ToString();
            var prop = typeof(Book).GetProperty(propertyName);
            if (prop == null) return;
            Text_find.Text = "";
            ListViewBooks.ItemsSource = context.Books.OrderBy(book => prop.GetValue(book)).ToList();
        }

        private void Filter_Book_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilter();
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilter();

        private void ApplyFilter()
        {
            var selectedItem = Filter_Book_ComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem == null) return;
            var filterText = Text_find.Text.ToLower();
            var propertyName = selectedItem.Content.ToString();
            var prop = typeof(Book).GetProperty(propertyName);
            if (prop == null) return;
            var filteredBooks = context.Books
                .Where(book => prop.GetValue(book)?.ToString()?.ToLower().Contains(filterText) == true)
                .ToList();
            ListViewBooks.ItemsSource = filteredBooks;
        }
    }
}
