using enum_example.context;
using enum_example.helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("TodoList"));
var app = builder.Build();

app.MapGet("/todoitems", async (AppDbContext db) =>
    await db.Todos.ToListAsync());

app.MapGet("/todoitems/complete", async (AppDbContext db) =>
    await db.Todos.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, AppDbContext db) =>
{
    var res = await db.Todos.FindAsync(id);

    if (res is Todo) 
    {
        var todo = new TodoItemDTO(){
            Id = res.Id,
            IsComplete = res.IsComplete,
            Name = res.Name,
            Tags = res.Tags.Select(e => EnumHelper.GetEnumDescription(e))
        };
        return Results.Ok(todo);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPost("/todoitems", async (TodoItemDTO dto, AppDbContext db) =>
{
    var todo = new Todo(){
        Id = dto.Id,
        Name = dto.Name,
        IsComplete = dto.IsComplete,
        Tags = dto.Tags.Select(t => EnumHelper.GetEnumValueFromDescription<Tags>(t)).ToList()
    };

    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", dto);
});

app.MapPut("/todoitems/{id}", async (int id, TodoItemDTO inputTodo, AppDbContext db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;
    todo.Tags = inputTodo.Tags.Select(t => EnumHelper.GetEnumValueFromDescription<Tags>(t)).ToList();

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }

    return Results.NotFound();
});

app.Run();
