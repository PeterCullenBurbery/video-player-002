namespace _01ManagerConnectsToOracleDatabase
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            panel4 = new Panel();
            textBoxBinaryOutput = new TextBox();
            videoView1 = new LibVLCSharp.WinForms.VideoView();
            playButton = new Button();
            pauseButton = new Button();
            ((System.ComponentModel.ISupportInitialize)videoView1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1457, 34);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 652);
            panel2.Name = "panel2";
            panel2.Size = new Size(1457, 39);
            panel2.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.Dock = DockStyle.Left;
            panel3.Location = new Point(0, 34);
            panel3.Name = "panel3";
            panel3.Size = new Size(46, 618);
            panel3.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.Dock = DockStyle.Right;
            panel4.Location = new Point(1393, 34);
            panel4.Name = "panel4";
            panel4.Size = new Size(64, 618);
            panel4.TabIndex = 3;
            // 
            // textBoxBinaryOutput
            // 
            textBoxBinaryOutput.Location = new Point(46, 34);
            textBoxBinaryOutput.Multiline = true;
            textBoxBinaryOutput.Name = "textBoxBinaryOutput";
            textBoxBinaryOutput.ReadOnly = true;
            textBoxBinaryOutput.Size = new Size(690, 291);
            textBoxBinaryOutput.TabIndex = 4;
            // 
            // videoView1
            // 
            videoView1.BackColor = Color.Black;
            videoView1.Location = new Point(790, 76);
            videoView1.MediaPlayer = null;
            videoView1.Name = "videoView1";
            videoView1.Size = new Size(472, 248);
            videoView1.TabIndex = 5;
            videoView1.Text = "videoView1";
            // 
            // playButton
            // 
            playButton.Location = new Point(95, 383);
            playButton.Name = "playButton";
            playButton.Size = new Size(100, 44);
            playButton.TabIndex = 6;
            playButton.Text = "Play";
            playButton.UseVisualStyleBackColor = true;
            playButton.Click += playButton_Click;
            // 
            // pauseButton
            // 
            pauseButton.Location = new Point(250, 383);
            pauseButton.Name = "pauseButton";
            pauseButton.Size = new Size(100, 44);
            pauseButton.TabIndex = 7;
            pauseButton.Text = "Pause";
            pauseButton.UseVisualStyleBackColor = true;
            pauseButton.Click += pauseButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1457, 691);
            Controls.Add(pauseButton);
            Controls.Add(playButton);
            Controls.Add(videoView1);
            Controls.Add(textBoxBinaryOutput);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)videoView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private TextBox textBoxBinaryOutput;
        private LibVLCSharp.WinForms.VideoView videoView1;
        private Button playButton;
        private Button pauseButton;
    }
}
