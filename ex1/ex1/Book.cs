using System;

namespace ex1
{       /// <summary>
        /// A class representing a book.
        /// </summary>
     public class Book :IComparable
    {
        
        /// <summary>
        /// serial code of the book.
        /// </summary>
        public string ISBN { get; set; }
        /// <summary>
        /// name of the book.
        /// </summary>
        public string Name { get; set; }
        private int copies;
        private decimal price;
        private Author author;
#region properties
        /// <summary>
        /// the author of the book.
        /// </summary>
        public Author AuthorP
        {
            get
            {
                return author.Clone();
            }
            set
            {
                author = value.Clone();
            }
        }

        /// <summary>
        /// copies of book.
        /// </summary>
        public int Copies
        {
            get
            {
                return copies;
            }
            set
            {
                if (value < 0)
                    throw new Exception("Illegal num of copies");
                copies = value;
            }
        }
        /// <summary>
        /// price of book.
        /// </summary>
        public decimal Price
        {
            get
            {
                return price;
            }
            set
            {
                if (value < 0)
                    throw new Exception("Illegal price");
                price = value;
            }
        }
        #endregion
        #region constructor
        /// <summary>
        /// constructor with five parameters.
        /// </summary>
        /// <param name="ISBN">serial of the book</param>
        /// <param name="name">title</param>
        /// <param name="copies"></param>
        /// <param name="price"></param>
        /// <param name="a">author of the book</param>
        public Book(string ISBN, string name, int copies, decimal price, Author a)
        {
            this.ISBN = ISBN;
            Name = name;
            Copies = copies;
            Price = price;
            AuthorP = a;
        }
        #endregion
        /// <summary>
        /// return true or false if two books are equals.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Book other = obj as Book;
            if (obj == null)
                return false;
            return this.ISBN == other.ISBN;
        }
        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return int.Parse(ISBN);
        }

        /// <summary>
        ///  compare two books by title(name) of the book.
        /// </summary>
        /// <param name="obj">Other book to compare </param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if(!(obj is Book other))
            {
                throw new ArgumentException("argument to CompareTo is not type Book");
            }
            return this.Name.CompareTo(other.Name);
        }
        /// <summary>
        /// print name and serial.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + " " + ISBN;
        }

    }
}
