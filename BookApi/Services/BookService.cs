using BookApi.Models;

namespace BookApi.Services
{
    public class BookService
    {
            private static List<Book> books = new List<Book>();
            private static int nextId = 1;

            public List<Book> GetAll()
            {
                return books;
            }

            public Book? GetById(int id)
            {
                return books.FirstOrDefault(b => b.Id == id);
            }

            public Book Add(Book book)
            {
                book.Id = nextId++;
                books.Add(book);
                return book;
            }

            public bool Update(int id, Book updatedBook)
            {
                var book = books.FirstOrDefault(b => b.Id == id);
                if (book == null) return false;

                book.Title = updatedBook.Title;
                book.Author = updatedBook.Author;
                book.ISBN = updatedBook.ISBN;
                book.PublicationDate = updatedBook.PublicationDate;

                return true;
            }

            public bool Delete(int id)
            {
                var book = books.FirstOrDefault(b => b.Id == id);
                if (book == null) return false;

                books.Remove(book);
                return true;
            }
        
    }
}
