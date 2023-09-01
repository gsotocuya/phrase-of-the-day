using System.Text.Json;
using System.Text.Json.Serialization;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace PhraseOfTheDay.Controllers;

public class Pharse
{
    public string Date { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Publish { get; set; } = string.Empty;
}

[ApiController]
[Route("api/phrase")]
public class PhraseController : ControllerBase
{
    private readonly ILogger<Pharse> _logger;

    public PhraseController(ILogger<Pharse> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<string> GetAsync()
    {
        var client = new HttpClient();
        var response = await client.GetAsync("https://proverbia.net/frase-del-dia");

        if (!response.IsSuccessStatusCode) return "Empty";

        var html = await response.Content.ReadAsStringAsync();
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var date = document.DocumentNode.SelectSingleNode("(//div[contains(@class,'col-md-8 px-md-4')]//h2)[1]")
            .InnerText.Trim();
        var content = document.DocumentNode.SelectSingleNode("(//blockquote[contains(@class,'bsquote')]//p)[1]")
            .InnerText.Trim();
        var author = document.DocumentNode.SelectSingleNode("(//blockquote[contains(@class,'bsquote')]//a)[1]")
            .InnerText.Trim();
        var publish = document.DocumentNode.SelectSingleNode("(//blockquote[contains(@class,'bsquote')]//em)[1]")
            .InnerText.Trim();
        var result = new Pharse
        {
            Date = date,
            Content = content,
            Author = author,
            Publish = publish
        };

        return JsonConvert.SerializeObject(result);
    }
}