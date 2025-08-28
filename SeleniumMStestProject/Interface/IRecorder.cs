using H.Core.Recorders;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumMStestProject.Interface
{
    public interface IRecorder
    {
        void StartRecording(string fileName);

        void StopRecording();

        void PauseRecording();

        void ResumeRecording();

        void SaveRecording(string fileName);

        void DiscardRecording();

        void SetOutputFormat(string format);

        string GetOutputFormat();

        void SetVideoQuality(int quality);

        int GetVideoQuality();

        void SetAudioQuality(int quality);

        int GetAudioQuality();

        void SetRecordingDuration(TimeSpan duration);

        event EventHandler<SessionStartEventArgs> RecordingStarted;

        event EventHandler<SessionEndEventArgs> RecordingStopped;

        event EventHandler<EventArgs> RecordingPaused;

        event EventHandler<EventArgs> RecordingResumed;

        event EventHandler<EventArgs> RecordingDiscarded;

        event EventHandler<EventArgs> RecordingSaved;

        event EventHandler<EventArgs> OutputFormatChanged;

        event EventHandler<EventArgs> VideoQualityChanged;

        event EventHandler<EventArgs> AudioQualityChanged;

        event EventHandler<EventArgs> RecordingDurationChanged;

        event EventHandler<EventArgs> RecordingError;

        event EventHandler<EventArgs> RecordingWarning;

        event EventHandler<EventArgs> RecordingInfo;

        event EventHandler<EventArgs> RecordingProgress;

        event EventHandler<EventArgs> RecordingCompleted;

        Recorder State { get; }

        void Pause();

        void Resume();

        void Stop();

        void Start(int Delay = 0);
    }
}