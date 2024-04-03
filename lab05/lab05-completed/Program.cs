using System.Numerics;

// ConversionDemos.Run();
// FixedDemos.Run();
// SumAllDemos.Run();
// EnumerationDemos.Run();

#region Extras - Covariant return types (Animals)

// See also: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/covariant-returns
namespace Extras { 

class Animal { }
class Dog : Animal { }
class Cat : Animal { }

abstract class PetShop {
  // Pet shop can sell you animals
  // (Method to illustrate covariant return types)
  public abstract Animal GetAnimal();
}

interface IBioLabPetShop { 
  // Our pet shop can also clone cats (??)
  // (In principle, it would be correct to also have 
  // contravariant argument types, but C# does not
  // directly support that.)
  public abstract Animal CloneCat(Cat c);

}

class CatShop : PetShop, IBioLabPetShop {
  // For inherited virtual method, we can give a more
  // specific return type (note this does not work with 
  // interfaces) so this pet shop only returns cats.
  public override Cat GetAnimal() {
    return new Cat();
  }

  // In principle, we can write CloneCat that actually 
  // clones any animal, but we cannot use this directly 
  // to implement the interface. But we can add it as
  // a new method...
  public Animal CloneCat(Animal a) {
    return new Cat();
  }
  
  // ...and implement the interface explicitly and
  // just call our new method to clone the cat.
  Animal IBioLabPetShop.CloneCat(Cat c) {
    return CloneCat(c);
  }
}

#endregion
#region Extras - Contravariant interface example (Logger)

// An interface represents a logger that can log
// values of type T. Since T only appears in arguments
// of methods, we can mark it as contra-variant.
interface ILogger<in T> {
  public void Log(T t);
}

// Concrete implementation of a logger for any Animal
class AnimalLogger : ILogger<Animal> {
  public void Log(Animal t) {
    Console.WriteLine("logging some animal");
  }
}

static class LoggerDemo {
  // This method takes a logger that can log cats and logs three cats
  public static void LogThreeCats(ILogger<Cat> logger) {
    logger.Log(new Cat()); 
    logger.Log(new Cat());
    logger.Log(new Cat());
  }
  public static void Run() {
    // AnimalLogger implements ILogger<Animal>, *not* ILogger<Cat>
    // but thanks to contravariance this works. Remove the `in`
    // kewyord in the `ILogger` declaration and see that it will stop working!
    //
    // What's going on?
    // * Cat is a subtype of Animal (Cat can be used where Animal is expected) 
    // * ILogger<Animal> becomes a subtype of ILogger<Cat> (ILogger<Animal>
    //   can be used where ILogger<Cat> is expected)
    // * The direction of the "subtyping" relation is reversed!!
    //   (with covariance, the direction stays the same).
    LogThreeCats(new AnimalLogger());
  }
}
}
#endregion

#region Fixed point - conversion demos

public static class ConversionDemos {
  public static void Run() {
    Console.WriteLine("WWWWWFFF");
    
    byte b1 = byte.CreateTruncating(1.25 * (1 << 3));
    Console.WriteLine(b1.ToString("B").PadLeft(8, '0'));

    byte b2 = byte.CreateTruncating(0.875 * (1 << 3));
    Console.WriteLine(b2.ToString("B").PadLeft(8, '0'));

    byte b3 = byte.CreateTruncating(31.875 * (1 << 3));
    Console.WriteLine(b3.ToString("B").PadLeft(8, '0'));

    byte b4 = byte.CreateTruncating(31.9 * (1 << 3));
    Console.WriteLine(b3.ToString("B").PadLeft(8, '0'));

    Console.WriteLine("\nDOUBLE");
    Console.WriteLine((double)b1 / (1 << 3));
    Console.WriteLine((double)b2 / (1 << 3));
    Console.WriteLine((double)b3 / (1 << 3));
    Console.WriteLine((double)b4 / (1 << 3));
  }
}

#endregion
#region Fixed point - sample implementation

public interface IFractionalPartDefinition {
  public abstract static int Bits { get; }
}
public sealed class Dot3 : IFractionalPartDefinition {
  public static int Bits => 3;
}
public sealed class Dot4 : IFractionalPartDefinition {
  public static int Bits => 4;
}

