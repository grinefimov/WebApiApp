using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiApp.Models;
using WebApiApp.Services;

namespace WebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetAsync()
        {
            return await _bookService.GetAsync();
        }

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public async Task<ActionResult<Book>> GetAsync(string id)
        {
            var book = await _bookService.GetAsync(id);
            if (book == null) return NotFound();
            return book;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateAsync(Book book)
        {
            await _bookService.CreateAsync(book);
            return CreatedAtRoute("GetBook", new { id = book.Id }, book);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateAsync(string id, Book bookIn)
        {
            var book = await _bookService.GetAsync(id);
            if (book == null) return NotFound();
            await _bookService.UpdateAsync(id, bookIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var book = await _bookService.GetAsync(id);
            if (book == null) return NotFound();
            await _bookService.RemoveAsync(book.Id);
            return NoContent();
        }
    }
}