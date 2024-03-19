using System.Drawing;

Panels.Run();

#region Homework #1 - Basic unit system

public struct MeterPerSecond {
  internal double value;
	public static MeterPerSecond operator *(MeterPerSecond a, int b) => 
		new MeterPerSecond { value = a.value + b };
}
public struct Meter {
  internal double value;
	public static Meter operator +(Meter a, Meter b) => 
		new Meter { value = a.value + b.value };
}
public struct Second {
  internal double value;
	public static MeterPerSecond operator /(Meter a, Second b) => 
		new MeterPerSecond { value = a.value / b.value };
}
public static class UnitExtensions {
  public static Meter Meters(this double value) => new Meter { value = value };
  public static Second Seconds(this double value) => new Second { value = value };
  public static Meter Meters(this int value) => new Meter { value = value };
  public static Second Seconds(this int value) => new Second { value = value };
}

public static class Units {
  public static void Run() {
			var dist = 1.5.Meters() + 2.Meters();
			Console.WriteLine("Distance: " + dist.value);

			var time = 3.Seconds();
			var speed = dist / time;
			Console.WriteLine("Speed: " + speed.value);

			speed *= 2;
			Console.WriteLine("Doubled speed: " + speed.value);

			// Should not compile
			// speed *= dist;
			// distance += time;
  }
}

#endregion
#region Homework #1 - Fancy unit system

public class MeterUnit { }
public class SecondUnit { }
public class MeterPerSecondUnit { }

public struct Number<T> {
  internal double value;
	public static Number<T> operator *(Number<T> a, int b) => 
		new Number<T> { value = a.value + b };
	public static Number<T> operator +(Number<T> a, Number<T> b) => 
		new Number<T> { value = a.value + b.value };
	public static Number<MeterPerSecond> operator /(Number<T> a, Number<SecondUnit> b) => 
		new Number<MeterPerSecond> { value = a.value / b.value };
}

public static class FancyUnitExtensions {
  public static Number<MeterUnit> FancyMeters(this double value) => new Number<MeterUnit> { value = value };
  public static Number<SecondUnit> FancySeconds(this double value) => new Number<SecondUnit> { value = value };
  public static Number<MeterUnit> FancyMeters(this int value) => new Number<MeterUnit> { value = value };
  public static Number<SecondUnit> FancySeconds(this int value) => new Number<SecondUnit> { value = value };
}

public static class FancyUnits {
  public static void Run() {
			var dist = 1.5.FancyMeters() + 2.FancyMeters();
			Console.WriteLine("Distance: " + dist.value);

			var time = 3.FancySeconds();
			var speed = dist / time;
			Console.WriteLine("Speed: " + speed.value);

			speed *= 2;
			Console.WriteLine("Doubled speed: " + speed.value);

			// Should not compile
			// speed *= dist;
			// distance += time;
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
  protected List<Control> Children { get; } = new List<Control>();
}
public class StackPanel : Panel { 
	public void AddChild(Control child) {
		Children.Add(child);
	}
}
public class  Canvas : Panel {
	protected List<Point> Locations { get; } = new List<Point>();
	public void AddChild(Control child, int x, int y) {
		Children.Add(child);
		Locations.Add(new Point(x, y));
	}  
}
public struct UnresolvedCanvasPlacement<TOriginalControl> where TOriginalControl : Control {
	private Canvas canvas;
	private TOriginalControl control;

	public UnresolvedCanvasPlacement(Canvas targetCanvas, TOriginalControl placedControl) {
		canvas = targetCanvas;
		control = placedControl;
	}

	public TOriginalControl At(int x, int y) {
		canvas.AddChild(control, x, y);
		return control;
	}
}
public static class ControlExtensions {
	public static T PlacedIn<T>(this T control, StackPanel panel) where T : Control {
		panel.AddChild(control);
		return control;
	}
	public static UnresolvedCanvasPlacement<T> PlacedIn<T>(this T control, Canvas panel) where T : Control {
		return new UnresolvedCanvasPlacement<T>(panel, control);
	}
}
public static class Panels {
	public static void Run() {
		var stack = new StackPanel();
		var canvas = new Canvas();

		new Label().WithText("Hello").PlacedIn(stack).WithText("Hello!");
		//new Label().WithText("Hello").PlacedIn(canvas).WithText("Hello!");
		new Label().WithText("Hello").PlacedIn(canvas).At(10, 10).WithText("Hello!");
	}
}

#endregion