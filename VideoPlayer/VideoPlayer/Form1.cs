using System;
using System.Data;
using System.IO;
using Oracle.ManagedDataAccess.Client; // Make sure Oracle.ManagedDataAccess.Core is installed
using LibVLCSharp.Shared;

namespace VideoPlayer
{
    public partial class Form1 : Form
    {
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private bool isMediaSet = false; // Track whether the media has already been set
        private bool isPaused = false;   // Track whether the video is paused

        public Form1()
        {
            InitializeComponent();

            // Initialize VLC libraries
            Core.Initialize();

            // Create a new LibVLC instance
            _libVLC = new LibVLC();

            // Create a new media player using the VLC instance and assign the video view
            _mediaPlayer = new MediaPlayer(_libVLC);

            // Connect media player to videoView1
            _mediaPlayer.Hwnd = videoView1.Handle;
        }

        // Play button click event handler
        private void playButton_Click(object sender, EventArgs e)
        {
            if (!isMediaSet)  // Load the video from the database only once
            {
                // Connect to the Oracle database and retrieve the video BLOB
                byte[] videoBlob = GetVideoBlobFromOracle();

                if (videoBlob != null)
                {
                    // Create a MemoryStream from the byte array
                    using (MemoryStream stream = new MemoryStream(videoBlob))
                    {
                        // Create a custom media input for VLC to stream from memory
                        var media = new Media(_libVLC, new StreamMediaInput(stream));

                        // Set the media to the player and play
                        _mediaPlayer.Media = media;
                        _mediaPlayer.Play();

                        // Mark that the media is set and playing
                        isMediaSet = true;
                        isPaused = false;
                    }
                }
                else
                {
                    MessageBox.Show("Failed to load video from database.");
                }
            }
            else  // Resume playing if paused, or pause if playing
            {
                if (isPaused)
                {
                    _mediaPlayer.Play();  // Resume playing
                    isPaused = false;
                }
                else
                {
                    _mediaPlayer.Pause();  // Pause the video
                    isPaused = true;
                }
            }
        }

        // Pause button click event handler (optional)
        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (_mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Pause();
                isPaused = true;
            }
        }

        // Function to retrieve video BLOB from the Oracle database
        private byte[] GetVideoBlobFromOracle()
        {
            // Connection string parameters
            string connectionString = "User Id=Kingdom_Rush;Password=1234;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=Kingdom_Rush.localdomain)));";

            // SQL query with joins and REGEXP_LIKE to select the correct BLOB
            string query = @"
                SELECT btl.video
                FROM kingdom_rush.beat_the_level btl
                JOIN kingdom_rush.player p ON btl.player_id = p.player_id
                JOIN kingdom_rush.kingdom_rush_level_stage kr_stage ON btl.kingdom_rush_level_stage_id = kr_stage.kingdom_rush_level_stage_id
                JOIN kingdom_rush.play_mode pm ON btl.play_mode_id = pm.play_mode_id
                JOIN kingdom_rush.game_difficulty gd ON btl.game_difficulty_id = gd.game_difficulty_id
                WHERE REGEXP_LIKE(kr_stage.kingdom_rush_level_stage, 'The Grand Arena', 'i')
                  AND REGEXP_LIKE(gd.game_difficulty, 'Impossible', 'i')
                  AND REGEXP_LIKE(pm.play_mode, 'Campaign', 'i')
                  AND REGEXP_LIKE(p.username, 'best of the best cream of the crop crème de la crème', 'i')";

            byte[] videoBlob = null;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    // Execute the query
                    using (OracleDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        if (reader.Read())
                        {
                            // Get the BLOB as a byte array
                            using (MemoryStream ms = new MemoryStream())
                            {
                                long bufferSize = reader.GetBytes(0, 0, null, 0, 0); // Get BLOB length
                                byte[] buffer = new byte[bufferSize];
                                reader.GetBytes(0, 0, buffer, 0, (int)bufferSize);
                                ms.Write(buffer, 0, buffer.Length);
                                videoBlob = ms.ToArray();
                            }
                        }
                    }
                }
            }

            return videoBlob;
        }
    }
}
