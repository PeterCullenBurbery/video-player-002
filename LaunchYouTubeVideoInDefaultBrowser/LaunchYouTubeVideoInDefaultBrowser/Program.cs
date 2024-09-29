using System;
using System.Diagnostics;

namespace LaunchYouTubeVideoInDefaultBrowser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // URL of the YouTube video
            string url = "https://www.youtube.com/watch?v=EUJu0gF0iJ8";

            try
            {
                // Open the URL in the default browser
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true  // Ensure it opens in the default browser
                });

                Console.WriteLine("The YouTube video is playing in the default browser.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
