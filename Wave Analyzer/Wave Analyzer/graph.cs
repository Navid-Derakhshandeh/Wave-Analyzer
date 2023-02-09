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
    public partial class graph : Form
    {
        public graph()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Wave File (*.wav)|*.wav;";
            if (open.ShowDialog() != DialogResult.OK) return;

            chart1.Series.Add("Wave");
            chart1.Series["Wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart1.Series["Wave"].ChartArea = "ChartArea1";
            NAudio.Wave.WaveChannel32 wave = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(open.FileName));

            byte[] Buffer = new byte[20000];
            int Read = 0;
            while(wave.Position < wave.Length)
            {
                Read = wave.Read(Buffer, 0, 20000);
                

                for(int i = 0; i < Read /4; i++)
                {
                    chart1.Series["Wave"].Points.Add(BitConverter.ToSingle(Buffer, i * 4));

                }
            }
        }
    }
}
