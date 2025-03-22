using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Library_GPNTB.Context;
using Library_GPNTB.Models;



namespace Library_GPNTB
{
    public partial class MainWindow : Window
    {
        LibraryContext context = new LibraryContext();
        Book selectedBook;

        public MainWindow()
        {
            InitializeComponent();
            ListViewBooks.ItemsSource = context.Books.ToList();
            ListViewUsers.ItemsSource = context.Users.ToList();
        }

        private void User_add_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = ListViewUsers.SelectedItem as User;

            if (selectedBook == null || selectedUser == null) return;

            selectedBook.Reader = selectedUser;
            selectedBook.Status = BookStatus.Issued;
            selectedBook.DateIssued = DateTime.Now;
            selectedBook.DateReturn = DateTime.Now.AddDays(14);

            RefreshBookList();
            MessageBox.Show($"Книга {selectedBook.Name} выдана {selectedUser.Name}");
        }

        private void User_dell_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBook == null) return;

            selectedBook.Reader = null;
            selectedBook.Status = BookStatus.Available;
            selectedBook.DateIssued = null;
            selectedBook.DateReturn = null;

            RefreshBookList();
            MessageBox.Show($"Книга {selectedBook.Name} возвращена в библиотеку.");
        }
        private void Remove_Maintenance_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBook == null) return;
            if (selectedBook.Status != BookStatus.Maintenance)
            {
                MessageBox.Show($"У книги должен быть статус обслуживания");
                return;
            }
            selectedBook.Name = EditNameTextBox.Text;
            selectedBook.Genre = EditGenreTextBox.Text;
            selectedBook.Description = EditDescriptionTextBox.Text;
            selectedBook.Status = BookStatus.Available;
            selectedBook.DateIssued = null;
            selectedBook.DateReturn = null;

            RefreshBookList();
            MessageBox.Show($"Книга {selectedBook.Name} возвращена в библиотеку.");
        }


        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBook == null) return;

            context.Books.Remove(selectedBook);
            RefreshBookList();
            MessageBox.Show($"Книга '{selectedBook.Name}' удалена.");
        }

        private void ChangeToMaintenance_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBook == null) return;

            selectedBook.Status = BookStatus.Available;
            RefreshBookList();
            MessageBox.Show($"Жанр книги '{selectedBook.Name}' изменен на 'Обслуживание'.");
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

            ListViewBooks.ItemsSource = context.Books.OrderBy(book => prop.GetValue(book)).ToList();
        }



        private void Filter_Book_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilter();
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilter();

        private void ApplyFilter()
        {
            var selectedItem = Filter_Book_ComboBox.SelectedItem as ComboBoxItem;
            var filterText = Text_find.Text.ToLower();
            var propertyName = selectedItem.Content.ToString();
            var prop = typeof(Book).GetProperty(propertyName);

            if (prop == null) return;

            var filteredBooks = context.Books
                .Where(book => prop.GetValue(book).ToString().ToLower().Contains(filterText.ToLower())).ToList();

            ListViewBooks.ItemsSource = filteredBooks;
        }
    }
}