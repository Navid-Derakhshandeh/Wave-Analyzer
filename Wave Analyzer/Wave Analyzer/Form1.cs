using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.CoreAudioApi;

namespace Wave_Analyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Wave File (*.wav)|*.wav;";
            if (open.ShowDialog() != DialogResult.OK) return;




            NAudio.Wave.WaveChannel32 wave4 = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(open.FileName));

            NAudio.Wave.Wave16ToFloatProvider wave2 = new NAudio.Wave.Wave16ToFloatProvider(new NAudio.Wave.WaveFileReader(open.FileName));

            double a = wave2.Volume;
            textBox6.Text = a.ToString();

            NAudio.Wave.WaveFileReader wave3 = new NAudio.Wave.WaveFileReader(open.FileName);

            double b = wave3.SampleCount;
            textBox1.Text = b.ToString();

            double c = wave3.Length;
            textBox2.Text = c.ToString();

            float d = wave3.Position;
            textBox5.Text = d.ToString();

            bool u = wave3.CanRead;

            textBox3.Text = u.ToString();

            bool r = wave3.CanTimeout;
            textBox4.Text = r.ToString();




            moveWaveViewer1.BackColor = Color.White;
            moveWaveViewer1.SamplesPerPixel = 400;
            moveWaveViewer1.StartPosition = 40000;
            moveWaveViewer1.WaveStream = new NAudio.Wave.WaveFileReader(open.FileName);

            moveWaveViewer1.FitScreen();

        }

        private void chartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph frm = new graph();
            frm.Show();
        }
    }
}
