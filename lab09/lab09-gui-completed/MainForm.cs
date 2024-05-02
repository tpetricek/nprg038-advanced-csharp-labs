using System.Reactive.Linq;

namespace lab09_gui_demo {
  public partial class MainForm : Form {
    List<(int x, int y, Brush brush)> points = new List<(int, int, Brush)>();
    Random random = new Random();

    public MainForm() {
      InitializeComponent();

      var downs = Observable.FromEventPattern<MouseEventArgs>(this, "MouseDown");
      var dots = 
        from d in downs 
        let x = d.EventArgs.X
        let y = d.EventArgs.Y
        where Math.Pow(Width / 2 - x, 2) + 
          Math.Pow(Height / 2 - y, 2) < Math.Pow(300, 2)
        let rgb = random.Next(0xffffff+1)
        let brush = new SolidBrush(Color.FromArgb(rgb+0x78000000))
        select (d.EventArgs.X, d.EventArgs.Y, brush);

      dots.Subscribe(pt => { points.Add(pt); this.Refresh(); });
    }

    protected override void OnPaint(PaintEventArgs e) {
      base.OnPaint(e);
      foreach(var (x, y, brush) in points) { 
        e.Graphics.FillEllipse(brush, new Rectangle(x-20, y-20, 40, 40));
      }
    }
  }
}