public readonly struct Fixed<TBase, TDot> :
	IAdditionOperators<Fixed<TBase, TDot>, Fixed<TBase, TDot>, Fixed<TBase, TDot>>, 
	ISubtractionOperators<Fixed<TBase, TDot>, Fixed<TBase, TDot>, Fixed<TBase, TDot>>,
  IMultiplyOperators<Fixed<TBase, TDot>, Fixed<TBase, TDot>, Fixed<TBase, TDot>>,
  IDivisionOperators<Fixed<TBase, TDot>, Fixed<TBase, TDot>, Fixed<TBase, TDot>>,
  IAdditiveIdentity<Fixed<TBase, TDot>, Fixed<TBase, TDot>>
  where TBase : IBinaryInteger<TBase>, IConvertible
	where TDot : IFractionalPartDefinition {
  public static Fixed<TBase, TDot> AdditiveIdentity => new(0.0);

  public TBase Value { get; init; }
  public Fixed(double value) {
    double normalized = value * (1 << TDot.Bits);
    Value = TBase.CreateTruncating(normalized);
  }
  public double ToDouble() {
    return Value.ToDouble(null) / (1 << TDot.Bits);
  }
  public static Fixed<TBase, TDot> operator +(Fixed<TBase, TDot> left, Fixed<TBase, TDot> right) {
    return new Fixed<TBase, TDot> { Value = left.Value + right.Value };
  }

  public static Fixed<TBase, TDot> operator -(Fixed<TBase, TDot> left, Fixed<TBase, TDot> right) {
    return new Fixed<TBase, TDot> { Value = left.Value - right.Value };
  }

  public static Fixed<TBase, TDot> operator *(Fixed<TBase, TDot> left, Fixed<TBase, TDot> right) {
    long uncorrected = left.Value.ToInt64(null) * right.Value.ToInt64(null);
    return new Fixed<TBase, TDot> { Value = TBase.CreateTruncating(uncorrected >> TDot.Bits) };
  }

  public static Fixed<TBase, TDot> operator /(Fixed<TBase, TDot> left, Fixed<TBase, TDot> right) {
    long leftCorrected = left.Value.ToInt64(null) << TDot.Bits;
    long result = leftCorrected / right.Value.ToInt64(null);
    return new Fixed<TBase, TDot> { Value = TBase.CreateTruncating(result) };
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

  	x1 += x2;
    Console.WriteLine(x1.Value.ToString("B").PadLeft(8, '0'));
	  Console.WriteLine(x1.ToDouble());
  }
}
#endregion
#region Fixed point - sum all

public static class NumberListExtensions {
  public static T SumAll<T>(this IEnumerable<T> items) where 
      T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T> {
    var res = T.AdditiveIdentity;
    foreach(var item in items) {
      res += item;
    }
    return res;
  }
}

public static class SumAllDemos {
  public static void Run() {
    var l1 = new []{ new Fixed<byte, Dot3>(1.25), new Fixed<byte, Dot3>(2.5), new Fixed<byte, Dot3>(0.25) };
    Console.WriteLine(l1.SumAll().ToDouble());
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
    for(var i = 0; i < items.Count; i++) {
      Console.WriteLine(items[i]);
    }
  }

  public static void PrintAllReverse(List<Animal> items) {
    for(var i = items.Count - 1; i <= 0; i--) {
      Console.WriteLine(items[i]);
    }
  }

  public static void Run() {
    var c1 = new Animal[] { new Cat(), new Dog(), new Cat(), new Cat() };
    var c2 = new List<Animal>() { new Cat(), new Dog(), new Cat(), new Cat() };
    var c3 = c2.Take(3);
    var c4 = new List<Cat>() { new Cat(), new Cat() };
    
    // PrintAll(c1); -- Array is not List (but it is IList, IReadOnlyCollection, IEnumerable)
    PrintAll(c2);
    // PrintAll(c3); -- IEnumerable is not List (just IEnumerable)
    // PrintAll(c4); -- We need covariant interface (IReadOnlyCOllection or IEnumerable)

    PrintAllReverse(c2);
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