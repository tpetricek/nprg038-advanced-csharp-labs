using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

HttpDemos.Run();
// HttpDemos.RandomWalk("/wiki/Charles_University");
// RestDemos.Run();
// DeserializationDemos.Run();
// RefitDemos.Run();

#region Web requests and crawling demo

public class HttpDemos {
  public static void Run() {
    var httpClient = new HttpClient();

    // https://en.wikipedia.org/wiki/Charles_University
    // Title regex: "<title>(.*)</title>"
    // Links regex: "/wiki/[^\":]*"
  }


  public static Random random = new Random();

  public static void RandomWalk(string page) {
  }
}

#endregion
#region REST API basic calling demo

public class RestDemos {
  public static void Run() {
    var httpClient = new HttpClient();
    
    // "https://b2c.cpost.cz/services/PostCode/getDataAsJson?" +
    // "cityOrPart=Praha&nameStreet=Malostranske namesti");
    
    // JsonValue.Parse
    // AsArray + AsObject
  }
}

#endregion
#region REST API with JSON deserialization

public record PostCodeItem
  ( string NameCity, string NameStreet, 
    string Name, string PostCode, string Number );

public class DeserializationDemos {
  public static void Run() {
    // JsonSerializer
  }
}

#endregion
#region REST API with Refit library

// public interface ICeskaPostaWebApi {
//   [Get("/services/PostCode/getDataAsJson?cityOrPart={city}&nameStreet={street}")]
//   Task<IReadOnlyList<PostCodeItem>> GetPostCodesByCityAndStreetAsync(string city, string street);
// }

public static class RefitDemos {
  public static void Run() {    
    // var address = "https://b2c.cpost.cz";
    // var postApi = RestService.For<ICeskaPostaWebApi>(address);
  }
}

#endregion