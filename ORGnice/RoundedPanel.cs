using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ORGnice
{
    public class RoundedPanel : Panel
    {
        public int CornerRadius { get; set; } = 16;

        // Slightly stronger but still subtle shadow
        public Color ShadowColor { get; set; } = Color.FromArgb(35, 0, 0, 0); // more visible
        public int ShadowOffset { get; set; } = 3; // slightly bigger offset

        public RoundedPanel()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            this.Padding = new Padding(16);

            // Disable default background painting
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Clear to parent background so no excess color shows
            g.Clear(this.Parent?.BackColor ?? Color.Transparent);

            int r = CornerRadius;
            int offset = ShadowOffset;

            // Shadow rectangle slightly offset bottom-right
            Rectangle shadowRect = new Rectangle(
        offset,
        offset,
        this.Width - offset - 1,
        this.Height - offset - 1
      );

            // Main panel rectangle
            Rectangle panelRect = new Rectangle(
        0,
        0,
        this.Width - offset - 1,
        this.Height - offset - 1
      );

            // Draw shadow
            using (GraphicsPath shadowPath = CreateRoundedRect(shadowRect, r))
            using (SolidBrush shadowBrush = new SolidBrush(ShadowColor))
            {
                g.FillPath(shadowBrush, shadowPath);
            }

            // Draw main panel
            using (GraphicsPath panelPath = CreateRoundedRect(panelRect, r))
            using (SolidBrush panelBrush = new SolidBrush(this.BackColor))
            {
                g.FillPath(panelBrush, panelPath);
            }
        }

        private GraphicsPath CreateRoundedRect(Rectangle rect, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);               // Top-left
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);       // Top-right
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90); // Bottom-right
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);       // Bottom-left
            path.CloseFigure();
            
            return path;
        }
    }
}