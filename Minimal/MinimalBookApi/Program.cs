

using Microsoft.EntityFrameworkCore;
using MinimalBookApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DataContext>(options =>
         options.UseSqlServer(builder.Configuration
         .GetConnectionString("MinimalConnectionStrings")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var books = new List<Book>()
{
    new Book {Id = 1, Title = "Harry Potter", Author = "BUU"},
    new Book {Id = 2, Title = "House of the dragons", Author = "Jon"},
    new Book {Id = 3, Title = "GOT", Author = "jrr Martin"}
};

app.MapGet("/book",async (DataContext context) =>
     await context.Books.ToListAsync());


app.MapGet("/book/{id}", (int id) =>
{
    var book = books.Find(b => b.Id == id);

    if (book is null)
        return Results.NotFound("this book cannot found ");

    return Results.Ok(book);
});

app.MapPost("/book", (Book book) =>
{
    books.Add(book);
    return books;
});

app.MapPut("/book/{id}", (Book UpdatedBook,int id) =>
{
    var book = books.Find(b => b.Id == id);

    if (book is null)
        return Results.NotFound("this book cannot found ");

    book.Title = UpdatedBook.Title;
    book.Author = UpdatedBook.Author;

    return Results.Ok(book);
});


app.MapDelete("/book/{id}", (int id) =>
{
    var book = books.Find(b => b.Id == id);

    if (book is null)
        return Results.NotFound("this book cannot found ");

    books.Remove(book);

    return Results.Ok(book);
});

app.Run();


public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
}

