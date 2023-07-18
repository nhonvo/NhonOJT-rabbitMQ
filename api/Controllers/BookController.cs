using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private static readonly List<Book> _books = new();

    private readonly ILogger<BookController> _logger;

    private readonly IMessageProducer _messageProducer;
    public BookController(ILogger<BookController> logger, IMessageProducer messageProducer)
    {
        _logger = logger;
        _messageProducer = messageProducer;
    }
    [HttpPost]
    public IActionResult CreateBook(Book book)
    {
        _books.Add(book);
        _messageProducer.SendingMessage<Book>(book);

        return Ok();
    }
}
