using Accord.Video.FFMPEG;
using GleamTech.VideoUltimate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GroupDocs.Watermark;
using GroupDocs.Watermark.Watermarks;
using Color = GroupDocs.Watermark.Watermarks.Color;
using Font = GroupDocs.Watermark.Watermarks.Font;
using FontStyle = GroupDocs.Watermark.Watermarks.FontStyle;

namespace video_editor
{
    public class VideoEditor
    {
        public int currentFrame { get; set; }
        public List<Bitmap> frames { get; set; }
        public Operatrion operatrion { get; set; }
        private string systemPath = @"D:\JEDB projects\Video editor (multimedia 2.0)\snapshot 3.0\";

        public VideoEditor()
        {
            frames = new List<Bitmap>();
            currentFrame = 0;
        }

        public void Cut(int start, int end)
        {
            frames.RemoveRange(0, start - 1);
            frames.RemoveRange(end, frames.Count - end - 1);
        }

        public void Delete(int start, int end)
        {
            frames.RemoveRange(start, end - start);
        }

        public void GetFrames(string filePath, int stepSize)
        {
            var videoFrameReader = new VideoFileReader();
            videoFrameReader.Open(filePath);
            
            for(int i = 0; i < videoFrameReader.FrameCount; i += stepSize)
            {
                if (i >= videoFrameReader.FrameCount) break;
                frames.Add(videoFrameReader.ReadVideoFrame(i)); 
            }

            videoFrameReader.Close();
            //var videoFrameReader = new VideoFrameReader(filePath);

            //while (videoFrameReader.GetEnumerator().MoveNext())
            //{
            //    frames.Add(videoFrameReader.GetFrame());
            //    for (int i = 0; i < stepSize; ++i)
            //    {
            //        videoFrameReader.GetEnumerator().MoveNext();
            //    }
            //}
        }

        public void SaveFrames(string filePath, int stepSize)
        {
            var videoFrameReader = new VideoFrameReader(filePath);

            int frameIndex = 0;

            while (videoFrameReader.GetEnumerator().MoveNext())
            {
                videoFrameReader.GetFrame().Save(string.Format(@"D:\JEDB projects\Video editor (multimedia 2.0)\snapshot 3.0\frames\Frame{0}.bmp", frameIndex), ImageFormat.Bmp);
                for (int i = 0; i < stepSize; ++i)
                {
                    videoFrameReader.GetEnumerator().MoveNext();
                }
                ++frameIndex;
            }
        }

        public void SaveVideo(string name, int height = -1, int width = -1, int frameRate = 30)
        {
            if (width == -1 || width > frames[0].Width) width = frames[0].Width;
            if (height == -1 || height > frames[0].Height) height = frames[0].Height;

            VideoFileWriter writer = new VideoFileWriter();
            writer.Open(name + ".avi", width, height, frameRate, VideoCodec.MPEG4, 1000000);
            for (int i = 0; i < frames.Count; ++i)
            {
                writer.WriteVideoFrame(new Bitmap(frames[i], new Size(width, height)));
            }
            writer.Close();

            File.Move(systemPath + @"video_editor\video_editor\bin\Debug\" + name + ".avi", systemPath + @"output\" + name + ".avi");
        }

        public void MoveSection(int start, int end, int distination)
        {
            int range = end - start;
            List<Bitmap> newFrames = frames.GetRange(start, range);
            frames.RemoveRange(start, range);
            frames.InsertRange(distination - range, newFrames);
        }

        public void AddWatermark()
        {

        }
    }
}
