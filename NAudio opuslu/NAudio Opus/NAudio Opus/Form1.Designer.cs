namespace NAudio_Opus
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.myPortBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.targetIPBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.targetPortBox = new System.Windows.Forms.TextBox();
            this.StartCallButton = new System.Windows.Forms.Button();
            this.StopCallButton = new System.Windows.Forms.Button();
            this.connectionStatus = new System.Windows.Forms.Label();
            this.stunBox = new System.Windows.Forms.Button();
            this.stunLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(1141, 187);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(130, 33);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start Loopback";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(1141, 226);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(130, 33);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop Loopback";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // myPortBox
            // 
            this.myPortBox.Location = new System.Drawing.Point(10, 112);
            this.myPortBox.Name = "myPortBox";
            this.myPortBox.Size = new System.Drawing.Size(130, 20);
            this.myPortBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "My Port";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(171, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Target IP";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // targetIPBox
            // 
            this.targetIPBox.Location = new System.Drawing.Point(174, 112);
            this.targetIPBox.Name = "targetIPBox";
            this.targetIPBox.Size = new System.Drawing.Size(130, 20);
            this.targetIPBox.TabIndex = 4;
            this.targetIPBox.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Target Port";
            // 
            // targetPortBox
            // 
            this.targetPortBox.Location = new System.Drawing.Point(174, 158);
            this.targetPortBox.Name = "targetPortBox";
            this.targetPortBox.Size = new System.Drawing.Size(130, 20);
            this.targetPortBox.TabIndex = 6;
            // 
            // StartCallButton
            // 
            this.StartCallButton.Location = new System.Drawing.Point(12, 24);
            this.StartCallButton.Name = "StartCallButton";
            this.StartCallButton.Size = new System.Drawing.Size(92, 32);
            this.StartCallButton.TabIndex = 8;
            this.StartCallButton.Text = "Start Call";
            this.StartCallButton.UseVisualStyleBackColor = true;
            this.StartCallButton.Click += new System.EventHandler(this.StartCallButton_Click);
            // 
            // StopCallButton
            // 
            this.StopCallButton.Location = new System.Drawing.Point(189, 24);
            this.StopCallButton.Name = "StopCallButton";
            this.StopCallButton.Size = new System.Drawing.Size(92, 32);
            this.StopCallButton.TabIndex = 9;
            this.StopCallButton.Text = "Stop Call";
            this.StopCallButton.UseVisualStyleBackColor = true;
            this.StopCallButton.Click += new System.EventHandler(this.StopCallButton_Click);
            // 
            // connectionStatus
            // 
            this.connectionStatus.AutoSize = true;
            this.connectionStatus.Location = new System.Drawing.Point(336, 34);
            this.connectionStatus.Name = "connectionStatus";
            this.connectionStatus.Size = new System.Drawing.Size(94, 13);
            this.connectionStatus.TabIndex = 10;
            this.connectionStatus.Text = "Connection Status";
            // 
            // stunBox
            // 
            this.stunBox.Location = new System.Drawing.Point(553, 74);
            this.stunBox.Name = "stunBox";
            this.stunBox.Size = new System.Drawing.Size(86, 32);
            this.stunBox.TabIndex = 11;
            this.stunBox.Text = "STUN Info";
            this.stunBox.UseVisualStyleBackColor = true;
            this.stunBox.Click += new System.EventHandler(this.stunBox_Click);
            // 
            // stunLabel
            // 
            this.stunLabel.AutoSize = true;
            this.stunLabel.Location = new System.Drawing.Point(669, 84);
            this.stunLabel.Name = "stunLabel";
            this.stunLabel.Size = new System.Drawing.Size(125, 13);
            this.stunLabel.TabIndex = 12;
            this.stunLabel.Text = "use button for STUN info";
            this.stunLabel.Click += new System.EventHandler(this.label4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1283, 260);
            this.Controls.Add(this.stunLabel);
            this.Controls.Add(this.stunBox);
            this.Controls.Add(this.connectionStatus);
            this.Controls.Add(this.StopCallButton);
            this.Controls.Add(this.StartCallButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.targetPortBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.targetIPBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.myPortBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Name = "Form1";
            this.Text = "NAudio Opus Test";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.TextBox myPortBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox targetIPBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox targetPortBox;
        private System.Windows.Forms.Button StartCallButton;
        private System.Windows.Forms.Button StopCallButton;
        private System.Windows.Forms.Label connectionStatus;
        private System.Windows.Forms.Button stunBox;
        private System.Windows.Forms.Label stunLabel;
    }
}

