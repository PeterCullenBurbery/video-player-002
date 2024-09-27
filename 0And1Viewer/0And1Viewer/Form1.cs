using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace _0And1Viewer
{
    public partial class Form1 : Form
    {
        // Hardcoded file path
        private const string filePath = @"C:\Users\peter\source\repos\video-player-002\hex_test_file.bin";

        public Form1()
        {
            InitializeComponent();
            LoadBinaryFile(); // Automatically loads the file when the form initializes
        }

        // Method to load the binary file and convert it to a string of 0s and 1s
        private void LoadBinaryFile()
        {
            try
            {
                // Read all bytes from the file
                byte[] fileBytes = File.ReadAllBytes(filePath);

                // StringBuilder to store binary string
                StringBuilder binaryString = new StringBuilder();

                // Convert each byte to its binary representation (8 bits)
                foreach (byte b in fileBytes)
                {
                    binaryString.Append(Convert.ToString(b, 2).PadLeft(8, '0')); // Convert byte to binary and pad with 0s
                }

                // Display the binary string in the TextBox (assumes TextBox is named textBoxBinaryOutput)
                textBoxBinaryOutput.Text = binaryString.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading file: {ex.Message}");
            }
        }

        // This event handler can be left empty or removed if not needed
        private void textBoxBinaryOutput_TextChanged(object sender, EventArgs e)
        {
            // Optionally implement this if you need to handle changes in the TextBox
        }
    }
}
