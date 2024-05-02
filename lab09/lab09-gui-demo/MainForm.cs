using System.Reactive.Linq;

namespace lab09_gui_demo {
  public partial class MainForm : Form {
    List<(int x, int y, Brush brush)> points = new List<(int, int, Brush)>();
    Random random = new Random();

    public MainForm() {
      InitializeComponent();

      var downs = Observable.FromEventPattern<MouseEventArgs>(this, "MouseDown");

      // TODO: let x, let y and select tuple
      // TODO: add where for some regions only
      // TODO: Math.Pow(Width / 2 - x, 2)
      // TODO: RGBA: 0x78000000 + (0xffffff+1)

      downs.Subscribe(pt => { 
        points.Add((pt.EventArgs.X, pt.EventArgs.Y, Brushes.Black)); 
        this.Refresh(); 
      });
    }

    protected override void OnPaint(PaintEventArgs e) {
      base.OnPaint(e);
      foreach(var (x, y, brush) in points) { 
        e.Graphics.FillEllipse(brush, new Rectangle(x-20, y-20, 40, 40));
      }
    }
  }
}
