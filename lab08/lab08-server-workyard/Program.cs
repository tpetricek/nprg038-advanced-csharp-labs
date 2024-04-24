/*
class Program {
  static void Main(string[] args) {
    var app = WebApplication.CreateBuilder(args).Build();
    app.Run(ProcessHelloRequest);
    app.UseHttpsRedirection();
    app.Run();
  }

  static Task ProcessHelloRequest(HttpContext context) {
    Console.WriteLine(context.Request.Path);
    Console.WriteLine(context.Request.QueryString);
    return context.Response.WriteAsync("Hello world");
  }
}
*/