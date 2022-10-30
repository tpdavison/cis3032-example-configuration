using System;

namespace LaMaCo.Comments.Web.Models;

public class CommentViewModel
{
    public int Id { get; set; }
    public string? AuthorName { get; set; }
    public string? Text { get; set; }
}
