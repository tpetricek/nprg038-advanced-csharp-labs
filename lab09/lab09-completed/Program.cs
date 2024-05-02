using System.Collections;
using System.Runtime.CompilerServices;

EnumeratorDemo.Run();
// CaptureDemos.Run();
// LinqToObjects.Run();
// InifiniteLinq.Run(); 

#region Deque enumerator

public class IntArrayArray : IEnumerable<int> {
  int[][] data;
  int count = 0;
  const int BLOCK_SIZE = 10;

  public IntArrayArray(int[][] data, int count) {
    this.data = data;
    this.count = count;
  }

  public int this[int index] {
    get {
      if (index < 0 || index >= count) throw new IndexOutOfRangeException(); 
      return data[index / BLOCK_SIZE][index % BLOCK_SIZE];
    }
  }

  // Easier approach - by using single index
  class IntArrayArrayEnumerator : IEnumerator<int> {
    public IntArrayArray arar;
    public int index = -1;

    public int Current => arar[index];

    object IEnumerator.Current => arar[index];

    public void Dispose() { }

    public bool MoveNext() {
      index++;
      return index < arar.count;
    }

    public void Reset() {
      index = -1; 
    }
  }

  // VERSION #1: Using an index 
  public IEnumerator<int> GetEnumerator_v1() {
    for(int index = 0; index < count; index++) {
      yield return data[index / BLOCK_SIZE][index % BLOCK_SIZE];
    }
  }

  // VERSION #2: Using nested loops
  public IEnumerator<int> GetEnumerator_v2() {
    int returned = 0;
    for(int i = 0; i < data.Length; i++) { 
      for(int j = 0; j < BLOCK_SIZE; i++) {
        if (returned > count) yield break;
        yield return data[i][j];
      }
    }
  }

  // VERSION #3: Explicit implementation
  public IEnumerator<int> GetEnumerator() {
    return new IntArrayArrayEnumerator() { arar = this };
  }

  IEnumerator IEnumerable.GetEnumerator() {
    throw new NotImplementedException();
  }
}

public class EnumeratorDemo {
  public static void Run() {
    int[][] data = new[] {
      Enumerable.Range(0, 10).ToArray(),
      Enumerable.Range(20, 10).ToArray(),
      Enumerable.Range(40, 10).ToArray()
    };
    var arar = new IntArrayArray(data, 25);
    foreach(var el in arar) {
      Console.WriteLine(el);
    }
  }
}

#endregion
#region Lambdas - variable capture demo

class CaptureDemos {
  public static void Run() {
    RunOne();
    RunTwo();
  }

  public static void RunOne() {
    var actions = new List<Action>();
    for(var i = 0; i < 10; i++) {
      actions.Add(() => Console.WriteLine(i));
    }
    foreach(var action in actions) {
      action();
    }
  }
  public static void RunTwo() {

    var actions = new List<Action>();
    for(var i = 0; i < 10; i++) {
      var j = i;
      actions.Add(() => Console.WriteLine(j));
    }
    foreach(var action in actions) {
      action();
    }
  }
}

#endregion
#region LINQ - basic linq to objects

class LinqToObjects {
  public static void Run() {
    var passwords = ("123456,password,12345678,qwerty," +
      "123456789,12345,1234,111111,1234567,dragon").Split(',');

    var over6 = 
      from p in passwords 
      where p.Length > 6
      select p;

    var over6_ = passwords
      .Where(p => p.Length > 6);

    Console.WriteLine(string.Join(",", over6));

    var numeric = 
      from p in passwords 
      where !p.Any(c => c < '0' || c > '9')
      select p.Length;

    var numeric_ = passwords
      .Where(p => !p.Any(c => c < '0' || c > '9'))
      .Select(p => p.Length);

    Console.WriteLine(string.Join(",", numeric));

    var lengths = 
      from p in passwords 
      let l = p.Length
      where l > 6
      orderby l
      select l;

    var lengths_ = passwords
      .Select(p => new { p, l = p.Length })
      .Where(p => p.l > 6)
      .OrderBy(p => p.l)
      .Select(p => p.l);

    Console.WriteLine(string.Join(",", lengths));
  }
}

#endregion
#region LINQ - infinite sequences

public class InifiniteLinq {
  public static IEnumerable<string> RandomPasswords() {
    var letters = "abcdefghijklmnopqrstupvwxyz";
    var numbers = "0123456789";
    var all = letters + letters.ToUpper() + numbers;      
    var rnd = new Random();
    while(true) {
      var chars = Enumerable.Range(1, rnd.Next(30))
        .Select(_ => all[rnd.Next(all.Length)]);
      yield return new string(chars.ToArray());      
    }
  }

  public static void Run() {
    var ahojs = 
      from p in RandomPasswords()
      // Works: where p.Contains("ahoj")
      // Breaks: orderby p.Length
      select p;

    foreach(var passwords in ahojs) 
      Console.WriteLine(passwords);
  }
}

#endregion
