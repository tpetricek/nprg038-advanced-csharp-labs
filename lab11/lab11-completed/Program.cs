V3.LockingDemo.Run();

#region Starting point without locks

namespace V1 { 
  public class Point {
    private int x;
    private int y;
    public int X { get { return x; } }
    public int Y { get { return y; } }

    public Point(int x, int y) { this.x = x; this.y = y; }
    public void Rotate() {
      var oldx = this.x;
      this.x = this.y;
      this.y = oldx;
    }
    public void Ratio() {
      this.x = this.x / (this.x + this.y);
      this.y = this.y / (this.x + this.y);
    }
  }
  public static class LockingDemo {
    public static void Run() {
      Point pt = new Point(0, 42); 
      Parallel.For(0, 100, i => {
        pt.Rotate();
        // TODO: Add: try { pt.Ratio() } catch { } 
      });
      Console.WriteLine($"{pt.X}, {pt.Y}");
    }
  }
}

#endregion
#region Incorrect locking using Monitor

namespace V2 { 
  public class Point {
    private int x;
    private int y;
    public int X { get { return x; } }
    public int Y { get { return y; } }

    public object lockObj = new object();

    public Point(int x, int y) { this.x = x; this.y = y; }
    public void Rotate() {
      Monitor.Enter(lockObj);
      var oldx = this.x;
      this.x = this.y;
      this.y = oldx;
      Monitor.Exit(lockObj);
    }
    public void Ratio() {
      // TODO: Fix me with try { .. } finally { .. }
      Monitor.Enter(lockObj);
      this.x = this.x / (this.x + this.y);
      this.y = this.y / (this.x + this.y);
      Monitor.Exit(lockObj);
    }
  }
  public static class LockingDemo {
    public static void Run() {
      Point pt = new Point(0, 42); // TODO: Change to 0, 0 and see what happens! 
      Parallel.For(0, 100, i => {
        pt.Rotate();
      });
      Console.WriteLine($"{pt.X}, {pt.Y}");
    }
  }
}

#endregion
#region Using the lock syntax

namespace V3 { 
  public class Point {
    private int x;
    private int y;
    public int X { get { return x; } }
    public int Y { get { return y; } }

    public object lockObj = new object();

    public Point(int x, int y) { this.x = x; this.y = y; }
    public void Rotate() {
      lock(lockObj) { 
        var oldx = this.x;
        this.x = this.y;
        this.y = oldx;
      }
    }
    public void Ratio() {
      lock(lockObj) { 
        this.x = this.x / (this.x + this.y);
        this.y = this.y / (this.x + this.y);
      }
    }
  }
  public static class LockingDemo {
    public static void Run() {
      Point pt = new Point(0, 42); // TODO: Change to 0, 0 and see what happens! 
      Parallel.For(0, 100, i => {
        pt.Rotate();
      });
      Console.WriteLine($"{pt.X}, {pt.Y}");
    }
  }
}

#endregion