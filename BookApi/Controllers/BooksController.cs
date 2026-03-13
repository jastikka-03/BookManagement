using BookApi.Models;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _service;

        public BooksController(BookService service)
        {
            _service = service;
        }

        // ----------------------
        // GET ALL BOOKS
        // ----------------------
        [HttpGet("Books")]
        public ActionResult<List<Book>> GetAll()
        {
            return _service.GetAll();
        }

        // ----------------------
        // GET SINGLE BOOK 
        // ----------------------
        [HttpGet("Book/{id}")]
        public ActionResult<Book> Get(int id)
        {
            var book = _service.GetById(id);
            if (book == null) return NotFound();
            return book;
        }

        // ----------------------
        // POST SINGLE BOOK 
        // ----------------------
        [HttpPost("Book")]
        public ActionResult<Book> Create(Book book)
        {
            var newBook = _service.Add(book);
            return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
        }

        // ----------------------
        // POST MULTIPLE BOOKS 
        // ----------------------
        [HttpPost("Books")]
        public ActionResult<List<Book>> CreateMultiple([FromBody] List<Book> books)
        {
            var addedBooks = new List<Book>();
            foreach (var book in books)
            {
                var newBook = _service.Add(book);
                addedBooks.Add(newBook);
            }
            return Ok(addedBooks);
        }

        // ----------------------
        // PUT SINGLE BOOK
        // ----------------------
        [HttpPut("Book/{id}")]
        [ProducesResponseType(typeof(Book), 200)]
        public ActionResult<Book> Update(int id, Book book)
        {
            if (!_service.Update(id, book)) return NotFound();

            // Return the updated book
            var updatedBook = _service.GetById(id);
            return Ok(updatedBook);
        }

        // ----------------------
        // PUT MULTIPLE BOOKS
        // ----------------------
        [HttpPut("Books")]
        [ProducesResponseType(typeof(List<Book>), 200)]
        public ActionResult<List<Book>> UpdateMultiple([FromBody] List<Book> books)
        {
            var updatedBooks = new List<Book>();
            foreach (var book in books)
            {
                if (_service.Update(book.Id, book))
                {
                    updatedBooks.Add(_service.GetById(book.Id));
                }
            }
            return Ok(updatedBooks);
        }

        // ----------------------
        // DELETE SINGLE BOOK
        // ----------------------
        [HttpDelete("Book/{id}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        public ActionResult<object> Delete(int id)
        {
            var book = _service.GetById(id);
            if (book == null) return NotFound();

            _service.Delete(id);

            // Return a JSON object with a message
            return Ok(new { message = $"Book with ID {id} ('{book.Title}') was deleted successfully." });
        }

        // ----------------------
        // DELETE MULTIPLE BOOKS
        // ----------------------
        [HttpDelete("Books")]
        [ProducesResponseType(typeof(List<object>), 200)]
        [ProducesResponseType(400)]
        public ActionResult<List<object>> DeleteMultiple([FromBody] List<int> ids)
        {
            var deletedMessages = new List<object>();

            foreach (var id in ids)
            {
                var book = _service.GetById(id);
                if (book != null)
                {
                    _service.Delete(id);
                    deletedMessages.Add(new { message = $"Book with ID {id} ('{book.Title}') deleted successfully." });
                }
                else
                {
                    deletedMessages.Add(new { message = $"Book with ID {id} not found." });
                }
            }

            return Ok(deletedMessages);
        }
    }
    }