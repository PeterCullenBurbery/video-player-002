namespace VideoPlayer
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
            videoView1 = new LibVLCSharp.WinForms.VideoView();
            panel3 = new Panel();
            panel4 = new Panel();
            panel5 = new Panel();
            playButton = new Button();
            pauseButton = new Button();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)videoView1).BeginInit();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1717, 75);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(videoView1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 75);
            panel2.Name = "panel2";
            panel2.Size = new Size(1717, 696);
            panel2.TabIndex = 1;
            // 
            // videoView1
            // 
            videoView1.BackColor = Color.Black;
            videoView1.Dock = DockStyle.Fill;
            videoView1.Location = new Point(0, 0);
            videoView1.MediaPlayer = null;
            videoView1.Name = "videoView1";
            videoView1.Size = new Size(1717, 696);
            videoView1.TabIndex = 0;
            videoView1.Text = "videoView1";
            videoView1.Click += videoView1_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(pauseButton);
            panel3.Controls.Add(playButton);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 689);
            panel3.Name = "panel3";
            panel3.Size = new Size(1717, 82);
            panel3.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.Dock = DockStyle.Left;
            panel4.Location = new Point(0, 75);
            panel4.Name = "panel4";
            panel4.Size = new Size(97, 614);
            panel4.TabIndex = 3;
            // 
            // panel5
            // 
            panel5.Dock = DockStyle.Right;
            panel5.Location = new Point(1648, 75);
            panel5.Name = "panel5";
            panel5.Size = new Size(69, 614);
            panel5.TabIndex = 4;
            // 
            // playButton
            // 
            playButton.Location = new Point(54, 11);
            playButton.Name = "playButton";
            playButton.Size = new Size(145, 58);
            playButton.TabIndex = 0;
            playButton.Text = "Play";
            playButton.UseVisualStyleBackColor = true;
            playButton.Click += playButton_Click;
            // 
            // pauseButton
            // 
            pauseButton.Location = new Point(223, 11);
            pauseButton.Name = "pauseButton";
            pauseButton.Size = new Size(145, 58);
            pauseButton.TabIndex = 1;
            pauseButton.Text = "Pause";
            pauseButton.UseVisualStyleBackColor = true;
            pauseButton.Click += pauseButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1717, 771);
            Controls.Add(panel5);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Form1";
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)videoView1).EndInit();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private LibVLCSharp.WinForms.VideoView videoView1;
        private Button pauseButton;
        private Button playButton;
    }
}
