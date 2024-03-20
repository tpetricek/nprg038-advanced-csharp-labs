using System.Drawing;

Units.Run();
// FancyUnits.Run();
// Panels.Run();

#region Homework #1 - Basic unit system

public struct MeterPerSecond {
  internal double value;
}
public struct Meter {
  internal double value;
}
public struct Second {
  internal double value;
}
public static class UnitExtensions {
}

public static class Units {
  public static void Run() {
			// var dist = 1.5.Meters() + 2.Meters();
			// Console.WriteLine("Distance: " + dist.value);

			// var time = 3.Seconds();
			// var speed = dist / time;
			// Console.WriteLine("Speed: " + speed.value);

			// speed *= 2;
			// Console.WriteLine("Doubled speed: " + speed.value);

			// Should not compile
			// speed *= dist;
			// distance += time;
  }
}

#endregion
#region Homework #1 - Fancy unit system

public static class FancyUnitExtensions {
}

public static class FancyUnits {
  public static void Run() {			
		// var dist = 1.5.FancyMeters() + 2.FancyMeters();
		// Console.WriteLine("Distance: " + dist.value);
  }
}

#endregion
#region Homework #2 - Basic panels

public abstract class Control { }
public class Label : Control {
  public string Text { get; set; }
	public Label WithText(string text) { Text = text; return this; }
}

public abstract	class Panel {
}
public class StackPanel : Panel { 
}
public class  Canvas : Panel {
}
public static class ControlExtensions {
	public static T PlacedIn<T>(this T control, Panel panel) where T : Control {
		throw new NotImplementedException("oops");
	}
}
public static class Panels {
	public static void Run() {
		var stack = new StackPanel();
		var canvas = new Canvas();

		new Label().WithText("Hello").PlacedIn(stack).WithText("Hello!");
		//new Label().WithText("Hello").PlacedIn(canvas).WithText("Hello!");
		//TODO: new Label().WithText("Hello").PlacedIn(canvas).At(10, 10).WithText("Hello!");
	}
}

#endregion