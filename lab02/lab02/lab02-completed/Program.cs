// Casts.Run();
// Generic.Run();
// Overloading.Run();
// Numerics.Run();

#region Conversions, upcasts and downcasts

class Animal {
  public virtual void MakeSound() { Console.WriteLine("Aaargh!"); }
}
class Cat : Animal {
  public override void MakeSound() { Console.WriteLine("Meow!"); }
}
class Flytrap {  
  public static implicit operator Animal(Flytrap f) { return new Animal(); }
}

public static class Casts {
  public static void Run() { 
    var c = new Cat();
    var a = new Animal();
    var f = new Flytrap();

    var x = (Animal)c;
    var y = (Cat)a;
    var z = (Animal)f;

    x.MakeSound();
    y.MakeSound();
    z.MakeSound();
  }
}

#endregion

#region Generic and extension methods

public static class GenericDemos {
  public static void Swap<T>(T[] arr, int i, int j) {
    T old = arr[i];
    arr[i] = arr[j];
    arr[j] = old;
  }
}
public static class ArrayExtensions {
  public static void PrintAll(this string[] msgs) {
    foreach (var msg in msgs) {
      Console.WriteLine(" * " + msg);
    }
  }
}

public static class Generic {
  public static void Demos() {
    var cats = new[] { new Cat(), new Cat() };
    var anims = new[] { new Animal(), new Animal() };
    var nums = new[] { 1, 2, 3 };
    var strs = new[] { "A", "B", "C" };

    GenericDemos.Swap(cats, 0, 1);
    GenericDemos.Swap(anims, 0, 1);
    GenericDemos.Swap(nums, 0, 1);
    GenericDemos.Swap(strs, 0, 1);
  }
  public static void Run() {
    var msgs = new[] { "Hello world!", "Ahoj svete!" };
    GenericDemos.Swap(msgs, 0, 1);
    msgs.PrintAll();
  }
}

#endregion

#region Overload resolution and dynamic

public static class Greeter {
  public static void Greet<T>(T who) { 
    Console.WriteLine($"Hello {who} (T={typeof(T).Name})");
  }
  public static void Greet(string who) { 
    Console.WriteLine($"Hello {who} (string)");
  }
  public static void Greet(object who) { 
    Console.WriteLine($"Hello {who} (object)");
  }
}

public static class Overloading {
  public static void Run() {
    Console.WriteLine("\n===== WriteLine =====");
    Console.WriteLine("Hello...");
    Console.WriteLine(42);
    Console.WriteLine("is the answer!");

    Console.WriteLine("\n===== Greet =====");
    Greeter.Greet("Tomas");
    Greeter.Greet(42);
    Greeter.Greet((object)"Tomas");

    Console.WriteLine("\n===== Dynamic =====");
    dynamic d1 = "Tomas";
    dynamic d2 = 42;
    dynamic d3 = new object();
    Greeter.Greet(d1);
    Greeter.Greet(d2);
    Greeter.Greet(d3);
  }
}

#endregion

#region Combining multiple interfaces

public interface IIncrement {
  public void Increment();
}
public interface INumerical {
  public int Value { get; }
}
public interface IIncrement<T> {
  public T Increment();
}

public class ColouredNumber : IIncrement, INumerical {
  public int Value { get; set; }
  public string? Colour { get; set; }
  public void Increment() {
    Value++;
  }
}
public struct ColouredNumberFast : IIncrement, INumerical {
  public int Value { get; set; }
  public string? Colour { get; set; }
  public void Increment() {
    Value++;
  }
}
public class ColouredNumberImmutable : 
    IIncrement<ColouredNumberImmutable>, INumerical {
  public int Value { get; set; }
  public string? Colour { get; set; }
  public ColouredNumberImmutable Increment() {
    return new ColouredNumberImmutable {  
      Value = Value + 1, Colour = Colour 
    };
  }
}

public static class Numerics {
  public static void IncAndPrint_v1(INumerical value) {
    if (value is IIncrement inc) inc.Increment();
    Console.WriteLine(value.Value);
  }
  public static void IncAndPrint<T>(T value) 
      where T:IIncrement,INumerical {
    value.Increment();
    Console.WriteLine(value.Value);
  }
  public static void IncAndPrintImmutable<T>(T value) 
      where T:IIncrement<T>,INumerical {
    var valueInc = value.Increment();
    Console.WriteLine(valueInc.Value);
  }
  public static void Run() {
    var red5 = new ColouredNumber() { Value = 5, Colour = "red" };
    IncAndPrint(red5);
    IncAndPrint(red5);

    var pink6 = new ColouredNumberFast() { Value = 6, Colour = "pink" };
    IncAndPrint(pink6);
    IncAndPrint(pink6);

    var green8 = new ColouredNumberImmutable() { Value = 8, Colour = "green" };
    IncAndPrintImmutable(green8);
    IncAndPrintImmutable(green8);
  }
}

#endregion