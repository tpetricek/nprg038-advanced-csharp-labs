ExcelUntyped.Run();
// ExcelHungarian.Run();
// ExcelTyped.Run();
// ExcelImmutable.Run();
// ExcelExtensions.Run();

// DemoExtensions.Run();
// DemoMutable.Run();

#region Excel - Untyped

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
#region Excel - Hungarian

public static class ExcelHungarian
{
  public static int Lookup(int row, int column)
  {
    if (row == 1 && column == 5) return 100;
    return 0;
  }

  public static void Run()
  {
    var inputRow = 1;
    var inputCol = 5;
    var v1 = Lookup(inputRow, inputCol);
    Console.WriteLine(v1);
    var v2 = Lookup(inputCol, inputRow);
    Console.WriteLine(v2);
  }
}

#endregion
#region Excel - Typed

public struct Row {
  public int Value { get; set; }
}

public struct Column {
  public int Value { get; set; }
}

public static class ExcelTyped {
  public static int Lookup(Row r, Column c) {
    if (r.Value == 1 && c.Value == 5) return 100;
    return 0;
  }

  public static void Run() {
    var r = new Row { Value = 1 };
    var c = new Column { Value = 5 };
    var v1 = Lookup(r, c);
    Console.WriteLine(v1);
    // var v2 = Lookup(c, r);
    // Console.WriteLine(v2);
  }
}

#endregion
#region Excel - Immutable

public struct ImmutableRow {
  public int Value { get; }
  public ImmutableRow(int value) { Value = value; } 
}

public struct ImmutableColumn {
  public int Value { get; }
  public ImmutableColumn(int value) { Value = value; }
}

public static class ExcelImmutable{
  public static int Lookup(ImmutableRow r, ImmutableColumn c) {
    if (r.Value == 1 && c.Value == 5) return 100;
    return 0;
  }

  public static void Run() {
    var r = new ImmutableRow(1);
    var c = new ImmutableColumn(5);
    var v1 = Lookup(r, c);
    Console.WriteLine(v1);
  }
}

#endregion
#region Excel - Extensions

public static class IndexExtensions { 
  public static ImmutableRow AsRow(this int index) => 
    new ImmutableRow(index);
  public static ImmutableColumn AsColumn(this int index) =>
    new ImmutableColumn(index);
}

public static class ExcelExtensions {
  public static int Lookup(ImmutableRow r, ImmutableColumn c) {
    if (r.Value == 1 && c.Value == 5) return 100;
    return 0;
  }

  public static void Run() {
    var r = 1.AsRow();
    var c = 5.AsColumn();
    var v1 = Lookup(r, c);
    Console.WriteLine(v1);
  }
}

#endregion

#region Demo - Extension methods

public static class RandomExtensions { 
  public static int NextDice(this Random rnd) => rnd.Next(1, 7);
}

public static class DemoExtensions {
  public static void Run() { 
    var rnd = new Random();
    for(int i = 0; i < 10; i++) {
      Console.WriteLine(rnd.NextDice());
    }
  }
}

#endregion
#region Demo - Generic extension methods

public class Series<K, V> {
  public K[] Keys;
  public V[] Values;
  public Series(K[] keys, V[] values) {
    Keys = keys; Values = values;
  }
}

public static class SeriesExtensions {
  public static double Average<T>(this Series<T, double> series) {
    return series.Values.Average();
  }
}

public static class DemoGenericExtensions {
  public static void Run() {
    var ds = new Series<string, double>(
        new[] { "Monday", "Tuesday", "Wednesday" },
        new[] { 10.1, 12.1, 15.1 }
      );
    Console.WriteLine(ds.Average());
  }
}

#endregion
#region Demo - Mutable structs

public struct Point {
  public int X;
  public int Y;
  
  public void Reset() { X = 0; Y = 0; }
}

public static class PointExtensions { 
  public static void MoveX(this ref Point p, int by) {
    p.X += by;
  }
  public static void MoveY(this Point p, int by) {
    p.Y += by;
  }
}

public static class DemoMutable { 
  public static void Run() {
    var pt = new Point { X = 100, Y = 10 };
    pt.Reset();
    
    pt.MoveX(99);
    PointExtensions.MoveX(ref pt, 99);

    pt.MoveY(99);
    PointExtensions.MoveY(pt, 99);

    Console.WriteLine($"({pt.X}, {pt.Y})");
  }
}

#endregion