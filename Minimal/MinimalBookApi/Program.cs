using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapGet("/book", () =>
{
    return books;
});

app.MapGet("/book/{id}", (int id) =>
{
    var book = books.Find(b => b.Id == id);

    if (book is null)
        return Results.NotFound("this book cannot found ");

    return Results.Ok(book);
});

app.Run();


class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
}

