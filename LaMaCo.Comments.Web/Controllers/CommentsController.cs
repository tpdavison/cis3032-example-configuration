using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaMaCo.Comments.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaMaCo.Comments.Web.Controllers;

public class CommentsController : Controller
{
    private HttpClient Client { get; }

    record CommentDto(int Id, string AuthorName, string Text);

    public CommentsController(HttpClient client,
                              IConfiguration configuration)
    {
        // FIXME: don't use HttpClient directly in controllers
        // FIXME: use a Service Proxy / Remote Facade pattern -- see "Isolation"
        var baseUrl = configuration["WebServices:Comments:BaseURL"];
        client.BaseAddress = new System.Uri(baseUrl);
        client.Timeout = TimeSpan.FromSeconds(5);
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        Client = client;
    }

    public async Task<IActionResult> Index()
    {
        var response = await Client.GetAsync("/comments");
        response.EnsureSuccessStatusCode();
        var comments = await response.Content.ReadAsAsync<IEnumerable<CommentDto>>();
        var vm = comments.Select(c => new CommentViewModel
        {
            Id = c.Id,
            AuthorName = c.AuthorName,
            Text = c.Text
        });
        return View(vm);
    }
}
