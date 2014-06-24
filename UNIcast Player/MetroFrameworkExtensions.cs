using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetroFramework;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.Design;
using MetroFramework.Controls;
using MetroFramework.Drawing;
using System.Drawing.Drawing2D;

namespace UNIcast_Streamer
{
    [ToolboxBitmap(typeof(Button))]
    public class ImageButton : MetroControlBase
    {
        protected Image DefaultImage;
        private Image _Image { get; set; }
        public Image MouseOverImage { get; set; }
        public Image DisabledImage { get; set; }
        public Image ClickedImage { get; set; }

        protected bool isImageSet;

        public ImageButton()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        public Image Image
        {
            get
            {
                return _Image;
            }
            set
            {
                _Image = value;
                if (!isImageSet)
                {
                    DefaultImage = _Image;
                    isImageSet = true;
                }
                Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Image != null)
                e.Graphics.DrawImage(Image, 0, 0, Image.Width, Image.Height);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (DefaultImage != null && DisabledImage != null)
                Image = Enabled ? DefaultImage : DisabledImage;
            base.OnEnabledChanged(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (MouseOverImage != null)
                Image = MouseOverImage;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (DefaultImage != null)
                Image = DefaultImage;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (ClickedImage != null)
                Image = ClickedImage;
            base.OnMouseClick(e);
        }
    }

    // Based on http://msdn.microsoft.com/en-us/magazine/cc163861.aspx
    [ToolboxBitmap(typeof(Ratings))]
    public class Ratings : MetroControlBase
    {

        public const int UserSuppliedImage = 0;
        public const int Circle = 1;

        public const int Square = 2;
        protected Image m_FilledImage;
        protected Image m_EmptyImage;
        protected Image m_HoverImage;
        protected int m_ImageCount = 5;
        protected int m_TopMargin = 2;
        protected int m_LeftMargin = 4;
        protected int m_BottomMargin = 2;
        protected int m_RightMargin = 4;
        protected int m_ImageSpacing = 8;

        protected int m_ImageToDraw = 1;
        protected Color m_SelectedColor = Color.Empty;
        protected Color m_HoverColor = Color.Empty;
        protected Color m_EmptyColor = Color.Empty;

        protected int m_selectedItem = 3;
        protected int m_hoverItem = 1;
        protected bool m_hovering = false;

        protected Rectangle[] ItemAreas;

        public Ratings()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            //add any additional initilization code here
            this.ItemAreas = new Rectangle[this.m_ImageCount];
        }


        public event EventHandler SelectedItemChanged;

        protected virtual void OnSelectedItemChanged()
        {
            EventArgs e = new EventArgs();
            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(this, e);
            }
        }


        #region "Properties"


        public virtual int SelectedItem
        {
            get { return m_selectedItem; }
            set
            {
                if (value >= 0 & value <= this.ImageCount + 1)
                {
                    m_selectedItem = value;
                    OnSelectedItemChanged();
                }
                else
                {
                    value = 0;
                }
            }
        }

        public virtual int HoverItem
        {
            get { return m_hoverItem; }
        }

        public virtual bool IsHovering
        {
            get { return m_hovering; }
        }

        public virtual Color SelectedColor
        {
            get
            {
                if (m_SelectedColor.Equals(Color.Empty))
                {
                    return this.ForeColor;
                }
                else
                {
                    return m_SelectedColor;
                }
            }
            set
            {
                if (!value.Equals(m_SelectedColor))
                {
                    m_SelectedColor = value;
                    this.Invalidate();
                }
            }
        }

        public virtual Color HoverColor
        {
            get
            {
                if (m_HoverColor.Equals(Color.Empty))
                {
                    return Color.FromKnownColor(KnownColor.Highlight);
                }
                else
                {
                    return m_HoverColor;
                }
            }
            set
            {
                if (!value.Equals(m_HoverColor))
                {
                    m_HoverColor = value;
                    this.Invalidate();
                }
            }
        }

