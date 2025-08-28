using SeleniumMStestProject.Interface;

namespace SeleniumMStestProject.Extensions
{
    public static class RecorderExtensions
    {
        public static void StartRecording(this IRecorder recorder)
        {
            // Implementation for starting the recording
            Console.WriteLine($"{recorder.Name} started recording.");
        }

        public static void StopRecording(this IRecorder recorder)
        {
            // Implementation for stopping the recording
            Console.WriteLine($"{recorder.Name} stopped recording.");
        }

        public static void PauseRecording(this IRecorder recorder)
        {
            // Implementation for pausing the recording
            Console.WriteLine($"{recorder.Name} paused recording.");
        }

        public static void ResumeRecording(this IRecorder recorder)
        {
            // Implementation for resuming the recording
            Console.WriteLine($"{recorder.Name} resumed recording.");
        }
    }
}