ExcelUntyped.Run();

#region Excel

public static class ExcelUntyped {
  public static int Lookup(int i, int j) {
    if (i == 1 && j == 5) return 100;
    return 0;
  }

  public static void Run() {
    var i = 1;
    var j = 5;
    var v1 = Lookup(i, j);
    Console.WriteLine(v1);
    var v2 = Lookup(j, i);
    Console.WriteLine(v2);
  }
}

#endregion

#region Extensions

public class Animal {
  public virtual void MakeSound() { Console.Write("Humph"); }
}

public class Cat {
}

public static class DemoExtensions {
  public static void Run() { 
    var rnd = new Random();
    for(int i = 0; i < 10; i++) {
      Console.WriteLine(rnd.Next()); // TODO: NextDice
    }
  }
}

#endregion

#region Series

public class Series<K, V> {
  public K[] Keys;
  public V[] Values;
  public Series(K[] keys, V[] values) {
    Keys = keys; Values = values;
  }
}

public static class DemoGenericExtensions {
  public static void Run() {
    var ds = new Series<string, double>(
        new[] { "Monday", "Tuesday", "Wednesday" },
        new[] { 10.1, 12.1, 15.1 }
      );
    // TODO: Console.WriteLine(ds.Average());
  }
}

#endregion

#region Mutable

public struct Point {
  public int X;
  public int Y;
}

public static class PointExtensions { 
}

public static class DemoMutable { 
  public static void Run() {
    var pt = new Point { X = 100, Y = 10 };

    // TODO!
    // pt.Reset();

    // TODO!    
    // pt.MoveX(99);
    // PointExtensions.MoveX(pt, 99);

    Console.WriteLine($"({pt.X}, {pt.Y})");
  }
}

#endregion