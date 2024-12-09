using BestPractice.API.Models;
using BestPractice.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BestPractice.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController(IBookService bookService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ServiceResult<IEnumerable<Book>>>> Read()
    {
        var result = await bookService.Read();
        if (result.ProblemDetails != null) return StatusCode(result.Status, result.ProblemDetails);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceResult<Book>>> Read(int id)
    {
        var result = await bookService.Read(id);
        if (result.ProblemDetails != null) return StatusCode(result.Status, result.ProblemDetails);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResult<Book>>> Create(BookCreateDto bookCreateDto)
    {
        var result = await bookService.Create(bookCreateDto);
        if (result.ProblemDetails != null) return StatusCode(result.Status, result.ProblemDetails);
        if (result.Data == null) return StatusCode(500);
        return CreatedAtAction(nameof(Read), new { id = result.Data.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ServiceResult<Book>>> Update(int id, BookUpdateDto bookUpdateDto)
    {
        var result = await bookService.Update(id, bookUpdateDto);
        if (result.ProblemDetails != null) return StatusCode(result.Status, result.ProblemDetails);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ServiceResult<Book>>> Delete(int id)
    {
        var result = await bookService.Delete(id);
        if (result.ProblemDetails != null) return StatusCode(result.Status, result.ProblemDetails);
        return NoContent();
    }

    [HttpPatch("quantity/{id:int}")]
    public async Task<ActionResult<ServiceResult<Book>>> UpdateQuantity(int id, int quantity)
    {
        var result = await bookService.UpdateQuantity(id, quantity);
        if (result.ProblemDetails != null) return StatusCode(result.Status, result.ProblemDetails);
        return Ok(result);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<ServiceResult<PaginatedList<Book>>>> ReadPaged(int pageNumber = 1, int pageSize = 10)
    {
        var result = await bookService.Read(pageNumber, pageSize);
        if (result.ProblemDetails != null) return StatusCode(result.Status, result.ProblemDetails);
        return Ok(result);
    }
}