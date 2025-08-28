using SeleniumMStestProject.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumMStestProject.Vision
{
    internal class VideoRecorder
    {
        private bool _recording = false;
        private IDriver _driver;
        private IRecorder _videoRecorder = null;

        public VideoRecorder()
        {
        }

        public VideoRecorder(IDriver driver, bool shouldRecord)
        {
            _driver = driver;

            if (shouldRecord)
            {
                StartVideoCapture();
                _recording = true;
            }
        }

        public TestContext SeleniumTestContext { get; private set; } // Changed type from object to TestContext

        public void StartVideoCapture()
        {
            try
            {
                if (!_recording)
                {
                    var videoGuid = Guid.NewGuid();
                    SeleniumTestContext.VideoGuid = videoGuid.ToString();
                    // Initialize the video recorder
                    //_videoRecorder = new IRecorder(); // Replace with a concrete implementation of IRecorder
                    _videoRecorder.StartRecording(_driver.PageName);
                    _recording = true;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }
    }

    public class TestContext // Added TestContext class definition
    {
        public string VideoGuid { get; set; }
    }
}