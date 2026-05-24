using Publisher.Business.Author.Implementation;
using Publisher.Business.Author.Interface;
using Publisher.Business.Book.Implementation;
using Publisher.Business.Book.Interface;
using Publisher.Business.Shared.Mapper.Implementation;
using Publisher.Business.Shared.Mapper.Interface;
using Publisher.Data;
using Publisher.Data.Repositories.Implementation;
using Publisher.Data.Repositories.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//EF Core and SQL Server configuration
builder.Services.AddDbContext<PubContext>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddTransient<IDTOMapper, DTOMapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
