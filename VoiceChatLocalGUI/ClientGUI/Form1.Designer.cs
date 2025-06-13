namespace ClientGUI
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
            textBox1 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            deviceList = new ListView();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(40, 23);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(420, 31);
            textBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(486, 12);
            button1.Name = "button1";
            button1.Size = new Size(263, 52);
            button1.TabIndex = 2;
            button1.Text = "Connect";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(486, 91);
            button2.Name = "button2";
            button2.Size = new Size(263, 52);
            button2.TabIndex = 3;
            button2.Text = "Refresh Source";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(486, 199);
            button3.Name = "button3";
            button3.Size = new Size(263, 52);
            button3.TabIndex = 4;
            button3.Text = "Start";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(486, 257);
            button4.Name = "button4";
            button4.Size = new Size(263, 52);
            button4.TabIndex = 5;
            button4.Text = "Stop";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(486, 361);
            button5.Name = "button5";
            button5.Size = new Size(263, 52);
            button5.TabIndex = 6;
            button5.Text = "Exit";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // deviceList
            // 
            deviceList.Location = new Point(40, 71);
            deviceList.Name = "deviceList";
            deviceList.Size = new Size(420, 327);
            deviceList.TabIndex = 7;
            deviceList.UseCompatibleStateImageBehavior = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(deviceList);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Client";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private ListView deviceList;
    }
}
