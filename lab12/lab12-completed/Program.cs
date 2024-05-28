CoroutinesDemo.Run();

#region Mini coroutines

class CoroutineStep { 
  public static CoroutineStep Instance = new CoroutineStep();
}

class CoroutinesDemo {

  public static int total = 0;

  public static IEnumerable<CoroutineStep> Worker(string name, int count) {
    while(count > 0) { 
      // Do one piece of work
      Console.WriteLine($"{name}: {count}");
      count--;
      // Increment total work counter
      int newTotal = total + 1;
      total = newTotal;
      // Take a break and let others run
      yield return CoroutineStep.Instance;
    }
    yield break;
  }

  public static void Execute(params IEnumerable<CoroutineStep>[] workers) { 
    var enumerators = workers.Select(w => w.GetEnumerator()).ToList();
    bool anyWorking = true;
    while(anyWorking) {
      anyWorking = false;
      foreach(var en in enumerators) {
        if (en.MoveNext()) anyWorking = true;
      }
    }
  }

  public static void Run() {
    Execute(
      Worker("Ada", 20),
      Worker("Grace", 10),
      Worker("Margaret", 15)
    );
    Console.WriteLine($"Total work: {total}");
  }
}

#endregion