// Casts.Run();
// Generic.Demos();
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
    
    // QUESTION: Will this run or not?
    
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
  public static void Swap(object[] arr, int i, int j) {
    object old = arr[i];
    arr[i] = arr[j];
    arr[j] = old;
  }
}

// TODO: PrintAll definition

public static class Generic {
  public static void Demos() {
    var cats = new[] { new Cat(), new Cat() };
    var anims = new[] { new Animal(), new Animal() };
    var nums = new[] { 1, 2, 3 };
    var strs = new[] { "A", "B", "C" };

    // QUESTION: Which of these will compile? efficiently?
    // TODO: Make Swap generic

    // GenericDemos.Swap(cats, 0, 1);
    // GenericDemos.Swap(anims, 0, 1);
    // GenericDemos.Swap(strs, 0, 1);
    // GenericDemos.Swap(nums, 0, 1); 
  }

  public static void Run() {
    var msgs = new[] { "Hello world!", "Ahoj svete!" };
    GenericDemos.Swap(msgs, 0, 1);
    //msgs.PrintAll();
  }
}

#endregion

#region Overload resolution and dynamic

public static class Greeter {
  public static void Greet(object who) { 
    Console.WriteLine($"Hello {who} (object)");
  }

  // TODO: Add string and generic overloads.
}

public static class Overloading {
  public static void Run() {

    // QUESTION: Why so many overloads?

    Console.WriteLine("\n===== WriteLine =====");
    Console.WriteLine("Hello...");
    Console.WriteLine(42);
    Console.WriteLine("is the answer!");


    Console.WriteLine("\n===== Greeter =====");
    var v1 = "Tomas";
    var v2 = 42;
    var v3 = new object();

    Greeter.Greet(v1);
    Greeter.Greet(v2);
    Greeter.Greet(v3);

    // DEMO: What does this do if I use 'dynamic'?
  }
}

#endregion

#region Combining multiple interfaces

public interface IIncrement {
  public void Increment();
}

public class ColouredNumber : IIncrement {
  public int Value { get; set; }
  public string? Colour { get; set; }
  public void Increment() {
    Value++;
  }
}

public static class Numerics {
  public static void IncAndPrint(IIncrement value) {
    value.Increment();
    Console.WriteLine(value.ToString());
  }
  public static void Run() {
    var red5 = new ColouredNumber() { Value = 5, Colour = "red" };
    IncAndPrint(red5);
    IncAndPrint(red5);

    // TODO: Add INumeric with Value : int
    // TODO: Add struct 'FastColouredNumber'
    // TODO: How to change IncAndPrint type?
    // TODO: Can we figure out how to make it immutable?
    // NOTE: TypeScript demo
  }
}

#endregion