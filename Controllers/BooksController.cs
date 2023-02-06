using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEB_API.Entities;

namespace WEB_API.Controllers;

[Route("[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly BookStoreContext _context;

    public BooksController(BookStoreContext context) => _context = context;

    // GET: Books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAllBooksAsync() =>
        Ok(await _context.Books.ToListAsync());

    //GET: Books/5
    [HttpGet(template: "{id}")]
    public async Task<ActionResult<Book>> GetBookByIDAsync(int id)
    {
        var book = await _context.Books.FindAsync(keyValues: id);

        return book is not null ?
            Ok(value: book) :
            NotFound(value: $"Book with ID = {id} is not FOUND!!");
    }

    //DELETE: Books/5
    [HttpDelete(template: "{id}")]
    public async Task<IActionResult> DeleteBookByIdAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book is null)
        {
            return NotFound(value: $"Book with ID = {id} is not FOUND!!");
        }

        _context.Books.Remove(entity: book);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return NoContent();
    }

    //PUT: Books/5
    [HttpPut(template: "{id}")]
    public async Task<IActionResult> PutBookAsync(int id, Book book)
    {
        if (id != book.ID)
        {
            return BadRequest();
        }

        _context.Entry(entity: book).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await BookExistsAsync(id: id))
            {
                return NotFound(value: $"Book with ID = {id} is not FOUND!!");
            }

            throw;
        }

        return NoContent();
    }

    //POST: Books
    [HttpPost]
    public async Task<ActionResult<Book>> CreateBookAsync(Book book)
    {
        var bookID = book.ID;

        if (await BookExistsAsync(id: bookID))
        {
            return Conflict(error: $"Book with ID = {bookID} is duplicate!!");
        }

        await _context.Books.AddAsync(entity: book);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return CreatedAtAction(
            actionName: nameof(GetBookByIDAsync),
            routeValues: new { id = book.ID },
            value: book);
    }

    private async Task<bool> BookExistsAsync(int id) =>
        await _context.Books.AnyAsync(predicate: book => book.ID == id);
}