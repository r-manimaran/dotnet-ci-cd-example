
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Define Simple in-memory data Store
var items = new List<string> { "Item1", "Item2", "Item3" };

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


//Define the endpoints
app.MapGet("/",() => "Welcome to Github Actions CI-CD Deployment");

app.MapGet("/items", () => items)
    .WithName("GetItems")
    .WithOpenApi();

app.MapGet("/items/{id:int}", (int id) =>
{
     if(id<0 || id >= items.Count)
     {
        return Results.NotFound("Item not found");
     }  
     return Results.Ok(items[id]);
})
.WithName("GetItemById")
.WithOpenApi();

app.MapPost("/items",(string newItem)=> {
    items.Add(newItem);
    return Results.Created($"/items/{items.Count - 1}", newItem);
});

app.MapPut("/items/{id:int}", (int id, string updatedItem) =>
{
    if (id < 0 || id >= items.Count)
    {
        return Results.NotFound("Item not found");
    }
    items[id] = updatedItem;
    return Results.Ok();
});

app.MapDelete("/items/{id:int}", (int id) =>
{
    if (id < 0 || id >= items.Count)
    {
        return Results.NotFound("Item not found");
    }
    items.RemoveAt(id);
    return Results.NoContent();
});


app.Run();

