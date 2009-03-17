using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System;
using System.Runtime.InteropServices;

namespace FreeMeterRevival.Controls
{
    ///============================================================================================
    /// <summary>
    /// Font size enumeration
    /// </summary>
    ///============================================================================================
    public enum FontSize
    {
        Small,
        Normal,
        Large
    }
    ///============================================================================================
    /// <summary>
    /// This control will show the Networking traffic of a computer / Network card
    /// </summary>
    ///============================================================================================
    public partial class NetworkGraph : PictureBox
    {


        #region Imports
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        static extern bool DeleteObject(IntPtr oBm);
        #endregion

        #region Variables
        private Color m_downloadColor = Color.Lime;
        private Color m_uploadColor = Color.Red;
        private Color m_overlapColor = Color.Yellow;

        private bool m_uploadActive = true;
        private bool m_downloadActive = true;
        private bool m_showSummary = false;
        private bool m_showTitle = false;

        private FontSize m_fontSize = FontSize.Normal;
        #endregion

        #region Properties


        public Color DownloadColor
        {
            get { return m_downloadColor; }
            set { m_downloadColor = value; }
        }
        public Color UploadColor
        {
            get { return m_uploadColor; }
            set { m_uploadColor = value; }
        }
        public Color OverlapColor
        {
            get { return m_overlapColor; }
            set { m_overlapColor = value; }
        }

        [DefaultValue(true)]
        public bool UploadActive
        {
            get { return m_uploadActive; }
            set { m_uploadActive = value; }
        }
        [DefaultValue(true)]
        public bool DownloadActive
        {
            get { return m_downloadActive; }
            set { m_downloadActive = value; }
        }
        [DefaultValue(true)]
        public bool ShowSummary
        {
            get { return m_showSummary; }
            set { m_showSummary = value; }
        }
        [DefaultValue(true)]
        public bool ShowTitle
        {
            get { return m_showTitle; }
            set { m_showTitle = value; }
        }
        [DefaultValue(FontSize.Normal)]
        public FontSize TitleSize
        {
            get { return m_fontSize; }
            set { m_fontSize = value; }
        }
        #endregion

        public NetworkGraph()
        {
            InitializeComponent();
        }

        public NetworkGraph(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Update the current graph with the new values.
        /// </summary>
        /// <returns>True, if the update was successful, otherwise false</returns>
        ///----------------------------------------------------------------------------------------
        public bool UpdateGraph(int[] full_downlines, int[] full_uplines)
        {



            try
            {
                Bitmap bm = new Bitmap(this.Width, this.Height, PixelFormat.Format16bppRgb555);
                Graphics g = Graphics.FromImage((Image)bm);

                g.FillRegion(new LinearGradientBrush(
                    new PointF(bm.Width / 2, 0), new PointF(bm.Width / 2, bm.Height)
                    , Color.Gray, Color.Black)
                    , new Region(new Rectangle(0, 0, bm.Width, bm.Height)));

                //draw each line in the graph
                DrawGraph(g, full_downlines, full_uplines, false);

                IntPtr oBm = bm.GetHbitmap();
                this.Image = Image.FromHbitmap(oBm);


                DeleteObject(oBm);
                bm.Dispose();
                g.Dispose();

            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Draw the Upload and Download graph
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="dlines"></param>
        /// <param name="ulines"></param>
        /// <param name="drawingIcon"></param>
        ///----------------------------------------------------------------------------------------
        private void DrawGraph(Graphics graph, int[] dlines, int[] ulines, bool drawingIcon)
        {
            for (int i = 0; i < dlines.Length; i++)
            {
                if (dlines[i] > 0 || ulines[i] > 0)
                {
                    if (dlines[i] > ulines[i])
                    {
                        if (m_downloadActive && m_uploadActive)
                        {
                            graph.DrawLine(new Pen(m_downloadColor), i, Height, i, Height - dlines[i]);
                            graph.DrawLine(new Pen(m_overlapColor), i, Height, i, Height - ulines[i]);
                        }
                        else if (m_downloadActive && !m_uploadActive)
                            graph.DrawLine(new Pen(m_downloadColor), i, Height, i, Height - dlines[i]);
                        else if (!m_downloadActive && m_uploadActive)
                            graph.DrawLine(new Pen(m_uploadColor), i, Height, i, Height - ulines[i]);
                    }
                    else if (dlines[i] < ulines[i])
                    {
                        if (m_downloadActive && m_uploadActive)
                        {
                            graph.DrawLine(new Pen(m_uploadColor), i, Height, i, Height - ulines[i]);
                            graph.DrawLine(new Pen(m_overlapColor), i, Height, i, Height - dlines[i]);
                        }
                        else if (!m_downloadActive && m_uploadActive)
                            graph.DrawLine(new Pen(m_uploadColor), i, Height, i, Height - ulines[i]);
                        else if (m_downloadActive && !m_uploadActive)
                            graph.DrawLine(new Pen(m_downloadColor), i, Height, i, Height - dlines[i]);
                    }
                    else if (dlines[i] == ulines[i])
                    {
                        if (m_uploadActive && m_downloadActive)
                            graph.DrawLine(new Pen(m_overlapColor), i, Height, i, Height - ulines[i]);
                        else if (!m_uploadActive && m_downloadActive)
                            graph.DrawLine(new Pen(m_downloadColor), i, Height, i, Height - dlines[i]);
                        else if (m_uploadActive && !m_downloadActive)
                            graph.DrawLine(new Pen(m_uploadColor), i, Height, i, Height - ulines[i]);
                    }
                }
            }


            int down = dlines[dlines.Length - 1];
            int up = ulines[ulines.Length - 1];

            if (m_downloadActive && m_showSummary)
            {
                graph.DrawLine(Pens.Black, dlines.Length - 2, 0, dlines.Length - 2, Height);
                graph.DrawLine(Pens.Black, dlines.Length - 1, 0, dlines.Length - 1, Height - down);
                graph.DrawLine(Pens.White, dlines.Length - 1, Height, dlines.Length - 1, Height - down);
            }

            if (m_uploadActive && m_showSummary)
            {
                graph.DrawLine(Pens.Black, 1, 0, 1, Height);
                graph.DrawLine(Pens.Black, 0, 0, 0, Height - up);
                graph.DrawLine(Pens.White, 0, Height, 0, Height - up);
            }


            if (m_showTitle && !drawingIcon)
            {
                Font f = null;
                string fontName = "Verdana";
                int fontSize = 6;

                switch (m_fontSize)
                {
                    case FontSize.Small:
                         f = new Font(fontName, fontSize, FontStyle.Regular);
                        break;
                    case FontSize.Normal:
                         f = new Font(fontName, fontSize + 1, FontStyle.Regular);
                        break;
                    case FontSize.Large:
                        f = new Font(fontName, fontSize + 2, FontStyle.Regular);
                        break;
                }


                string text = "TEST";// display_xscale + " " + display_yscale;
                SizeF size = graph.MeasureString(text, f);
                RectangleF rect = new RectangleF(new PointF(2, 2), size);

                //graph.FillRectangle(new SolidBrush(Color.Black), rect);
                graph.DrawString(text, f, new SolidBrush(Properties.Settings.Default.ForegroundColor), rect);
            }
        }



    }
}

