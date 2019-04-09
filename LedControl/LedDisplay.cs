using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Xml;



namespace LedControl
{
    public class LedDisplay : Control, ISupportInitialize

    {
        // increment scroll offset by pixel or segment
        public enum Step
        {
            Pixel = 0,
            Segment = 1
        }

        // matrix size, must not exceed x*y = 64 bits = ulong
        const float HSEGMENTS = 6.0f;       // number of horizontal segments
        const float VSEGMENTS = 8.0f;       // number vertical segments

        #region Local variables

        private Dictionary<char, ulong> segDict = new Dictionary<char, ulong> { };

        private Bitmap displayBitmap;       // keeps a bitmap for the string to display
        private float segmentSpace;         // space between segments
        private float charSpace;            // space between characters
        private bool scroll;                // true or false
        private int scrollDelay;            // timer ticks between scroll movement
        private int scrollOffset;           // horizontal offset when scrolling
        private Step scrollStep;            // step when scrolling
        private string xmlFile;             // character defintion file

        private Timer scrollTimer = new Timer();

        #endregion

        #region Public Properties

        [Browsable(false)]
        public float HorizontalSegments // number
        {
            get { return HSEGMENTS; }
            set { }
        }

        [Browsable(false)]
        public float VerticalSegments
        {
            get { return VSEGMENTS; }
            set { }
        }

        [DefaultValue(20), Description("Space in % between segments"), Category("Apperance")]
        public int SegmentSpace
        {
            get
            {
                return (int) (segmentSpace * 100.0f);
            }
            set
            {
                if (segmentSpace == value)
                    return;
                segmentSpace = Math.Abs(value / 100.0f);
                if (!isInitializing)
                {
                    Invalidate();
                }
            }
        }

        // space between characters could be down to zero
        [DefaultValue(20), Description("Space in % between characters"), Category("Apperance")]
        public int CharSpace
        {
            get
            {
                return (int) (charSpace * 100.0f);
            }
            set
            {
                if (charSpace == value)
                    return;
                charSpace = Math.Abs(value / 100.0f);
                if (!isInitializing)
                {
                    Invalidate();
                }
            }
        }

        [DefaultValue(false), Category("Appearance"), Description("Scroll text")]
        public bool Scroll
        {
            get { return scroll; }
            set 
            { 
                scroll = value;
                if (scroll == true && !DesignMode)  // don't start timer in design mode
                    scrollTimer.Start();
                else
                    scrollTimer.Stop();
            }
        }

        // windows timers accuracy is depending on the computer
        // a medium computer has average minimum of 15-20 milliseconds between ticks
        [DefaultValue(25), Category("Appearance"), Description("Scroll delay in milliseconds.")]
        public int ScrollDelay
        {
            get { return scrollDelay; }
            set 
            { 
                scrollDelay = value < 5 ? 5 : value;    // minimum scroll delay 
                scrollTimer.Interval = scrollDelay;
            }
        }

        [DefaultValue(typeof(Step), "Pixel"), Category("Appearance"), Description("Scroll step, pixel or segment")]
        public Step ScrollStep
        {
            get
            {
                return scrollStep;
            }
            set
            {
                scrollStep = value;
                if (!isInitializing)
                {
                    Invalidate();
                }
            }
        }

        [Browsable(true), Description("Text shown in control"), Category("Appearance")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                if (!isInitializing)
                {
                    Invalidate();
                }
            }
        }

