using ex1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace ex1_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            books = new ObservableCollection<Book>();
            authors = new ObservableCollection<Author>();
            InitializeComponent(); 
            lbBooks.ItemsSource = books;
        }
        private ObservableCollection<Book> books;//collections of books
        private ObservableCollection<Author> authors;//collections of authors 
        

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!AllTextBoxesFilled(inputPanel))
            {
                MessageBox.Show("You must fill all textboxes", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                (string ISBN, string name, int copies, decimal price, Author a) = GetBookData();
                Book newBook = new Book(ISBN, name, copies, price, a);
                tbTitle.Clear(); tbPrice.Clear(); tbISBN.Clear(); tbCopies.Clear(); tbAfirstName.Clear(); tbAlastName.Clear();
                if (!books.Contains(newBook))//if the book dont exists add new book
                {
                    books.Add(newBook);
                    books = new ObservableCollection<Book>(books.OrderBy(b => b.Name).ThenBy(b => b.AuthorP.FirstName));
                    lbBooks.ItemsSource = books;

                    TextRange rangeOfText2 = new TextRange(rlbUpdate.Document.ContentEnd,
                    rlbUpdate.Document.ContentEnd);
                    rangeOfText2.Text = "Added " + newBook.Name.ToString() + "\r";
                    rangeOfText2.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.LawnGreen);
                }
                
                else
                {
                    throw new Exception("The book is already in libery");
                }
                if (!authors.Contains(newBook.AuthorP))//if the author dont exists add new one
                {
                    Author newA = new Author(newBook.AuthorP.FirstName, newBook.AuthorP.LastName);
                    authors.Add(newA);
                }
                else
                {
                    var item = authors.FirstOrDefault(i => i.Equals(newBook.AuthorP));//check if exists this author in ObservableCollection(authors). 
                    if (item != null)
                    {
                        item.NumOfBook++;
                    }
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Input is not in correct format "+ ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private (string ISBN, string name, int copies, decimal price, Author a) GetBookData()
        {
            if (tbAfirstName.Text.Contains(" "))
            {
                throw new FormatException("first name without space");
            }
            Author au = new Author(tbAfirstName.Text, tbAlastName.Text);
            if (!int.TryParse(tbCopies.Text, out int c))
            {
                throw new FormatException("copies is not a number");
            }
            if (!decimal.TryParse(tbPrice.Text, out decimal p))
            {
                throw new FormatException("price is not a number");
            }
             return (tbISBN.Text, tbTitle.Text, c, p, au);
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lbBooks.SelectedItem == null)
            {
                MessageBox.Show("You must select a book to delete", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Book book2del = lbBooks.SelectedItem as Book;
            var result = MessageBox.Show("Are you sure you want to delete? "
                ,
                "Delete Book", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                var item = authors.FirstOrDefault(i => i.Equals(book2del.AuthorP));
                if (item.NumOfBook == 1)//if have only 1  of number of book by authors so is the last and can delete. 
                {
                    authors.Remove(item);
                }
                else
                {
                    item.NumOfBook--;
                }
                books.Remove(book2del);
                tbPriceUp.Clear();
                tbCopiesUp.Clear();
                tbNumBookA.Clear();
                tbAuthor.Clear();
                TextRange rangeOfText2 = new TextRange(rlbUpdate.Document.ContentEnd,
                rlbUpdate.Document.ContentEnd);
                rangeOfText2.Text = "Deleted " + book2del.Name.ToString()+"\r";
                rangeOfText2.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            }

        }

        private void buttonChangeAmount_Click(object sender, RoutedEventArgs e)
        {
            var select = lbBooks.SelectedItem as Book;
            try
            {
                if (!int.TryParse(tbCopiesUp.Text, out int c))
                {
                    throw new FormatException("copies is not a number");
                }
                string before = select.Copies.ToString();
                if (tbCopiesUp.Text==before)
                {
                    throw new Exception("change amount before click");
                }
                select.Copies = c;
                TextRange rangeOfText2 = new TextRange(rlbUpdate.Document.ContentEnd,
                rlbUpdate.Document.ContentEnd);
                rangeOfText2.Text = "Change amount of " + select.Name.ToString() +" from "+ before + " to "+c.ToString()+ "\r";
                rangeOfText2.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);

            }
            catch (FormatException ex)
            {
                if (lbBooks.SelectedItem != null) tbCopiesUp.Text = select.Copies.ToString();
                MessageBox.Show("Input is not in correct format " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                if (lbBooks.SelectedItem != null) tbCopiesUp.Text = select.Copies.ToString();
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonChangePrice_Click(object sender, RoutedEventArgs e)
        {
            var select = lbBooks.SelectedItem as Book;
            try
            {
                if(!decimal.TryParse(tbPriceUp.Text, out decimal p))
            {
                    throw new FormatException("price is not a number");
                }
                if(p>select.Price)
                {
                    throw new Exception("Illegal to raise price");
                    
                }
                if (tbPriceUp.Text == select.Price.ToString())
                {
                    throw new Exception("change price before click");
                }
                select.Price = p;
                TextRange rangeOfText2 = new TextRange(rlbUpdate.Document.ContentEnd,
                rlbUpdate.Document.ContentEnd);
                rangeOfText2.Text = "Change price of " + select.Name.ToString() + " to " +  p.ToString()+"$" + "\r";
                rangeOfText2.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
                     
            }
            catch (FormatException ex)
            {
                if (lbBooks.SelectedItem != null) tbPriceUp.Text = select.Price.ToString();
                MessageBox.Show("Input is not in correct format " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                if (lbBooks.SelectedItem != null) tbPriceUp.Text = select.Price.ToString();
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private bool AllTextBoxesFilled(Panel panel)
        {
            foreach (var item in panel.Children)
            {
                if (item is TextBox)
                {
                    TextBox tb = item as TextBox;
                    if (string.IsNullOrEmpty(tb.Text))
                    {
                        return false;
                    }
                }
                else
                {
                    if (item is Panel)
                    {
                        if (!AllTextBoxesFilled(item as Panel))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void LbBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbBooks.SelectedItem != null)
            {
                var select = lbBooks.SelectedItem as Book;
                tbAuthor.Text=select.AuthorP.ToString();
                tbCopiesUp.Text = select.Copies.ToString();
                tbPriceUp.Text = select.Price.ToString();
                var item = authors.FirstOrDefault(i => i.Equals(select.AuthorP));
                if(item!=null)
                    tbNumBookA.Text = item.NumOfBook.ToString();

            }
        }

        string filename = "library.xml";

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (books.Count > 0)
            {
                using (StreamWriter handle = new StreamWriter("filepath"))
                {
                    handle.Write(filename);
                }
                SaveStudentsToFile();
            }
            else
            {
                File.Delete("filepath");
            }
        }

        private void SaveStudentsToFile()
        {
            XmlTextWriter writer = new XmlTextWriter(filename, Encoding.Unicode);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("Lib");
            foreach (var book in books)
            {
                writer.WriteStartElement("Book");
                writer.WriteElementString("ISBN", book.ISBN);
                writer.WriteElementString("Title", book.Name);
                writer.WriteElementString("Author", book.AuthorP.ToString());
                writer.WriteElementString("Copies", book.Copies.ToString());
                writer.WriteElementString("Price", book.Price.ToString());
                writer.WriteEndElement();
            }
            
            foreach (var author in authors)
            {
                writer.WriteStartElement("Author");
                writer.WriteElementString("FirstName", author.FirstName);
                writer.WriteElementString("LastName", author.LastName);
                writer.WriteElementString("NumOfBook", author.NumOfBook.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndDocument();
            writer.Close();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            if (File.Exists("filepath"))
            {
                using (StreamReader handle = new StreamReader("filepath"))
                {
                    filename = handle.ReadToEnd();
                }
                if (File.Exists(filename))
                {
                    ReadStudentsFromXml();
                }
            }
        }

        private void ReadStudentsFromXml()
        {
            XmlTextReader reader = new XmlTextReader(filename);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            while (reader.Name != "Book" && reader.Name != "Author")
            {
                reader.Read();
            }
            while (reader.Name == "Book"|| reader.Name == "Author")
            {
                if (reader.Name == "Book")
                {
                    Book st;
                    reader.ReadStartElement("Book");
                    string isbn = reader.ReadElementString("ISBN");
                    string name = reader.ReadElementString("Title");
                    string authorXmlS = reader.ReadElementString("Author");
                    int copies = int.Parse(reader.ReadElementString("Copies"));
                    decimal price = decimal.Parse(reader.ReadElementString("Price"));
                    
                    string[] words = authorXmlS.Split(' ');
                    int i = authorXmlS.IndexOf(" ") + 1;
                    string restOfTheWord = authorXmlS.Substring(i);
                    Author a = new Author(words[0], restOfTheWord);
                    st = new Book(isbn, name, copies, price, a);
                    books.Add(st);
                }
                if (reader.Name == "Author")
                {
                    Author st;
                    reader.ReadStartElement("Author");
                    string firstName = reader.ReadElementString("FirstName");
                    string lastName = reader.ReadElementString("LastName");
                    int numOfBook = int.Parse(reader.ReadElementString("NumOfBook")); 
                    st = new Author(firstName, lastName);
                    st.NumOfBook = numOfBook;
                    authors.Add(st);
                }
                    
                reader.ReadEndElement();
            }
            reader.Close();
        }
    }
}
