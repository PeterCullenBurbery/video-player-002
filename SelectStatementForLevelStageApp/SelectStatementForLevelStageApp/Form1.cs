using System;
using System.Data;
using System.Diagnostics;  // For opening the browser
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;  // Oracle DataAccess

namespace SelectStatementForLevelStageApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void submitSQL(object sender, EventArgs e)
        {
            string userQuery = textBox1.Text;
            string connectionString = "User Id=no_write_instead_read_only;Password=1234;Data Source=localhost:1521/Kingdom_Rush_specify_levels_game_002.localdomain;";

            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    // Set the current schema to Kingdom_Rush
                    using (OracleCommand setSchemaCommand = new OracleCommand("ALTER SESSION SET CURRENT_SCHEMA = Kingdom_Rush", connection))
                    {
                        setSchemaCommand.ExecuteNonQuery();  // Set schema for the session
                    }

                    // Execute the user's query to get level_completion_id
                    using (OracleCommand command = new OracleCommand(userQuery, connection))
                    {
                        OracleDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();

                            // Retrieve the UUID (RAW(16)) as a byte array and convert it to a string
                            byte[] uuidBytes = reader.GetOracleBinary(0).Value;
                            string levelCompletionId = BitConverter.ToString(uuidBytes).Replace("-", "");

                            // Retrieve the YouTube link using the level_completion_id
                            string youtubeLink = GetYouTubeLink(connection, levelCompletionId);

                            if (!string.IsNullOrEmpty(youtubeLink))
                            {
                                // Launch the YouTube link in the default browser
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = youtubeLink,
                                    UseShellExecute = true  // Opens the URL in the default browser
                                });

                                textBox2.Text = "Success! YouTube video opened.";
                            }
                            else
                            {
                                textBox2.Text = "YouTube link not found.";
                            }
                        }
                        else
                        {
                            textBox2.Text = "No results found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                textBox2.Text = "Error: " + ex.Message;
            }
        }

        // Method to retrieve the YouTube video link based on level_completion_id
        private string GetYouTubeLink(OracleConnection connection, string levelCompletionId)
        {
            string youtubeLink = null;
            string query = "SELECT YouTube_video_string FROM level_completion WHERE RAWTOHEX(level_completion_id) = :id";

            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter("id", levelCompletionId));

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        youtubeLink = reader.IsDBNull(0) ? null : reader.GetString(0);  // Retrieve the YouTube video link
                    }
                }
            }

            return youtubeLink;
        }
    }
}
