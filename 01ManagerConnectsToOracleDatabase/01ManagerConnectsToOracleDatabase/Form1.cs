using LibVLCSharp.Shared; // For VLC media player
using Oracle.ManagedDataAccess.Client; // For Oracle database connection
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace _01ManagerConnectsToOracleDatabase
{
    public partial class Form1 : Form
    {
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private MemoryStream videoStream; // Memory stream for BLOB video data

        // Oracle connection string with Kingdom_Rush database properties
        private const string connectionString = "User Id=Kingdom_Rush;Password=1234;Data Source=localhost:1521/Kingdom_Rush.localdomain";

        public Form1()
        {
            InitializeComponent();

            // Initialize VLC libraries
            Core.Initialize();

            // Create a new LibVLC instance
            _libVLC = new LibVLC();

            // Create a new media player using the VLC instance and assign the video view
            _mediaPlayer = new MediaPlayer(_libVLC);
            _mediaPlayer.Hwnd = videoView1.Handle;

            // Initialize the videoStream
            videoStream = new MemoryStream();

            // Automatically load the BLOB from the database and prepare for playback
            LoadBlobFromDatabase();
        }

        // Play button click event handler (stream directly from MemoryStream)
        private void playButton_Click(object sender, EventArgs e)
        {
            if (videoStream != null && videoStream.Length > 0)
            {
                // Reset the MemoryStream position to the beginning
                videoStream.Seek(0, SeekOrigin.Begin);

                // Use StreamMediaInput to play the stream directly
                using (var mediaInput = new StreamMediaInput(videoStream))
                {
                    var media = new Media(_libVLC, mediaInput);
                    _mediaPlayer.Media = media;
                }

                // Try playing the video
                if (!_mediaPlayer.Play())
                {
                    MessageBox.Show("Failed to start video playback.");
                }
            }
            else
            {
                MessageBox.Show("Video stream is empty. Ensure the video has been loaded correctly.");
            }
        }

        // Pause button click event handler
        private void pauseButton_Click(object sender, EventArgs e)
        {
            // Pause the video
            if (_mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Pause();
            }
        }

        // Method to connect to Oracle, retrieve the BLOB, and store it in MemoryStream
        private void LoadBlobFromDatabase()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    // Open connection
                    conn.Open();

                    // SQL query to fetch the BLOB from the database
                    OracleCommand cmd = conn.CreateCommand();
                    cmd.CommandText = @"
                    SELECT VIDEO 
                    FROM beat_the_level bl
                    JOIN kingdom_rush_level_stage ks ON bl.KINGDOM_RUSH_LEVEL_STAGE_ID = ks.KINGDOM_RUSH_LEVEL_STAGE_ID
                    JOIN play_mode pm ON bl.PLAY_MODE_ID = pm.PLAY_MODE_ID
                    JOIN game_difficulty gd ON bl.GAME_DIFFICULTY_ID = gd.GAME_DIFFICULTY_ID
                    JOIN player p ON bl.PLAYER_ID = p.PLAYER_ID
                    WHERE REGEXP_LIKE(ks.kingdom_rush_level_stage, 'small test', 'i') 
                    AND REGEXP_LIKE(gd.game_difficulty, 'impossible', 'i') 
                    AND REGEXP_LIKE(pm.play_mode, 'campaign', 'i') 
                    AND REGEXP_LIKE(p.username, 'best of the best cream of the crop crème de la crème', 'i')";

                    using (OracleDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
                    {
                        if (reader.Read())
                        {
                            // Read the BLOB column (VIDEO)
                            OracleBlob blob = reader.GetOracleBlob(0);

                            // Clear any previous videoStream data
                            videoStream.SetLength(0);

                            // Process BLOB in chunks of 4096 bytes (4KB)
                            byte[] buffer = new byte[4096];
                            int bytesRead;
                            int totalBytesRead = 0;
                            int totalZeros = 0, totalOnes = 0;
                            StringBuilder binaryString = new StringBuilder();

                            // Read the entire BLOB into the MemoryStream
                            while ((bytesRead = blob.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                totalBytesRead += bytesRead;

                                // Write to the memory stream
                                videoStream.Write(buffer, 0, bytesRead);

                                // Count 0s and 1s for the data
                                for (int i = 0; i < bytesRead; i++)
                                {
                                    string binaryByte = Convert.ToString(buffer[i], 2).PadLeft(8, '0');

                                    // Count 0s and 1s
                                    foreach (char bit in binaryByte)
                                    {
                                        if (bit == '0') totalZeros++;
                                        else totalOnes++;
                                    }

                                    // For binary string output, only include the first 16KB of data
                                    if (totalBytesRead <= 16384)
                                    {
                                        binaryString.Append(binaryByte);
                                    }
                                }
                            }

                            // Calculate the average bit value
                            double averageValue = (double)totalOnes / (totalOnes + totalZeros);

                            // Display the statistics for the entire file and the first 16KB of the BLOB as binary
                            textBoxBinaryOutput.Text = $"Processed {totalBytesRead / 1024} KB of binary data.\n" +
                                                       $"Total 0s: {totalZeros}, Total 1s: {totalOnes}\n" +
                                                       $"Average bit value: {averageValue:F2}\n\n" +
                                                       "First 16KB of data (in binary):\n" +
                                                       binaryString.ToString();

                            // Reset the position of the MemoryStream for playback
                            videoStream.Seek(0, SeekOrigin.Begin);
                        }
                        else
                        {
                            MessageBox.Show("No matching record found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        // This event handler can be left empty or removed if not needed
        private void textBoxBinaryOutput_TextChanged(object sender, EventArgs e)
        {
            // Optionally implement this if you need to handle changes in the TextBox
        }
    }
}
