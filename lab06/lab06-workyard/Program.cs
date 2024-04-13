// EnumeratorDemos.Run();
// StringDemos.Run();
// ValidationDemos.Run();

#region Enumerables - infinite random walk 

public static class EnumeratorDemos {
  static Random random = new Random();
  static public IEnumerable<(int, int)> Walk(int x, int y) {
    yield return (x, y);
    // TODO: Move by random offset
    // TODO: Continue looping
  }

  public static void Run() {
    var x0 = Console.WindowWidth / 2;
    var y0 = Console.WindowHeight / 2;
    foreach(var (x, y) in Walk(x0, y0)) {
      Console.SetCursorPosition(x, y);
      Console.Write("X");
      System.Threading.Thread.Sleep(100);
      Console.SetCursorPosition(x, y);
      Console.Write(" ");
    }
  }
}

#endregion
#region Strings - how to check strings

public static class StringDemos {
  public static bool MyNullOrWhite(string s) {
    if (s == null) return true;
    // TODO: Check for non-space characters in 's'.
    return true;
  }
  public static void Run() {
    Console.WriteLine(MyNullOrWhite(""));
    Console.WriteLine(String.IsNullOrWhiteSpace(""));
    Console.WriteLine("	".EndsWith(" "));    

    // var words = "chrobak,hovnival,beruska".Split(',').Order();
    // foreach(var w in words) Console.WriteLine(w);
  }
}

#endregion
#region Validator - step 1 - basic validators
namespace Step1 { 

record class ValidationError {
  public required string Message { get; init; }
}

record class Order {
  public int Id { get; set; }
  public decimal Amount { get; set; }
}

record class PremiumOrder : Order {
  public int Delivery { get; set; }
}

abstract class Validator<T> {
  public abstract IEnumerable<ValidationError> Validate(T value);
}

class OrderValidator : Validator<Order> {
  public override IEnumerable<ValidationError> Validate(Order value) {
    List<ValidationError> errors = new List<ValidationError>();
    // errors.AddRange(Validate(value.Amount, new RangeValidator<decimal>(0, 100)));
    return errors;
  }
}

public static class ValidationDemos {
  public static void Run() {
    var order = new Order { Id = 42, Amount = 500 };
    ValidateAll(new[] { order }, new OrderValidator());

    // var premium = new PremiumOrder { Id = 50, Amount = 10, Delivery = 15 };
    // ValidateAllPremium(new List<PremiumOrder>{ premium }, new OrderValidator());
  }

	static void ValidateAllPremium(IEnumerable<PremiumOrder> orders, Validator<PremiumOrder> validator) {
		foreach (var o in orders) {
			foreach(var e in validator.Validate(o))
        Console.WriteLine(e.Message);
		}
	}
	static void ValidateAll(IEnumerable<Order> orders, Validator<Order> validator) {
		foreach (var o in orders) {
			foreach(var e in validator.Validate(o))
        Console.WriteLine(e.Message);
		}
	}
}
}
#endregion
#region Validator - step 2 - advanced syntax
namespace Step2 { 
record class ValidationError {
  public required string Message { get; init; }
}

record class Order {
  public int Id { get; set; }
  public decimal Amount { get; set; }
}

record class PremiumOrder : Order {
  public int Delivery { get; set; }
}

interface IValidator<in T> {
  public IEnumerable<ValidationError> Validate(T value);
}

abstract class Validator<T> : IValidator<T> {
  public abstract IEnumerable<ValidationError> Validate(T value);

  public IEnumerable<ValidationError> Validate<TValid>(TValid value, params Validator<TValid>[] validators) {
    List<ValidationError> errors = new List<ValidationError>();
    foreach(var validator in validators) errors.AddRange(validator.Validate(value));
    return errors;
  }
}

class RangeValidator<T> : Validator<IComparable<T>> {
  private T lo, hi;
  public RangeValidator(T lo, T hi) {
    this.lo = lo; this.hi = hi;
  }
  public override IEnumerable<ValidationError> Validate(IComparable<T> value) {
    if (value.CompareTo(lo) < 0) yield return new ValidationError { Message = "Value too small" };
    if (value.CompareTo(hi) > 0) yield return new ValidationError { Message = "Value too big" };
  }
}

class OrderValidator : Validator<Order> {
  public override IEnumerable<ValidationError> Validate(Order value) {
    List<ValidationError> errors = new List<ValidationError>();
    //errors.AddRange(Validate(value.Amount, new RangeValidator<decimal>(0, 100)));
    return errors;
  }
}
class PremiumOrderValidator : Validator<PremiumOrder> {
  public override IEnumerable<ValidationError> Validate(PremiumOrder value) {
    // return Collect(
    //   Validate(value.Id, RangeValidator(0, 100)),
    //   Validate(value.Amount, RangeValidator(0M, 7M))
    // );
    throw new NotImplementedException();
  }
}

public static class ValidationDemos {
  public static void Run() {
    var order = new Order { Id = 42, Amount = 500 };
    ValidateAll(new[] { order }, new OrderValidator());

    var premium = new PremiumOrder { Id = 50, Amount = 10, Delivery = 15 };
    ValidateAllPremium(new List<PremiumOrder>{ premium }, new OrderValidator());
  }

	static void ValidateAllPremium(IEnumerable<PremiumOrder> orders, IValidator<PremiumOrder> validator) {
		foreach (var o in orders) {
			foreach(var e in validator.Validate(o))
        Console.WriteLine(e.Message);
		}
	}
	static void ValidateAll<T>(IEnumerable<T> orders, Validator<T> validator) {
		foreach (var o in orders) {
			foreach(var e in validator.Validate(o))
        Console.WriteLine(e.Message);
		}
	}
}
}
#endregion