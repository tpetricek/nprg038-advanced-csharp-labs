#nullable disable
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Immutable;

#region Record types for JSON parsing

public record CurrencyExchangeRatesResult {
	public CurrencyExchangeRates Data { get; set; }
}

public record CurrencyExchangeRates {
	[JsonPropertyName("currency")]
	public string CurrencyId { get; set; }
	public IReadOnlyDictionary<string, decimal> Rates { get; set; }
}

public class Exchange {
	public string Id;
	public string Url;
}

#endregion

public class Program {
	private static ImmutableList<Exchange> listReal = ImmutableList.Create(
		new Exchange { Id = "USD", Url = "https://api.coinbase.com/v2/exchange-rates?currency=USD" },
		new Exchange { Id = "EUR", Url = "https://api.coinbase.com/v2/exchange-rates?currency=EUR" },
		new Exchange { Id = "GBP", Url = "https://api.coinbase.com/v2/exchange-rates?currency=GBP" }
	);
	private static object lockObj = new object();

	private static decimal ParsePrice(string json) {
		var opts = new JsonSerializerOptions(JsonSerializerDefaults.Web);
		var result = JsonSerializer.Deserialize<CurrencyExchangeRatesResult>(json, opts);
		return result.Data.Rates["CZK"];
	}

	private static HttpClient httpClient = new HttpClient();

	private static decimal GetAvgRate(ImmutableList<Exchange> list) {
		decimal sum = 0;
		foreach (var ex in list) {
			string json = httpClient.GetStringAsync(ex.Url).Result;
			sum += ParsePrice(json);
		}
		var avg = sum / list.Count;
		return avg;
	}

	public record AvgRateResult(decimal AvgRate);

	public record RatePost(string Id, string Url);

	public static void Main(string[] args) {
		#region Initialize Kestrel
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment()) {
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();
		#endregion

		app.MapGet("/rates/average", () => {
			var avgRate = GetAvgRate(listReal);
			return new AvgRateResult(avgRate);
		})
		.WithOpenApi();

		app.MapPost("/rates", (RatePost ratePost) => {
			lock(lockObj) { 
				listReal = listReal.Add(new Exchange { Id = ratePost.Id, Url = ratePost.Url });
			}
		})
		.WithOpenApi();

		app.MapDelete("/rates/{id}", (string id) => {
			lock(lockObj) { 
				listReal = listReal.RemoveAll(x => x.Id == id);
			}
		})
		.WithOpenApi();

		app.Run();
	}
}