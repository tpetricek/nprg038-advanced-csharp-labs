using System.Text.Json;

class Program {
  static void Main(string[] args) {
    var app = WebApplication.CreateBuilder(args).Build();
    //app.Run(ProcessHelloRequest);
    //app.Run(ProcessAddRequest);
    app.Run(ProcessJsonAddRequest);
    app.UseHttpsRedirection();
    app.Run();
  }
  #region Hello request handler

  static Task ProcessHelloRequest(HttpContext context) {
    Console.WriteLine(context.Request.Path);
    Console.WriteLine(context.Request.QueryString);
    return context.Response.WriteAsync("Hello world!");
  }

  #endregion
  #region Simple adder handler

  static Task ProcessAddRequest(HttpContext context) {
    Console.WriteLine(context.Request.Path);
    Console.WriteLine(context.Request.QueryString);

    if (context.Request.Path == "/add") {
      var qs = context.Request.QueryString.ToString().TrimStart('?');
      var sum = 0;
      foreach(var part in qs.Split('&')) {
        sum += Int32.Parse(part.Split('=')[1]);
      }
      return context.Response.WriteAsync(sum.ToString());
    }
    else {
      return context.Response.WriteAsync("Error 404");
    }
  }

  #endregion
  #region Fancy adder handler

  static Task ProcessJsonAddRequest(HttpContext context) {
    Console.WriteLine(context.Request.Path);
    Console.WriteLine(context.Request.QueryString);

    if (context.Request.Path == "/add") {
      var qs = context.Request.QueryString.ToString().TrimStart('?');
      var sum = 0;
      foreach(var part in qs.Split('&')) {
        sum += Int32.Parse(part.Split('=')[1]);
      }
      var res = new Result(sum);
      return context.Response.WriteAsync(JsonSerializer.Serialize(res));
    }
    else {
      return context.Response.WriteAsync("Error 404");
    }
  }

  #endregion
}

record class Result(int Value);