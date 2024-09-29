using System;
using System.IO;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace GenerateKingdomRushFolders
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenerateFolders_Click(object sender, EventArgs e)
        {
            // Base folder path where folders will be created
            string baseFolderPath = @"\\vmware-host\Shared Folders\C\kingdom-rush";

            // Connection string to your Oracle database
            string connectionString = "User Id=Kingdom_Rush;Password=1234;Data Source=localhost:1521/Kingdom_Rush_specify_levels_game_002.localdomain";

            // Initialize counters for folders created
            int gameFolderCount = 0;
            int stageFolderCount = 0;
            int playModeFolderCount = 0;
            int difficultyFolderCount = 0;

            // Establish a connection to the Oracle database
            using (OracleConnection con = new OracleConnection(connectionString))
            {
                con.Open();

                // Step 1: Get all games from the game table
                OracleCommand getGamesCommand = new OracleCommand("SELECT g.GAME_ID, g.GAME FROM GAME g", con);
                using (OracleDataReader gamesReader = getGamesCommand.ExecuteReader())
                {
                    while (gamesReader.Read())
                    {
                        string gameId = BitConverter.ToString((byte[])gamesReader["GAME_ID"]).Replace("-", "");  // Convert bytes to hex string
                        string gameName = gamesReader["GAME"].ToString();
                        string gameFolderPath = Path.Combine(baseFolderPath, gameName);

                        // Create the folder for the game
                        Directory.CreateDirectory(gameFolderPath);
                        gameFolderCount++;

                        // Step 2: Get stages for each game by joining game and level_stage
                        OracleCommand getStagesCommand = new OracleCommand(@"
                            SELECT l.LEVEL_STAGE_ID, l.LEVEL_STAGE 
                            FROM LEVEL_STAGE l
                            INNER JOIN GAME g ON l.GAME_ID = g.GAME_ID
                            WHERE g.GAME = :gameName", con);
                        getStagesCommand.Parameters.Add(new OracleParameter("gameName", gameName));

                        using (OracleDataReader stagesReader = getStagesCommand.ExecuteReader())
                        {
                            while (stagesReader.Read())
                            {
                                string stageId = BitConverter.ToString((byte[])stagesReader["LEVEL_STAGE_ID"]).Replace("-", "");
                                string stageName = stagesReader["LEVEL_STAGE"].ToString();
                                string stageFolderPath = Path.Combine(gameFolderPath, stageName);

                                // Create the folder for the stage
                                Directory.CreateDirectory(stageFolderPath);
                                stageFolderCount++;

                                // Step 3: Get play modes for each stage by joining with play_mode
                                OracleCommand getPlayModesCommand = new OracleCommand(@"
                                    SELECT p.PLAY_MODE_ID, p.PLAY_MODE
                                    FROM PLAY_MODE p", con);

                                using (OracleDataReader playModesReader = getPlayModesCommand.ExecuteReader())
                                {
                                    while (playModesReader.Read())
                                    {
                                        string playMode = playModesReader["PLAY_MODE"].ToString();
                                        string playModeFolderPath = Path.Combine(stageFolderPath, playMode);

                                        // Create the folder for the play mode
                                        Directory.CreateDirectory(playModeFolderPath);
                                        playModeFolderCount++;

                                        // Step 4: Add difficulties based on the game
                                        string[] difficulties;

                                        if (gameName == "Kingdom Rush" || gameName == "Kingdom Rush Frontiers")
                                        {
                                            difficulties = new[] { "Casual", "Normal", "Veteran" }; // No "Impossible"
                                        }
                                        else  // For KRO, KRV, and KR5
                                        {
                                            difficulties = new[] { "Casual", "Normal", "Veteran", "Impossible" };  // "Impossible" added
                                        }

                                        foreach (string difficulty in difficulties)
                                        {
                                            string difficultyFolderPath = Path.Combine(playModeFolderPath, difficulty);

                                            // Create the folder for the difficulty
                                            Directory.CreateDirectory(difficultyFolderPath);
                                            difficultyFolderCount++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Calculate total folders created
            int totalFoldersCreated = gameFolderCount + stageFolderCount + playModeFolderCount + difficultyFolderCount;

            // Update the TextBox with folder creation details
            textBox1.Text = "Folders created successfully!" + Environment.NewLine;
            textBox1.AppendText($"Game folders created: {gameFolderCount}" + Environment.NewLine);
            textBox1.AppendText($"Stage folders created: {stageFolderCount}" + Environment.NewLine);
            textBox1.AppendText($"Play Mode folders created: {playModeFolderCount}" + Environment.NewLine);
            textBox1.AppendText($"Difficulty folders created: {difficultyFolderCount}" + Environment.NewLine);
            textBox1.AppendText($"Total folders created: {totalFoldersCreated}");
        }
    }
}
