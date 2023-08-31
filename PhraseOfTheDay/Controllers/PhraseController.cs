using System.Text.Json;
using System.Text.Json.Serialization;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace PhraseOfTheDay.Controllers;

public class Pharse
{
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; }= string.Empty;
    public string Publish { get; set; }= string.Empty;
    public string Speciality { get; set; }= string.Empty;
}

[ApiController]
[Route("[controller]")]
public class PhraseController : ControllerBase
{
    public PhraseController()
    {
        
    }

    [HttpGet]
    [AcceptVerbs("GET")]
    public IEnumerable<Pharse> Get()
    {
        var document = new HtmlDocument();
        document.LoadHtml("https://proverbia.net/frase-del-dia");

        var content = document.DocumentNode.SelectSingleNode("//div[@class='col-md-8 px-md-4']");

        var pharses = new List<Pharse>();
        foreach (var item in content.SelectNodes("//blockquote[@class='bsquote']"))
        {
            var pharse = new Pharse();
            pharse.Description = item.SelectSingleNode("//p").InnerText;
            
            pharses.Add(pharse);
        }
        
        return pharses;
    }
    // public async Task<string> GetPhrase()
    // {
    //     
    //     var phrase = new List<string>();
    //     
    //     var httpClient = new HttpClient();
    //     var response = await httpClient.GetAsync($"https://proverbia.net/frase-del-dia");
    //     var stream = await response.Content.ReadAsStreamAsync();
    //
    //     var document = new HtmlDocument();
    //     document.Load(stream);
    //
    //     var headerNames = document.DocumentNode.SelectNodes("//blockquote[@class='bsquote']");
    //
    //     foreach (var item in headerNames)
    //     {
    //         phrase.Add(item.InnerText);
    //     }
    //
    //     string jsonString = JsonSerializer.Serialize(phrase);
    //
    //     
    //     
    //     
    //     return jsonString;
    // }
}