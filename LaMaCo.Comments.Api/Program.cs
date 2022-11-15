using LaMaCo.Comments.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddAzureWebAppDiagnostics();

// Add services to the container.

builder.Services.AddDbContext<CommentsContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = System.IO.Path.Join(path, "comments.db");
        options.UseSqlite($"Data Source={dbPath}");
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
    else
    {
        var cs = builder.Configuration.GetConnectionString("CommentsContext");
        options.UseSqlServer(cs);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var responseMessage = app.Configuration["Message"] ?? "";

app.MapPost("/comments", async (CommentsContext ctx, CommentDto dto) =>
{
    var comment = new Comment { AuthorName = dto.AuthorName, Text = dto.Text };
    await ctx.AddAsync(comment);
    await ctx.SaveChangesAsync();
    return responseMessage;
});

app.MapGet("/comments", async (CommentsContext ctx) =>
{
    return await ctx.Comments.ToListAsync();
});

app.Run();

record CommentDto(string AuthorName, string Text);
