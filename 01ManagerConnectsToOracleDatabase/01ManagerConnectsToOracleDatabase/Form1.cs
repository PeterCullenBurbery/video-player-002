using Oracle.ManagedDataAccess.Client; // ODP.NET for Oracle database connection
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Text;

namespace _01ManagerConnectsToOracleDatabase
{
    public partial class Form1 : Form
    {
        // Updated Oracle connection string with Kingdom_Rush database properties
        private const string connectionString = "User Id=Kingdom_Rush;Password=1234;Data Source=localhost:1521/Kingdom_Rush.localdomain";

        public Form1()
        {
            InitializeComponent();
            LoadBlobFromDatabase(); // Automatically loads the BLOB from the database when the form initializes
        }

        // Method to connect to Oracle, retrieve the BLOB in chunks, and display the first few kilobytes
        private void LoadBlobFromDatabase()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    // Open connection
                    conn.Open();

                    // Define the SQL query using regexp_like to match the stage, difficulty, play mode, and player
                    string sql = @"
                    SELECT VIDEO 
                    FROM beat_the_level bl
                    JOIN kingdom_rush_level_stage ks ON bl.KINGDOM_RUSH_LEVEL_STAGE_ID = ks.KINGDOM_RUSH_LEVEL_STAGE_ID
                    JOIN play_mode pm ON bl.PLAY_MODE_ID = pm.PLAY_MODE_ID
                    JOIN game_difficulty gd ON bl.GAME_DIFFICULTY_ID = gd.GAME_DIFFICULTY_ID
                    JOIN player p ON bl.PLAYER_ID = p.PLAYER_ID
                    WHERE REGEXP_LIKE(ks.kingdom_rush_level_stage, 'the grand', 'i') 
                    AND REGEXP_LIKE(gd.game_difficulty, 'impossible', 'i') 
                    AND REGEXP_LIKE(pm.play_mode, 'campaign', 'i') 
                    AND REGEXP_LIKE(p.username, 'best of the best cream of the crop crème de la crème', 'i')";

                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        // Execute the query and fetch the BLOB data
                        using (OracleDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
                        {
                            if (reader.Read())
                            {
                                // Read the BLOB column (VIDEO)
                                OracleBlob blob = reader.GetOracleBlob(0);

                                // Process BLOB in chunks of 4096 bytes (4KB)
                                byte[] buffer = new byte[4096];
                                int bytesRead;
                                int totalBytesRead = 0;
                                StringBuilder binaryString = new StringBuilder();

                                // Read and process up to the first 16KB only
                                while ((bytesRead = blob.Read(buffer, 0, buffer.Length)) > 0 && totalBytesRead < 16384)
                                {
                                    totalBytesRead += bytesRead;

                                    for (int i = 0; i < bytesRead; i++)
                                    {
                                        binaryString.Append(Convert.ToString(buffer[i], 2).PadLeft(8, '0'));
                                    }
                                }

                                // Display the first 16KB of the BLOB as binary in the TextBox
                                textBoxBinaryOutput.Text = $"Processed {totalBytesRead / 1024} KB of binary data.\n" +
                                                           binaryString.ToString();
                            }
                            else
                            {
                                MessageBox.Show("No matching record found.");
                            }
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
