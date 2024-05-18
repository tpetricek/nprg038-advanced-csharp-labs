LockingDemo.Run();

public class Point {
  private int x;
  private int y;
  public int X { get { return x; } }
  public int Y { get { return y; } }

  public Point(int x, int y) { this.x = x; this.y = y; }
  public void Rotate() {
    // Swap X and Y coordinates
  }
  public void Ratio() {
    // Divide both by their sum
  }
}
public static class LockingDemo {
  public static void Run() {
    Point pt = new Point(0, 42);  // TODO: Change to 0, 0 and see what happens! 
    // Parallel.For(0, 100, i => {
    // });
    Console.WriteLine($"{pt.X}, {pt.Y}");
  }
}