        [Browsable(true), Description("BackColor"), Category("Appearance")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                if (!isInitializing)
                {
                    Invalidate();
                }
            }
        }

        // Does not support background image, use transparency and paint the background
        [Browsable(false)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = null;
            }
        }

        // and not background image layout
        [Browsable(false)]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = ImageLayout.None;
            }
        }

        #endregion

        #region Initialize

        public LedDisplay()
        {
            this.SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.DoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor, true);

            InitializeComponent();

        }

        private void InitializeComponent()
        {
            displayBitmap = null;
            this.ForeColor = Color.LightGreen;
            this.BackColor = Color.Black;
            segmentSpace = 0.2f;
            charSpace = 0.2f;
            scroll = false;
            scrollOffset = 0;
            scrollDelay = 25;
            scrollStep = Step.Pixel;
            scrollTimer.Interval = scrollDelay;
            scrollTimer.Tick += new System.EventHandler(this.scrollTimer_Tick);
            scrollTimer.Enabled = false;

            xmlFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LedFont.xml";
            // this file is needed in design mode as well, thats why it's stored in 
            // the documents folder
            // application.startuppath is not available in design mode
            // if the file not exists, just create an empty file
            if (!File.Exists(xmlFile))
                WriteCharDef();

            ReadCharDef();
        }
        #endregion

        #region Overrides and Events

        protected override void Dispose(bool disposing)
        {   // maybe should dispose some local variables too
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (displayBitmap == null)
            {
                int width = Convert.ToInt32(base.Height / VSEGMENTS * HSEGMENTS);
                int bitmapWidth = (int) ((width + width * charSpace) * base.Text.Length);

                // the displayBitmap has at least the same width as the control, else depending of string lenght
                bitmapWidth = Math.Max(bitmapWidth, base.Width);
                displayBitmap = new Bitmap(bitmapWidth, base.Height);
            }

            // paint the bitmap
            using (Graphics g = Graphics.FromImage(displayBitmap))
            {
                DrawBackground(g, new Rectangle(0, 0, displayBitmap.Width, displayBitmap.Height));
                DrawString(base.Text, g, new Rectangle(0, 0, displayBitmap.Width, displayBitmap.Height));
            }

            if (!scroll) // if not scrolling just copy displayBitmap to control
            {
                Rectangle srcRect = new Rectangle(0, 0, this.Width, this.Height);
                e.Graphics.DrawImage(displayBitmap, 0, 0, srcRect, GraphicsUnit.Pixel);
            }
            else // scroll
            {
                // if we not reached the end of displayBitmap, just copy a part of it
                if ((displayBitmap.Width - scrollOffset) >= this.Width)
                {
                    Rectangle srcRect = new Rectangle(scrollOffset, 0, this.Width, this.Height);
                    e.Graphics.DrawImage(displayBitmap, 0, 0, srcRect, GraphicsUnit.Pixel);
                }
                // here we need a part from the end of displayBitmap + a part from the beginning
                else
                {
                    using (Bitmap bmpCopy = new Bitmap(this.Width, this.Height))
                    {
                        using (Graphics g = Graphics.FromImage(bmpCopy))
                        {
                            // get the end of displayBitmap and draw it at the beginning of bmpCopy
                            Rectangle srcRectEnd = new Rectangle(scrollOffset, 0, displayBitmap.Width - scrollOffset, this.Height);
                            g.DrawImage(displayBitmap, 0, 0, srcRectEnd, GraphicsUnit.Pixel);

                            // get the beginning of displayBitmap and draw it at the end of bmpCopy
                            Rectangle srcRectStart = new Rectangle(0, 0, this.Width - srcRectEnd.Width, this.Height);
                            g.DrawImage(displayBitmap, srcRectEnd.Width, 0, srcRectStart, GraphicsUnit.Pixel);

                            // draw the resulting bitmap to control
                            e.Graphics.DrawImage(bmpCopy, 0, 0);
                        }
                    }
                }
            }
        }

        // create a new bitmap on text changed,
        // else you get an error in design mode
        protected override void OnTextChanged(EventArgs e)
        {
            if (displayBitmap != null)
            {
                displayBitmap.Dispose();
                displayBitmap = null;
            }

            base.OnTextChanged(e);
        }

        // create a new bitmap on control resize,
        // else you get an error in design mode
        protected override void OnResize(EventArgs e)
        {
            if (displayBitmap != null)
            {
                displayBitmap.Dispose();
                displayBitmap = null;
            }

            base.OnResize(e);
        }

        // update backcolor in design mode
        protected override void OnBackColorChanged (EventArgs e)
        {
            if (displayBitmap != null)
            {
                displayBitmap.Dispose();
                displayBitmap = null;
            }

            base.OnBackColorChanged(e);
        }

        // timer ticker, increases scrollOffset until the end of displayBitmap is reached
        private void scrollTimer_Tick(object sender, EventArgs e)
        {
            //scrollOffset += this.Height / (int) VSEGMENTS; // one segment per tick

            //scrollOffset++;                 // one pixel per tick

            if (scrollStep == Step.Pixel)
                scrollOffset++;
            else
                scrollOffset += (int) (this.Height / VSEGMENTS);
            
            if (scrollOffset > displayBitmap.Width)
                scrollOffset = 0;

            this.Invalidate();
        }

        #endregion

        #region Public methods

        // check if char exists in dictionary
        public bool FindChar(char chr)
        {
            if (segDict.ContainsKey(chr))
                return true;
            else
                return false;
        }
       
        // add a char to dictionary, remove old one if it exists
        public void AddChar(char chr, ulong bits)
        {
            if (segDict.ContainsKey(chr))
                segDict.Remove(chr);
            segDict.Add(chr, bits);
        }

        // remove char if it exists in dictionary
        public void RemoveChar(char chr)
        {
            if (segDict.ContainsKey(chr))
                segDict.Remove(chr);
        }

        // get value for char
        public ulong GetValue(char chr)
        {
            ulong bits;
            segDict.TryGetValue(chr, out bits);
            return bits;
        }

        // get a list of characters in dictionary
        public ArrayList GetKeys()
        {
            ArrayList keyNameList = new ArrayList(segDict.Keys);
            return keyNameList;
        }

        #region Draw characters, strings and background

        // draws a single char to the graphics object within specified rectangle
        // can be used with any graphical object
        public void DrawChar(ulong bits, Graphics g, Rectangle rect)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float size = rect.Height / VSEGMENTS;       // height and width makes a square
            float space = (size * segmentSpace) / 2;    // space between segments

            using (SolidBrush segmentBrush = new SolidBrush(ForeColor))
            {
                for (int y = 0; y < VSEGMENTS; y++)         // vertical row
                {
                    for (int x = 0; x < HSEGMENTS; x++)     // horizontal row
                    {
                        if ((bits & 1) != 0)                // if bit 1 is set, fill segment
                        {
                            RectangleF segmentRect = new RectangleF(rect.X + x * size, y * size, size, size);
                            if ((space * 2) < size)         // don't inflate to zero
                                segmentRect.Inflate(-space, -space);
                            g.FillEllipse(segmentBrush, segmentRect);
                        }
                        bits = bits >> 1;                   // roll bits to the right
                    }
                }
            }
        }

        // draws a string to the graphics object within specified rectangle
        // can be used with any graphical object
        public void DrawString(string str, Graphics g, Rectangle rect)
        {
            ulong bits;

            if (str != null && str.Length > 0)
            {
                int width = Convert.ToInt32(this.Height / VSEGMENTS * HSEGMENTS);
                int pos = (int) (width + width * charSpace); // position for next character

                for (int i = 0; i < str.Length; i++)
                {
                    segDict.TryGetValue(str[i], out bits);
                    rect = new Rectangle(pos * i, 0, width, rect.Height);
                    DrawChar(bits, g, rect);
                }
            }
        }

        // paints a graphics object within specified rectangle
        // could be gradient, but that doesn't add much
        private void DrawBackground(Graphics g, Rectangle rect)
        {
            using (SolidBrush brush = new SolidBrush(base.BackColor))
                g.FillRectangle(brush, rect);
        }

        #endregion draw
        #endregion public methods

        #region Xml Read/Write

        // read character definitions
        // currently the file must exist i your documents directory
        internal void ReadCharDef()
        { 
            XmlTextReader xmlReader = null;

            try
            {
                xmlReader = new XmlTextReader(xmlFile);

                string chrs = "";
                ulong bits = 0;

                while (xmlReader.Read())
                {
                    // read matrix layout, currently not used
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.CompareTo("Layout") == 0)
                    {
                        int vertical = Int32.Parse(xmlReader.GetAttribute("Vertical").ToString());
                        int horizontal = Int32.Parse(xmlReader.GetAttribute("Horizontal").ToString());
                    }
                    // read character definitions
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.CompareTo("character") == 0)
                    {
                        chrs = xmlReader.GetAttribute("char").ToString();
                        bits = UInt64.Parse(xmlReader.GetAttribute("bits").ToString());
                        if (!FindChar(chrs[0]))
                            segDict.Add(chrs[0], bits);
                    }
                }
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
            }
        }

        // write character definitions
        // currently the file is written to your documents directory
        public  void WriteCharDef()
        {
            XmlTextWriter xmlWriter = null;

            try
            {
                xmlWriter = new XmlTextWriter(xmlFile, Encoding.UTF8);
                xmlWriter.Formatting = Formatting.Indented;

                xmlWriter.WriteStartDocument(false);
                xmlWriter.WriteComment("Led character definintions");

                // write matrix layout
                xmlWriter.WriteStartElement("Layout");
                xmlWriter.WriteAttributeString("Horizontal", HSEGMENTS.ToString());
                xmlWriter.WriteAttributeString("Vertical", VSEGMENTS.ToString());

                xmlWriter.WriteStartElement("CharacterTable");

                foreach (KeyValuePair<char, ulong> tmp in segDict)
                {
                    xmlWriter.WriteStartElement("character");
                    xmlWriter.WriteAttributeString("char", tmp.Key.ToString());
                    xmlWriter.WriteAttributeString("bits", tmp.Value.ToString());
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }
            catch (IOException)
            {
                throw;
            }
        }
        #endregion

        #region ISupportInitialize

        private bool isInitializing = false;

        void ISupportInitialize.BeginInit()
        {
            isInitializing = true;
        }

        void ISupportInitialize.EndInit()
        {
            isInitializing = false;
            Invalidate();
        }

        #endregion
    }
}
