using iTextSharp.text.html.simpleparser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumMStestProject.Vision
{
    internal class Recorder : RecorderBase
    {
        private readonly IImageProvider imageProvider;

        private readonly ManualResetEvent _stopCaptureEvent = new ManualResetEvent(false);

        private readonly IVideoFileWriter _videoEncoder;

        public Recorder(IImageProvider imageProvider)
        {
            this.imageProvider = imageProvider;
        }

        private readonly AutoResetEvent _videoFrameWritten = new AutoResetEvent(false),
             _audioFrameWritten = new AutoResetEvent(false),
             _videoBlockWritten = new AutoResetEvent(false);

        private Thread _recordThread;

        public Recorder(IVideoFileWriter writer, IImageProvider imageProvider, int frameRate)
        {
            _videoEncoder = writer;
            this.imageProvider = imageProvider;
            _videoEncoder.FrameRate = frameRate;
        }
    }
}