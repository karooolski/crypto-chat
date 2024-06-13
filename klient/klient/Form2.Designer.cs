namespace klient
{
    partial class Form2
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
            label1 = new System.Windows.Forms.Label();
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            richTextBox2 = new System.Windows.Forms.RichTextBox();
            button1 = new System.Windows.Forms.Button();
            richTextBox3 = new System.Windows.Forms.RichTextBox();
            richTextBox4 = new System.Windows.Forms.RichTextBox();
            richTextBox5 = new System.Windows.Forms.RichTextBox();
            button2 = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            button3 = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(23, 19);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(38, 15);
            label1.TabIndex = 0;
            label1.Text = "label1";
            label1.Click += label1_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = System.Drawing.SystemColors.Info;
            richTextBox1.Location = new System.Drawing.Point(23, 45);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(738, 262);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // richTextBox2
            // 
            richTextBox2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            richTextBox2.Location = new System.Drawing.Point(287, 313);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new System.Drawing.Size(222, 130);
            richTextBox2.TabIndex = 2;
            richTextBox2.Text = "";
            richTextBox2.TextChanged += richTextBox2_TextChanged;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(515, 355);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(122, 44);
            button1.TabIndex = 3;
            button1.Text = "send message";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // richTextBox3
            // 
            richTextBox3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            richTextBox3.Location = new System.Drawing.Point(91, 323);
            richTextBox3.Name = "richTextBox3";
            richTextBox3.Size = new System.Drawing.Size(146, 30);
            richTextBox3.TabIndex = 4;
            richTextBox3.Text = "";
            richTextBox3.TextChanged += richTextBox3_TextChanged;
            // 
            // richTextBox4
            // 
            richTextBox4.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            richTextBox4.Location = new System.Drawing.Point(643, 326);
            richTextBox4.Name = "richTextBox4";
            richTextBox4.Size = new System.Drawing.Size(146, 30);
            richTextBox4.TabIndex = 5;
            richTextBox4.Text = "";
            // 
            // richTextBox5
            // 
            richTextBox5.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            richTextBox5.Location = new System.Drawing.Point(643, 362);
            richTextBox5.Name = "richTextBox5";
            richTextBox5.Size = new System.Drawing.Size(146, 30);
            richTextBox5.TabIndex = 6;
            richTextBox5.Text = "";
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(715, 408);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(73, 35);
            button2.TabIndex = 7;
            button2.Text = "log out";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(32, 326);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(38, 15);
            label2.TabIndex = 8;
            label2.Text = "label2";
            label2.Click += label2_Click;
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(102, 362);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(122, 44);
            button3.TabIndex = 9;
            button3.Text = "request crypted chat";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ActiveBorder;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(button3);
            Controls.Add(label2);
            Controls.Add(button2);
            Controls.Add(richTextBox5);
            Controls.Add(richTextBox4);
            Controls.Add(richTextBox3);
            Controls.Add(button1);
            Controls.Add(richTextBox2);
            Controls.Add(richTextBox1);
            Controls.Add(label1);
            Name = "Form2";
            Text = "Form2";
            Load += Form2_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.RichTextBox richTextBox4;
        private System.Windows.Forms.RichTextBox richTextBox5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
    }
}