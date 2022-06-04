using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Video.FFMPEG;

namespace video_editor
{
    public partial class Form1 : Form
    {
        private string systemPath;
        private VideoEditor editor;
        private int startTime;
        private int endTime;    

        public Form1()
        {
            InitializeComponent();
            editor = new VideoEditor();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            systemPath = @"D:\JEDB projects\Video editor (multimedia 2.0)\snapshot 3.0\";
            BackgroundImage = Image.FromFile(systemPath + "background.jpg");
        }

        private void InitFrameSlider()
        { 
            pictureBox1.Image = editor.frames[0];
            trackBar1.Maximum = editor.frames.Count - 1;
            editor.currentFrame = 0;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Debug.Write(openFileDialog1.FileName);
                editor.SaveFrames(openFileDialog1.FileName, 2);
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Debug.Write(openFileDialog1.FileName);
                editor.GetFrames(openFileDialog1.FileName, 0);
                InitFrameSlider();
            }
        }

        private void rightButton_Click(object sender, EventArgs e)
        {
            if (editor.currentFrame < editor.frames.Count)
            {
                ++editor.currentFrame;
                pictureBox1.Image = editor.frames[editor.currentFrame];
            }
        }

        private void leftButton_Click(object sender, EventArgs e)
        {
            if (editor.currentFrame > 0)
            {
                --editor.currentFrame;
                pictureBox1.Image = editor.frames[editor.currentFrame];
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (editor.currentFrame < editor.frames.Count)
            {
                editor.currentFrame = trackBar1.Value;
                pictureBox1.Image = editor.frames[editor.currentFrame];
            }
        }

        private void cutButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = true;
            editor.operatrion = Operatrion.Cut;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = true;
            editor.operatrion = Operatrion.Delete;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            endButton.Enabled = true;
            startButton.Enabled = false;
            startTime = trackBar1.Value;
        }

        private void endButton_Click(object sender, EventArgs e)
        {
            endButton.Visible = false;
            endTime = trackBar1.Value;
            if(editor.operatrion == Operatrion.Cut)
            {
                editor.Cut(startTime, endTime); 
            }
            else if(editor.operatrion == Operatrion.Delete)
            {
                editor.Delete(startTime, endTime);
            }
            trackBar1.Value = 0;
            InitFrameSlider();
        }

        private void saveButton_Click_1(object sender, EventArgs e)
        {
            editor.CreateVideo("new");
        }
    }
}