        public virtual Color EmptyColor
        {
            get
            {
                if (m_EmptyColor.Equals(Color.Empty))
                {
                    return Color.FromKnownColor(KnownColor.Window);
                }
                else
                {
                    return m_EmptyColor;
                }
            }
            set
            {
                if (!value.Equals(m_EmptyColor))
                {
                    m_EmptyColor = value;
                    this.Invalidate();
                }
            }
        }

        public virtual Image FilledImage
        {
            get { return m_FilledImage; }
            set
            {
                if ((!object.ReferenceEquals(value, m_FilledImage)))
                {
                    m_FilledImage = value;
                    this.Invalidate();
                }
            }
        }

        public virtual Image EmptyImage
        {
            get { return m_EmptyImage; }
            set
            {
                if ((!object.ReferenceEquals(value, m_EmptyImage)))
                {
                    m_EmptyImage = value;
                    this.Invalidate();
                }
            }
        }

        public virtual Image HoverImage
        {
            get { return m_HoverImage; }
            set
            {
                if ((!object.ReferenceEquals(value, m_HoverImage)))
                {
                    m_HoverImage = value;
                    this.Invalidate();
                }
            }
        }

        public virtual int ImageToDraw
        {
            get { return m_ImageToDraw; }
            set
            {
                if (value != m_ImageToDraw)
                {
                    m_ImageToDraw = value;
                    this.Invalidate();
                }
            }
        }

        public virtual int LeftMargin
        {
            get { return m_LeftMargin; }
            set
            {
                if (value != m_LeftMargin)
                {
                    m_LeftMargin = value;
                    this.Invalidate();
                }
            }
        }

        public virtual int BottomMargin
        {
            get { return m_BottomMargin; }
            set
            {
                if (value != m_BottomMargin)
                {
                    m_BottomMargin = value;
                    this.Invalidate();
                }
            }
        }

        public virtual int RightMargin
        {
            get { return m_RightMargin; }
            set
            {
                if (value != m_RightMargin)
                {
                    m_RightMargin = value;
                    this.Invalidate();
                }
            }
        }

        public virtual int ImageSpacing
        {
            get { return m_ImageSpacing; }
            set
            {
                if (value != m_ImageSpacing)
                {
                    m_ImageSpacing = value;
                    this.Invalidate();
                }
            }
        }

        public virtual int ImageCount
        {
            get { return m_ImageCount; }
            set
            {
                if (value != this.m_ImageCount)
                {
                    this.m_ImageCount = value;
                    this.ItemAreas = new Rectangle[this.m_ImageCount];
                    this.Invalidate();
                }
            }
        }

        public virtual int TopMargin
        {
            get { return m_TopMargin; }
            set
            {
                if (value != m_TopMargin)
                {
                    m_TopMargin = value;
                    this.Invalidate();
                }
            }
        }

        #endregion

        #region "Drawing Routines"
        protected virtual void DrawUserSuppliedImage(Graphics g, int x, int y, int w, int h, int currentPos)
        {
            Image img = null;
            if (m_hovering & m_hoverItem > currentPos)
            {
                img = this.HoverImage;
            }
            else if (!m_hovering & m_selectedItem > currentPos)
            {
                img = this.FilledImage;
            }
            else
            {
                img = this.EmptyImage;
            }

            if ((img != null))
            {
                g.DrawImage(img, x, y, w, h);
            }
            else
            {
                this.DrawStandardImage(g, x, y, w, h, currentPos);
            }
        }


        protected virtual void DrawStandardImage(Graphics g, int x, int y, int w, int h, int currentPos)
        {
            Brush fillBrush = null;

            if (m_hovering & m_hoverItem > currentPos)
            {
                fillBrush = new SolidBrush(MetroPaint.GetStyleColor(Style));
            }
            else if (!m_hovering & m_selectedItem > currentPos)
            {
                fillBrush = new SolidBrush(MetroPaint.GetStyleColor(Style));
            }
            else
            {
                fillBrush = new SolidBrush(MetroPaint.ForeColor.Button.Disabled(Theme));
            }

            PointF[] pts = StarPoints(new Rectangle(x, y, w, h));
            g.FillPolygon(fillBrush, pts, System.Drawing.Drawing2D.FillMode.Winding);
        }
        #endregion

