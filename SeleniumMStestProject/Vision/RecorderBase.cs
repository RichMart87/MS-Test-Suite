using H.Core;
using H.Core.Recorders;
using H.Core.Storages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumMStestProject.Vision
{
    public abstract class RecorderBase : IRecorder, INotifyPropertyChanged
    {
        public ICollection<AudioSettings> SupportedSettings => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string ShortName => throw new NotImplementedException();

        public string UniqueName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsRegistered { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Description => throw new NotImplementedException();

        public ISettingsStorage Settings => throw new NotImplementedException();

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<IRecording>? Started;

        public event EventHandler<IRecording>? Stopped;

        public event EventHandler<ICommand>? CommandReceived;

        public event AsyncEventHandler<ICommand, IValue>? AsyncCommandReceived;

        public event EventHandler<Exception>? ExceptionOccurred;

        public void StartRecording(string fileName)
        {
            // Implementation for starting the recording
        }

        public void StopRecording()
        {
            // Implementation for stopping the recording
        }

        public void PauseRecording()
        {
            // Implementation for pausing the recording
        }

        public void ResumeRecording()
        {
            // Implementation for resuming the recording
        }

        public void SaveRecording(string fileName)
        {
            // Implementation for saving the recording
        }

        public void DiscardRecording()
        {
            // Implementation for discarding the recording
        }

        public void SetOutputFormat(string format)
        {
            // Implementation for setting the output format
        }

        public string GetOutputFormat()
        {
            // Implementation for getting the output format
            return string.Empty;
        }

        public void SetVideoQuality(int quality)
        {
            // Implementation for setting the video quality
        }

        public int GetVideoQuality()
        {
            // Implementation for getting the video quality
            return 0;
        }

        public void SetAudioQuality(int quality)
        {
            // Implementation for setting the audio quality
        }

        public int GetAudioQuality()
        {
            // Implementation for getting the audio quality
            return 0;
        }

        public void SetRecordingDuration(TimeSpan duration)
        {
            // Implementation for setting the recording duration
        }

        public Task<IRecording> StartAsync(AudioSettings? settings = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ICollection<string> GetAvailableSettings()
        {
            throw new NotImplementedException();
        }

        public void SetSetting(string key, object value)
        {
            throw new NotImplementedException();
        }

        public object? GetSetting(string key)
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        public string[] GetSupportedVariables()
        {
            throw new NotImplementedException();
        }

        public object? GetModuleVariableValue(string name)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}