using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using NAudio.Wave;

namespace Wave_Analyzer
{
    /// <summary>
    /// Control for viewing waveforms
    /// </summary>
    public class MoveWaveViewer : System.Windows.Forms.UserControl
    {
        public Color Pencolor { get; set; }
        public float Penwidth { get; set; }
        public void FitScreen()
        {
            if (waveStream == null) return;
            int samples = (int)(waveStream.Length / bytesPerSample);
            StartPosition = 0;
            SamplesPerPixel = samples / this.Width;

        }

        public void Zoom(int leftSample, int rightSample)
        {
            StartPosition = leftSample * bytesPerSample;
            SamplesPerPixel = (rightSample - leftSample) / this.Width;

        }

        private Point MousePos, startPos;
        private bool MouseDrag = false;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                startPos = e.Location;
                MousePos = new Point(-1, -1);
                MouseDrag = true;
                DrawVertical(e.X);
            }
            base.OnMouseDown(e);

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (MouseDrag)
            {
                DrawVertical(e.X);
                if (MousePos.X != -1) DrawVertical(MousePos.X);
                MousePos = e.Location;

            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (MouseDrag && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDrag = false;
                DrawVertical(startPos.X);

                if (MousePos.X == -1) return;
                DrawVertical(MousePos.X);

                int leftSample = (int)(StartPosition / bytesPerSample + SamplesPerPixel * Math.Min(startPos.X, MousePos.X));
                int rightSample = (int)(StartPosition / bytesPerSample + samplesPerPixel * Math.Max(startPos.X, MousePos.X));
                Zoom(leftSample, rightSample);

            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle) FitScreen();
            base.OnMouseUp(e);
        }

        private void DrawVertical(int x)
        {
            ControlPaint.DrawReversibleLine(PointToScreen(new Point(x, 0)), PointToScreen(new Point(x, Height)), Color.Black);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            FitScreen();
        }
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private WaveStream waveStream;
        private int samplesPerPixel = 128;
        private long startPosition;
        private int bytesPerSample;
        /// <summary>
        /// Creates a new WaveViewer control
        /// </summary>
        public MoveWaveViewer()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Pencolor = Color.DodgerBlue;
            this.Penwidth = 1;
        }

        /// <summary>
        /// sets the associated wavestream
        /// </summary>
        public WaveStream WaveStream
        {
            get
            {
                return waveStream;
            }
            set
            {
                waveStream = value;
                if (waveStream != null)
                {
                    bytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8) * waveStream.WaveFormat.Channels;
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// The zoom level, in samples per pixel
        /// </summary>
        public int SamplesPerPixel
        {
            get
            {
                return samplesPerPixel;
            }
            set
            {
                samplesPerPixel = Math.Max(1, value);
                this.Invalidate();
            }
        }

        /// <summary>
        /// Start position (currently in bytes)
        /// </summary>
        public long StartPosition
        {
            get
            {
                return startPosition;
            }
            set
            {
                startPosition = value;
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// <see cref="Control.OnPaint"/>
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (waveStream != null)
            {
                waveStream.Position = 0;
                int bytesRead;
                byte[] waveData = new byte[samplesPerPixel * bytesPerSample];
                waveStream.Position = startPosition + (e.ClipRectangle.Left * bytesPerSample * samplesPerPixel);

                using (Pen linePen = new Pen(Pencolor, Penwidth))
                {


                    for (float x = e.ClipRectangle.X; x < e.ClipRectangle.Right; x += 1)
                    {
                        short low = 0;
                        short high = 0;
                        bytesRead = waveStream.Read(waveData, 0, samplesPerPixel * bytesPerSample);
                        if (bytesRead == 0)
                            break;
                        for (int n = 0; n < bytesRead; n += 2)
                        {
                            short sample = BitConverter.ToInt16(waveData, n);
                            if (sample < low) low = sample;
                            if (sample > high) high = sample;
                        }
                        float lowPercent = ((((float)low) - short.MinValue) / ushort.MaxValue);
                        float highPercent = ((((float)high) - short.MinValue) / ushort.MaxValue);
                        e.Graphics.DrawLine(linePen, x, this.Height * lowPercent, x, this.Height * highPercent);
                    }
                }
            }

            base.OnPaint(e);
        }


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    }
}
