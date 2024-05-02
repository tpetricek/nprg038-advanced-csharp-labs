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
      // TODO: Implement me
      throw new NotImplementedException();
    }
  }

  // VERSION #1: Using an index 
  public IEnumerator<int> GetEnumerator_v1() {
    throw new NotImplementedException();
  }

  // VERSION #2: Using nested loops
  public IEnumerator<int> GetEnumerator_v2() {
    throw new NotImplementedException();
  }

  // VERSION #3: Explicit implementation
  public IEnumerator<int> GetEnumerator() {
    throw new NotImplementedException();
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

    // Over 6 characters 
    // Console.WriteLine(string.Join(",", over6));

    // Length of only numerical
    // Console.WriteLine(string.Join(",", numeric));

    // Lenghts of l > 6 with let
    // Console.WriteLine(string.Join(",", lengths));
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
    // TODO: Passwords with ahoj!
    foreach(var passwords in RandomPasswords()) 
      Console.WriteLine(passwords);
  }
}

#endregion
