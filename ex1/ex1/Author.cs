using System;
using System.Collections.Generic;
using System.Text;

namespace ex1
{
    /// <summary>
    /// A class representing a author.
    /// </summary>
    public class Author : IComparable
    {
        private int numOfBook;
        /// <summary>
        /// first name of Author.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Last Name of Author.
        /// </summary>
        public string LastName { get; set; }
        #region properties
        /// <summary>
        /// Number Of Books that written by the Author.
        /// </summary>
        public int NumOfBook
        {
            get
            {
                return numOfBook;
            }
            set
            {
                if (value < 1)
                    throw new Exception("Illegal num of book");
                numOfBook = value;
            }
        }
        #endregion
        #region constructor
        /// <summary>
        /// constructor with two parameters.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public Author(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            NumOfBook = 1;
        }
        #endregion
        internal Author Clone()
        {
            return new Author(this.FirstName, this.LastName);
        }
        /// <summary>
        /// compare two Authors by first name and last name.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (!(obj is Author other))
            {
                throw new ArgumentException("argument to CompareTo is not type Author");
            }
            return this.FirstName.CompareTo(other.FirstName);
        }
        /// <summary>
        /// print first name and last name of author.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
        /// <summary>
        /// return true or false if two authors are equals.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Author other = obj as Author;
            if (obj == null)
                return false;
            return this.FirstName == other.FirstName && this.LastName == other.LastName;
        }
    }
}
