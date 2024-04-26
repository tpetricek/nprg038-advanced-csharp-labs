using Refit;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

// HttpDemos.Run();
// HttpDemos.RandomWalk("/wiki/Charles_University");
// RestDemos.Run();
// DeserializationDemos.Run();
// RefitDemos.Run();
ReflectionDemos.Run();

#region Web requests and crawling demo

public class HttpDemos {
  public static void Run() {
    var httpClient = new HttpClient();
    var html = httpClient.GetStringAsync("https://en.wikipedia.org/wiki/Charles_University").Result;
    
    var regexTitle = new Regex("<title>(.*)</title>");
    Console.WriteLine(regexTitle.Match(html).Groups[1].Value);

    var regexLinks = new Regex("/wiki/[^\":]*");
    var links = regexLinks.Matches(html);
    for(var i = 0; i < links.Count; i++)
      Console.WriteLine(links[i].Value);
  }

  public static Random random = new Random();

  public static void RandomWalk(string page) {
    Console.WriteLine(page);

    var httpClient = new HttpClient();
    var html = httpClient.GetStringAsync("https://en.wikipedia.org" + page).Result;
    
    var regexTitle = new Regex("<title>(.*)</title>");
    Console.WriteLine(regexTitle.Match(html).Groups[1].Value);
    Console.WriteLine();

    var regexLinks = new Regex("/wiki/[^\":]*");
    var links = regexLinks.Matches(html);
    RandomWalk(links[random.Next(links.Count)].Value);
  }
}

#endregion
#region REST API basic calling demo

public class RestDemos {
  public static void Run() {
    var httpClient = new HttpClient();
    var task = httpClient.GetStringAsync(
       "https://b2c.cpost.cz/services/PostCode/getDataAsJson?" +
       "cityOrPart=Praha&nameStreet=Malostranske namesti");
    
    var json = task.Result;
    
    // Console.WriteLine(json);
    // Console.WriteLine(JsonValue.Parse(json));
    
    var parsed = JsonValue.Parse(json);
    foreach(var no in parsed!.AsArray()) {
      Console.WriteLine(no!.AsObject()["number"]);
    }
  }
}

#endregion
#region REST API with JSON deserialization

public record PostCodeItem(string NameCity, string NameStreet, string Name, string PostCode, string Number);

public class DeserializationDemos {
  public static void Run() {
    var httpClient = new HttpClient();
    var task = httpClient.GetStringAsync(
       "https://b2c.cpost.cz/services/PostCode/getDataAsJson?" +
       "cityOrPart=Praha&nameStreet=Malostranske namesti");
    var json = task.Result;
    
    // var items = JsonSerializer.Deserialize<IReadOnlyList<PostCodeItem>>(json);
    
    var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    var items = JsonSerializer.Deserialize<IReadOnlyList<PostCodeItem>>(json, options);
    foreach(var item in items!) {
      Console.WriteLine(item.Number);
    }
  }
}

#endregion
#region REST API with Refit library

public interface ICeskaPostaWebApi {
  [Get("/services/PostCode/getDataAsJson?cityOrPart={city}&nameStreet={street}")]
  Task<IReadOnlyList<PostCodeItem>> GetPostCodesByCityAndStreetAsync(string city, string street);
}

public static class RefitDemos {
  public static void Run() {    
    var address = "https://b2c.cpost.cz";
    var postApi = RestService.For<ICeskaPostaWebApi>(address);
    var items = postApi.GetPostCodesByCityAndStreetAsync("Praha", "Malostranske namesti").Result;
    foreach(var item in items!) {
      Console.WriteLine(item.Number);
    }
  }
}

#endregion
#region Creating object instances via Reflection

public static class ReflectionDemos {
  public static void Run() {
    var rndType = typeof(System.Random);
    object rndObj = Activator.CreateInstance(rndType)!;
    Console.WriteLine(((System.Random)rndObj).Next());
  }
}
#endregion