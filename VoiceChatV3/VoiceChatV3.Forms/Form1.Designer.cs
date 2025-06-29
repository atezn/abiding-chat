namespace VoiceChatV3.Forms
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
            txtServerIp = new TextBox();
            deviceList = new ListBox();
            userList = new ListBox();
            connect = new Button();
            Refresh = new Button();
            Start = new Button();
            End = new Button();
            Exit = new Button();
            txtPort = new TextBox();
            SuspendLayout();
            // 
            // txtServerIp
            // 
            txtServerIp.Location = new Point(40, 32);
            txtServerIp.Name = "txtServerIp";
            txtServerIp.Size = new Size(243, 23);
            txtServerIp.TabIndex = 0;
            txtServerIp.TextChanged += txtServerIp_TextChanged;
            // 
            // deviceList
            // 
            deviceList.FormattingEnabled = true;
            deviceList.ItemHeight = 15;
            deviceList.Location = new Point(40, 161);
            deviceList.Name = "deviceList";
            deviceList.Size = new Size(248, 139);
            deviceList.TabIndex = 1;
            deviceList.SelectedIndexChanged += deviceList_SelectedIndexChanged;
            // 
            // userList
            // 
            userList.FormattingEnabled = true;
            userList.ItemHeight = 15;
            userList.Location = new Point(301, 26);
            userList.Name = "userList";
            userList.Size = new Size(182, 274);
            userList.TabIndex = 2;
            userList.SelectedIndexChanged += userList_SelectedIndexChanged;
            // 
            // connect
            // 
            connect.Location = new Point(76, 110);
            connect.Name = "connect";
            connect.Size = new Size(170, 20);
            connect.TabIndex = 3;
            connect.Text = "Connect";
            connect.UseVisualStyleBackColor = true;
            connect.Click += connect_Click;
            // 
            // Refresh
            // 
            Refresh.Location = new Point(76, 317);
            Refresh.Name = "Refresh";
            Refresh.Size = new Size(170, 20);
            Refresh.TabIndex = 4;
            Refresh.Text = "Refresh";
            Refresh.UseVisualStyleBackColor = true;
            Refresh.Click += refresh_Click;
            // 
            // Start
            // 
            Start.Location = new Point(313, 317);
            Start.Name = "Start";
            Start.Size = new Size(170, 20);
            Start.TabIndex = 5;
            Start.Text = "Start";
            Start.UseVisualStyleBackColor = true;
            Start.Click += Start_Click;
            // 
            // End
            // 
            End.Location = new Point(313, 354);
            End.Name = "End";
            End.Size = new Size(170, 20);
            End.TabIndex = 6;
            End.Text = "End";
            End.UseVisualStyleBackColor = true;
            End.Click += End_Click;
            // 
            // Exit
            // 
            Exit.Location = new Point(76, 354);
            Exit.Name = "Exit";
            Exit.Size = new Size(170, 20);
            Exit.TabIndex = 8;
            Exit.Text = "Exit";
            Exit.UseVisualStyleBackColor = true;
            Exit.Click += Exit_Click;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(40, 61);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(243, 23);
            txtPort.TabIndex = 9;
            txtPort.TextChanged += txtPort_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(551, 416);
            Controls.Add(txtPort);
            Controls.Add(Exit);
            Controls.Add(End);
            Controls.Add(Start);
            Controls.Add(Refresh);
            Controls.Add(connect);
            Controls.Add(userList);
            Controls.Add(deviceList);
            Controls.Add(txtServerIp);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtServerIp;
        private ListBox deviceList;
        private ListBox userList;
        private Button connect;
        private Button Refresh;
        private Button Start;
        private Button End;
        private Button Exit;
        private TextBox txtPort;
    }
}
