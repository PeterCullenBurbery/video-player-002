using Oracle.ManagedDataAccess.Client; // For Oracle database connection
using Oracle.ManagedDataAccess.Types;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace OracleWindowsMediaPlayerMisspelledEvent
{
    public partial class Form1 : Form
    {
        private string tempFilePath = string.Empty; // Path to the temporary buffer file
        private const string connectionString = "User Id=Kingdom_Rush;Password=1234;Data Source=localhost:1521/Kingdom_Rush.localdomain";

        public Form1()
        {
            InitializeComponent();
        }

        // This event is triggered when Windows Media Player control is entered
        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            if (File.Exists(tempFilePath))
            {
                // Play the video if the temporary file is loaded
                axWindowsMediaPlayer1.URL = tempFilePath;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            else
            {
                // Prompt to load the file if it's not loaded yet
                MessageBox.Show("Please click 'Load Video' to load the video file.");
            }
        }

        // Button click event to load video data from the database and create the temporary file
        private void loadVideoButton_Click(object sender, EventArgs e)
        {
            LoadBlobFromDatabase(); // Generate the temporary file
        }

        // Load video data from the database and create the temporary file with .mp4 extension
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

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Read the BLOB column (VIDEO)
                            OracleBlob blob = reader.GetOracleBlob(0);

                            // Generate a temp file path with .mp4 extension using 2 GUIDs
                            string guid1 = Guid.NewGuid().ToString();
                            string guid2 = Guid.NewGuid().ToString();
                            string tempFileName = Path.Combine(Path.GetTempPath(), $"{guid1}_{guid2}.mp4");
                            tempFilePath = tempFileName;

                            // Write the BLOB to the temporary .mp4 file
                            using (FileStream tempFileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                            {
                                byte[] buffer = new byte[4096]; // 4KB buffer size
                                int bytesRead;
                                int totalBytesRead = 0;
                                int totalZeros = 0, totalOnes = 0;

                                // Read the BLOB in chunks and process data
                                while ((bytesRead = blob.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    tempFileStream.Write(buffer, 0, bytesRead);

                                    // Count 0s and 1s for the data
                                    for (int i = 0; i < bytesRead; i++)
                                    {
                                        string binaryByte = Convert.ToString(buffer[i], 2).PadLeft(8, '0');

                                        foreach (char bit in binaryByte)
                                        {
                                            if (bit == '0') totalZeros++;
                                            else totalOnes++;
                                        }
                                    }
                                    totalBytesRead += bytesRead;
                                }

                                // Close the file stream to ensure all data is written
                                tempFileStream.Close();

                                // Calculate the average bit value
                                double averageValue = (double)totalOnes / (totalOnes + totalZeros);

                                // Display the summary in textBox1
                                textBox1.Text = $"Total Data Processed: {totalBytesRead / 1024} KB\n" +
                                                $"Total 0s: {totalZeros}\n" +
                                                $"Total 1s: {totalOnes}\n" +
                                                $"Average Bit Value: {averageValue:F2}\n\n" +
                                                $"Temporary File: {tempFilePath}";

                                // Now that the temp file is created, inform the user
                                MessageBox.Show("Video file loaded! Click on the media player to play the video.");
                            }
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

        // Clean up the temporary file when the form is closed
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(tempFilePath) && File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath); // Clean up the temp file
            }
        }
    }
}