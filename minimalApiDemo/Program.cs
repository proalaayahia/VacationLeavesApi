using Microsoft.EntityFrameworkCore;
using minimalApiDemo.Data;
using minimalApiDemo.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("npgsql")));
builder.Services.GlobalExceptionsCustomMiddleware();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
migrateDB(app);
app.AddGlobalExceptionMiddleware();

//app.UseHttpsRedirection();
var todos = app.MapGroup("/api/todos");
todos.MapGet("/", async (AppDbContext _context) =>
{
    var items = await _context.ToDos.ToListAsync();
    if (items.Any()) return Results.Ok(items);
    return Results.Empty;
});
todos.MapPost("/", async (AppDbContext _context, ToDo todo) =>
{
    await _context.ToDos.AddAsync(todo);
    await _context.SaveChangesAsync();
    return Results.Created($"api/todos/{todo.Id}", todo);
});
todos.MapPut("/{id}", async (AppDbContext _context, int id, ToDo todo) =>
{
    var item = await _context.ToDos.FirstOrDefaultAsync(t => t.Id == id);
    if (item == null) return Results.NotFound();
    item.TodoName = todo.TodoName;
    await _context.SaveChangesAsync();
    return Results.NoContent();
});
todos.MapDelete("/{id}", async (AppDbContext _context, int id) =>
{
    var item = await _context.ToDos.FirstOrDefaultAsync(t => t.Id == id);
    if (item == null) return Results.NotFound();
    _context.ToDos.Remove(item);
    await _context.SaveChangesAsync();
    return Results.Ok();
});

app.Run();
 void migrateDB(WebApplication app)
{
    var builder = app.Services;
    using (var scope = builder.CreateScope())
    {

        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "an error occured while migrating or seeding database.");
        }
    }
}
