using System.Collections.Generic;
using System.Numerics;

// ConversionDemos.Run();
// FixedDemos.Run();
// SumAllDemos.Run();
// EnumerationDemos.Run();

#region Fixed point - conversion demos

public static class ConversionDemos {
  public static void Run() {
    
    // 1.25
    // 0.875
    // 31.875
    // 31.9

    Console.WriteLine("WWWWWFFF");
    byte b1 = 1;
    Console.WriteLine(b1.ToString("B").PadLeft(8, '0'));

    Console.WriteLine("\nDOUBLE");
    Console.WriteLine((double)b1);
  }
}

#endregion
#region Fixed point - sample implementation

public interface IFractionalPartDefinition {
}
public sealed class Dot3 : IFractionalPartDefinition {
}
public sealed class Dot4 : IFractionalPartDefinition {
}

public readonly struct Fixed<TBase, TDot> {
  public readonly TBase Value { get; init; }
  public Fixed(double d) { 
    throw new NotImplementedException();
  }
  public double ToDouble() {
    throw new NotImplementedException();
  }
}

public static class FixedDemos {
  public static void Run() {
    Console.WriteLine("FIXED (BYTE, DOT3)");
	  Fixed<byte, Dot3> x1 = new(1.25);
    Console.WriteLine(x1.Value.ToString("B").PadLeft(8, '0'));
	  Console.WriteLine(x1.ToDouble());

  	Fixed<byte, Dot3> x2 = new(3.5);
	  Console.WriteLine(x2.Value.ToString("B").PadLeft(8, '0'));
	  Console.WriteLine(x2.ToDouble());

  	// x1 += x2;
    Console.WriteLine(x1.Value.ToString("B").PadLeft(8, '0'));
	  Console.WriteLine(x1.ToDouble());
  }
}
#endregion
#region Fixed point - sum all

public static class NumberListExtensions {
  public static int SumAll(this IEnumerable<int> items) {
    var res = 0;
    foreach(var item in items) {
      res += item;
    }
    return res;
  }
}

public static class SumAllDemos {
  public static void Run() {
    var l0 = new []{ 1, 2, 3, 4 };
    Console.WriteLine(l0.SumAll());

    // var l1 = new []{ new Fixed<byte, Dot3>(1.25), new Fixed<byte, Dot3>(2.5), new Fixed<byte, Dot3>(0.25) };
    // Console.WriteLine(l1.SumAll().ToDouble());
  }
}
#endregion
#region Collections - enumeration

public abstract class Animal {
  public abstract string MakeSound();
}
public class Cat : Animal {
  public override string MakeSound() => "Meow!";
}
public class Dog : Animal {
  public override string MakeSound() => "Woof!";
}

public static class EnumerationDemos {
  public static void PrintAll(List<Animal> items) {
  }

  public static void Run() {
    var c1 = new List<Animal>() { new Cat(), new Dog(), new Cat(), new Cat() };
    var c2 = new Animal[] { new Cat(), new Dog(), new Cat(), new Cat() };
    var c3 = c2.Take(3);
    var c4 = new List<Cat>() { new Cat(), new Cat() };
    
    PrintAll(c1);
    // PrintAll(c2); 
    // PrintAll(c3); 
    // PrintAll(c4); 

    // PrintAllReverse(c1);
  }
}

#endregion
#region Collection - dictionaries

public interface ISimpleDictionary<TKey, TValue> // : IEnumerable<KeyValuePair<TKey, TValue>>
{
  TValue this[TKey key] { get; }
  IEnumerable<TKey> Keys { get; }
  IEnumerable<TValue> Values { get; }
  bool ContainsKey(TKey key);
  bool TryGetValue(TKey key, out TValue value);
}

#endregion