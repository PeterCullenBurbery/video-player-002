using Oracle.ManagedDataAccess.Client; // ODP.NET for Oracle database connection
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Text;

namespace _01ManagerConnectsToOracleDatabase
{
    public partial class Form1 : Form
    {
        // Updated Oracle connection string with your database properties
        private const string connectionString = "User Id=text_0_1_manager;Password=1234;Data Source=localhost:1521/text_0_1_manager.localdomain";

        public Form1()
        {
            InitializeComponent();
            LoadBlobFromDatabase(); // Automatically loads the BLOB from the database when the form initializes
        }

        // Method to connect to Oracle, retrieve the BLOB, and display the binary string
        private void LoadBlobFromDatabase()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    // Open connection
                    conn.Open();

                    // Define the SQL query using regexp_like to match the description
                    string sql = "SELECT data FROM test_blob_table WHERE REGEXP_LIKE(description, 'first', 'i')";

                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        // Execute the query and fetch the BLOB data
                        using (OracleDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
                        {
                            if (reader.Read())
                            {
                                // Read the BLOB column
                                OracleBlob blob = reader.GetOracleBlob(0);

                                // Convert the BLOB to a binary string (0s and 1s)
                                byte[] blobBytes = new byte[blob.Length];
                                blob.Read(blobBytes, 0, (int)blob.Length);

                                // Convert byte array to a binary string
                                StringBuilder binaryString = new StringBuilder();

                                foreach (byte b in blobBytes)
                                {
                                    binaryString.Append(Convert.ToString(b, 2).PadLeft(8, '0')); // Convert each byte to its 8-bit binary form
                                }

                                // Display the binary string in the TextBox (assumes TextBox is named textBoxBinaryOutput)
                                textBoxBinaryOutput.Text = binaryString.ToString();
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