        private PointF[] StarPoints(Rectangle bounds)
        {
            // Make room for the points.
            int num_points = 5;
            PointF[] pts = new PointF[num_points];

            double rx = bounds.Width / 2;
            double ry = bounds.Height / 2;
            double cx = bounds.X + rx;
            double cy = bounds.Y + ry;

            // Start at the top.
            double theta = -Math.PI / 2;
            double dtheta = 4 * Math.PI / num_points;
            for (int i = 0; i < num_points; i++)
            {
                pts[i] = new PointF(
                    (float)(cx + rx * Math.Cos(theta)),
                    (float)(cy + ry * Math.Sin(theta)));
                theta += dtheta;
            }

            return pts;
        }

        #region "Overrides"

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int imageWidth = 0;
            int imageHeight = 0;
            imageWidth = (this.Width - (LeftMargin + RightMargin + (this.m_ImageSpacing * (this.m_ImageCount - 1)))) / this.m_ImageCount;
            imageHeight = (this.Height - (TopMargin + BottomMargin));

            Point start = new Point(this.LeftMargin, this.TopMargin);

            for (int i = 0; i <= this.ImageCount - 1; i++)
            {
                this.ItemAreas[i].X = start.X - this.ImageSpacing / 2;
                this.ItemAreas[i].Y = start.Y;
                this.ItemAreas[i].Width = imageWidth + this.ImageSpacing / 2;
                this.ItemAreas[i].Height = imageHeight;

                if (this.ImageToDraw == UserSuppliedImage)
                {
                    DrawUserSuppliedImage(e.Graphics, start.X, start.Y, imageWidth, imageHeight, i);
                }
                else
                {
                    DrawStandardImage(e.Graphics, start.X, start.Y, imageWidth, imageHeight, i);
                }
                start.X += imageWidth + this.ImageSpacing;
            }
            base.OnPaint(e);
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            for (int i = 0; i <= this.ImageCount - 1; i++)
            {
                if (this.ItemAreas[i].Contains(e.X, e.Y))
                {
                    this.m_hoverItem = i + 1;
                    this.Invalidate();
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnClick(System.EventArgs e)
        {
            Point pt = this.PointToClient(MousePosition);

            for (int i = 0; i <= this.ImageCount - 1; i++)
            {
                if (this.ItemAreas[i].Contains(pt))
                {
                    this.m_hoverItem = i + 1;
                    this.SelectedItem = i + 1;
                    this.Invalidate();
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            base.OnClick(e);
        }

        protected override void OnMouseEnter(System.EventArgs e)
        {
            this.m_hovering = true;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            this.m_hovering = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        #endregion
    }

    public class PictureToggle : MetroControlBase
    {
        protected Image DefaultImage;
        private Image _Image { get; set; }
        public Image MouseOverImage { get; set; }
        public Image ToggleImage { get; set; }
        public bool IsChecked { get; set; }

        public event EventHandler Toggle;

        protected bool isImageSet;

        public PictureToggle()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        public Image Image
        {
            get
            {
                return _Image;
            }
            set
            {
                _Image = value;
                if (!isImageSet)
                {
                    DefaultImage = _Image;
                    isImageSet = true;
                }
                Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Image != null)
                e.Graphics.DrawImage(Image, 0, 0, Image.Width, Image.Height);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Image = MouseOverImage;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Image = IsChecked ? ToggleImage : DefaultImage;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            Image = IsChecked ? DefaultImage : ToggleImage;
            IsChecked = !IsChecked;
            if (Toggle != null)
            {
                Toggle(this, EventArgs.Empty);
            }
            base.OnMouseClick(e);
        }
    }

    [ToolboxBitmap(typeof(Button))]
    public class RecordButton : ImageButton
    {
        private int _ProgressPercentage;
        public int ProgressPercentage
        {
            get { return _ProgressPercentage; }
            set
            {
                _ProgressPercentage = value;
                Refresh();
            }
        }

        private int _LineWidth;
        public int LineWidth
        {
            get { return _LineWidth; }
            set
            {
                _LineWidth = value;
                Refresh();
            }
        }

        private Color _ProgressColor;
        public Color ProgressColor
        {
            get { return _ProgressColor; }
            set
            {
                _ProgressColor = value;
                Refresh();
            }
        }

        public RecordButton()
        {
            BackColor = Color.Transparent;
            ProgressColor = Color.Red;
            ProgressPercentage = 0;
            LineWidth = 5;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle((Bounds.Width - (int)(0.44 * Bounds.Width)) / 2, (Bounds.Height - (int)(0.44 * Bounds.Height)) / 2, (int)(0.44 * Bounds.Width), (int)(0.44 * Bounds.Height));
            e.Graphics.FillEllipse(new SolidBrush(MetroPaint.GetStyleColor(Style)), rect);
            DrawProgressCircle(e.Graphics, Color.FromKnownColor(KnownColor.ButtonFace), 100);
            DrawProgressCircle(e.Graphics, MetroPaint.GetStyleColor(Style), ProgressPercentage);
        }

        private void DrawProgressCircle(Graphics g, Color color, int percentage)
        {
            Pen pen = new Pen(new SolidBrush(color), LineWidth);
            Rectangle rect = new Rectangle((int)pen.Width / 2, (int)pen.Width / 2, Bounds.Width - (int)pen.Width, Bounds.Height - (int)pen.Width);
            g.DrawArc(pen, rect, 270, (int)(percentage / 100.0 * 360));
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            this.Height = this.Width;
            base.OnSizeChanged(e);
        }
    }

    // Based on http://msdn.microsoft.com/en-us/library/ms172532%28v=vs.90%29.aspx
    // This code shows a simple way to have a  
    // button-like control that has a background image.
    [ToolboxBitmap(typeof(Button))]
    public class PictureButton : Control
    {
        Image backgroundImage, pressedImage;
        bool pressed = false;

        // Property for the background image to be drawn behind the button text. 
        public Image BackgroundImage
        {
            get
            {
                return this.backgroundImage;
            }
            set
            {
                this.backgroundImage = value;
            }
        }

        // Property for the background image to be drawn behind the button text when 
        // the button is pressed. 
        public Image PressedImage
        {
            get
            {
                return this.pressedImage;
            }
            set
            {
                this.pressedImage = value;
            }
        }

        // When the mouse button is pressed, set the "pressed" flag to true  
        // and invalidate the form to cause a repaint.  The .NET Compact Framework  
        // sets the mouse capture automatically. 
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.pressed = true;
            this.Invalidate();
            base.OnMouseDown(e);
        }

        // When the mouse is released, reset the "pressed" flag 
        // and invalidate to redraw the button in the unpressed state. 
        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.pressed = false;
            this.Invalidate();
            base.OnMouseUp(e);
        }

        // Override the OnPaint method to draw the background image and the text. 
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.pressed && this.pressedImage != null)
                e.Graphics.DrawImage(this.pressedImage, 0, 0);
            else
                e.Graphics.DrawImage(this.backgroundImage, 0, 0);

            // Draw the text if there is any. 
            if (this.Text.Length > 0)
            {
                SizeF size = e.Graphics.MeasureString(this.Text, this.Font);

                // Center the text inside the client area of the PictureButton.
                e.Graphics.DrawString(this.Text,
                    this.Font,
                    new SolidBrush(this.ForeColor),
                    (this.ClientSize.Width - size.Width) / 2,
                    (this.ClientSize.Height - size.Height) / 2);
            }

            // Draw a border around the outside of the  
            // control to look like Pocket PC buttons.
            e.Graphics.DrawRectangle(new Pen(Color.Black), 0, 0,
                this.ClientSize.Width - 1, this.ClientSize.Height - 1);

            base.OnPaint(e);
        }
    }
}
