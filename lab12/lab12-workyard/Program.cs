CoroutinesDemo.Run();

#region Mini coroutines

class CoroutineStep { 
  public static CoroutineStep Instance = new CoroutineStep();
}

class CoroutinesDemo {

  // TODO: Add total work counter

  public static IEnumerable<CoroutineStep> Worker(string name, int count) {
    while(count > 0) { 
      Console.WriteLine($"{name}: {count}");
      count--;
    }
    yield break;
  }

  public static void Execute(params IEnumerable<CoroutineStep>[] workers) { 
    // TODO: Do all the work
  }

  public static void Run() {
    Execute(
      Worker("Ada", 20),
      Worker("Grace", 10),
      Worker("Margaret", 15)
    );
  }
}

#endregion