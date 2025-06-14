namespace P2P_VoiceChat_Latest_Trial
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
            startLoopBackButton = new Button();
            stopLoopBackButton = new Button();
            stunBox = new Button();
            stunIPTxt = new TextBox();
            stunPortTxt = new TextBox();
            label1 = new Label();
            label2 = new Label();
            targetIPBox = new TextBox();
            targetPortBox = new TextBox();
            startSendingButton = new Button();
            stopSendingButton = new Button();
            startReceivingButton = new Button();
            stopReceivingButton = new Button();
            SuspendLayout();
            // 
            // startLoopBackButton
            // 
            startLoopBackButton.Location = new Point(407, 28);
            startLoopBackButton.Name = "startLoopBackButton";
            startLoopBackButton.Size = new Size(108, 25);
            startLoopBackButton.TabIndex = 0;
            startLoopBackButton.Text = "start loopback";
            startLoopBackButton.UseVisualStyleBackColor = true;
            startLoopBackButton.Click += startLoopBackButton_Click;
            // 
            // stopLoopBackButton
            // 
            stopLoopBackButton.Location = new Point(407, 59);
            stopLoopBackButton.Name = "stopLoopBackButton";
            stopLoopBackButton.Size = new Size(108, 25);
            stopLoopBackButton.TabIndex = 1;
            stopLoopBackButton.Text = "stop loopback";
            stopLoopBackButton.UseVisualStyleBackColor = true;
            stopLoopBackButton.Click += stopLoopBackButton_Click;
            // 
            // stunBox
            // 
            stunBox.Location = new Point(12, 23);
            stunBox.Name = "stunBox";
            stunBox.Size = new Size(123, 39);
            stunBox.TabIndex = 2;
            stunBox.Text = "get STUN info";
            stunBox.UseVisualStyleBackColor = true;
            stunBox.Click += stunBox_Click;
            // 
            // stunIPTxt
            // 
            stunIPTxt.Location = new Point(176, 20);
            stunIPTxt.Name = "stunIPTxt";
            stunIPTxt.Size = new Size(100, 23);
            stunIPTxt.TabIndex = 3;
            stunIPTxt.Text = "STUN IP";
            // 
            // stunPortTxt
            // 
            stunPortTxt.Location = new Point(176, 49);
            stunPortTxt.Name = "stunPortTxt";
            stunPortTxt.Size = new Size(100, 23);
            stunPortTxt.TabIndex = 4;
            stunPortTxt.Text = "STUN Port";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(150, 28);
            label1.Name = "label1";
            label1.Size = new Size(20, 15);
            label1.TabIndex = 5;
            label1.Text = "IP:";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(141, 52);
            label2.Name = "label2";
            label2.Size = new Size(32, 15);
            label2.TabIndex = 6;
            label2.Text = "Port:";
            // 
            // targetIPBox
            // 
            targetIPBox.Location = new Point(162, 171);
            targetIPBox.Name = "targetIPBox";
            targetIPBox.Size = new Size(124, 23);
            targetIPBox.TabIndex = 7;
            targetIPBox.Text = "Enter target IP";
            // 
            // targetPortBox
            // 
            targetPortBox.Location = new Point(162, 200);
            targetPortBox.Name = "targetPortBox";
            targetPortBox.Size = new Size(124, 23);
            targetPortBox.TabIndex = 8;
            targetPortBox.Text = "Enter target Port";
            // 
            // startSendingButton
            // 
            startSendingButton.Location = new Point(6, 159);
            startSendingButton.Name = "startSendingButton";
            startSendingButton.Size = new Size(124, 35);
            startSendingButton.TabIndex = 11;
            startSendingButton.Text = "start sending sound";
            startSendingButton.UseVisualStyleBackColor = true;
            startSendingButton.Click += startSendingButton_Click;
            // 
            // stopSendingButton
            // 
            stopSendingButton.Location = new Point(6, 209);
            stopSendingButton.Name = "stopSendingButton";
            stopSendingButton.Size = new Size(124, 35);
            stopSendingButton.TabIndex = 12;
            stopSendingButton.Text = "stop sending sound";
            stopSendingButton.UseVisualStyleBackColor = true;
            stopSendingButton.Click += stopSendingButton_Click;
            // 
            // startReceivingButton
            // 
            startReceivingButton.AccessibleDescription = "startReceivingSound";
            startReceivingButton.Location = new Point(311, 159);
            startReceivingButton.Name = "startReceivingButton";
            startReceivingButton.Size = new Size(140, 35);
            startReceivingButton.TabIndex = 13;
            startReceivingButton.Text = "start receiving sound";
            startReceivingButton.UseVisualStyleBackColor = true;
            startReceivingButton.Click += startReceivingButton_Click;
            // 
            // stopReceivingButton
            // 
            stopReceivingButton.AccessibleDescription = "stopReceivingSound";
            stopReceivingButton.Location = new Point(311, 210);
            stopReceivingButton.Name = "stopReceivingButton";
            stopReceivingButton.Size = new Size(140, 35);
            stopReceivingButton.TabIndex = 14;
            stopReceivingButton.Text = "stop receiving sound";
            stopReceivingButton.UseVisualStyleBackColor = true;
            stopReceivingButton.Click += stopReceivingButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(571, 266);
            Controls.Add(stopReceivingButton);
            Controls.Add(startReceivingButton);
            Controls.Add(stopSendingButton);
            Controls.Add(startSendingButton);
            Controls.Add(targetPortBox);
            Controls.Add(targetIPBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(stunPortTxt);
            Controls.Add(stunIPTxt);
            Controls.Add(stunBox);
            Controls.Add(stopLoopBackButton);
            Controls.Add(startLoopBackButton);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button startLoopBackButton;
        private Button stopLoopBackButton;
        private Button stunBox;
        private TextBox stunIPTxt;
        private TextBox stunPortTxt;
        private Label label1;
        private Label label2;
        private TextBox targetIPBox;
        private TextBox targetPortBox;
        private Button startSendingButton;
        private Button stopSendingButton;
        private Button startReceivingButton;
        private Button stopReceivingButton;
    }
}
