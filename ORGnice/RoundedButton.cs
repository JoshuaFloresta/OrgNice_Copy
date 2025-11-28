using System;
using System.Drawing;
using System.Drawing.Drawing2D; // Needed for GraphicsPath
using System.Windows.Forms;

public class RoundedButton : Button
{
    // --- Public Properties for Customization ---
    private int _cornerRadius = 10; // Default corner radius
    public int CornerRadius
    {
        get { return _cornerRadius; }
        set
        {
            _cornerRadius = value;
            this.Invalidate(); // Redraw the button when radius changes
        }
    }

    // --- Override OnPaint to draw the rounded button ---
    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent); // Call the base class's OnPaint first

        Graphics graph = pevent.Graphics;
        graph.SmoothingMode = SmoothingMode.AntiAlias; // For smoother edges

        Rectangle rectSurface = this.ClientRectangle; // Area to draw on
        Rectangle rectBorder = Rectangle.Inflate(rectSurface, -1, -1); // Slightly smaller for border

        // Create a GraphicsPath for the rounded rectangle
        using (GraphicsPath pathSurface = GetRoundedRectanglePath(rectSurface, _cornerRadius))
        using (GraphicsPath pathBorder = GetRoundedRectanglePath(rectBorder, _cornerRadius - 1)) // Border path
        {
            // Draw button surface (background color)
            graph.FillPath(new SolidBrush(this.BackColor), pathSurface);

            // Draw button text
            TextRenderer.DrawText(graph, this.Text, this.Font, rectSurface, this.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            // Draw border (if not Flat style)
            if (this.FlatStyle == FlatStyle.Standard) // You might want to customize this for Flat style
            {
                using (Pen penBorder = new Pen(this.FlatAppearance.BorderColor, 1))
                {
                    graph.DrawPath(penBorder, pathBorder);
                }
            }
        }
    }

    // --- Helper method to create a rounded rectangle path ---
    private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        if (radius <= 0) // No rounding needed
        {
            path.AddRectangle(rect);
            return path;
        }

        int diameter = radius * 2;
        Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));

        // Top-left arc
        path.AddArc(arcRect, 180, 90);

        // Top-right arc
        arcRect.X = rect.Right - diameter;
        path.AddArc(arcRect, 270, 90);

        // Bottom-right arc
        arcRect.Y = rect.Bottom - diameter;
        path.AddArc(arcRect, 0, 90);

        // Bottom-left arc
        arcRect.X = rect.Left;
        path.AddArc(arcRect, 90, 90);

        path.CloseFigure(); // Connects the path segments
        return path;
    }

    // --- Override other events for better visual feedback (optional) ---

    // Ensure the button redraws itself when its size changes
    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        this.Invalidate();
    }

    // Optional: Add hover effect to BackColor for custom buttons
    private Color defaultBackColor;
    private Color hoverBackColor = Color.LightGray; // Customize hover color

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        defaultBackColor = this.BackColor; // Store initial color
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        this.BackColor = hoverBackColor;
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        this.BackColor = defaultBackColor;
    }
}