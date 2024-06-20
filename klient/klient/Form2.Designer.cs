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
            richTextBox5 = new System.Windows.Forms.RichTextBox();
            button2 = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            button3 = new System.Windows.Forms.Button();
            button4 = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Cursor = System.Windows.Forms.Cursors.No;
            label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(24, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(51, 20);
            label1.TabIndex = 0;
            label1.Text = "label1";
            label1.Click += label1_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            richTextBox1.BackColor = System.Drawing.SystemColors.Info;
            richTextBox1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            richTextBox1.Location = new System.Drawing.Point(23, 32);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new System.Drawing.Size(750, 275);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // richTextBox2
            // 
            richTextBox2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            richTextBox2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            richTextBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            richTextBox2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            richTextBox2.Location = new System.Drawing.Point(287, 313);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new System.Drawing.Size(222, 130);
            richTextBox2.TabIndex = 2;
            richTextBox2.Text = "";
            richTextBox2.TextChanged += richTextBox2_TextChanged;
            // 
            // button1
            // 
            button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            button1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
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
            richTextBox3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            richTextBox3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            richTextBox3.Cursor = System.Windows.Forms.Cursors.IBeam;
            richTextBox3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            richTextBox3.Location = new System.Drawing.Point(91, 323);
            richTextBox3.Name = "richTextBox3";
            richTextBox3.Size = new System.Drawing.Size(146, 30);
            richTextBox3.TabIndex = 4;
            richTextBox3.Text = "";
            richTextBox3.TextChanged += richTextBox3_TextChanged;
            // 
            // richTextBox5
            // 
            richTextBox5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            richTextBox5.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            richTextBox5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            richTextBox5.Location = new System.Drawing.Point(642, 348);
            richTextBox5.Name = "richTextBox5";
            richTextBox5.ReadOnly = true;
            richTextBox5.Size = new System.Drawing.Size(146, 54);
            richTextBox5.TabIndex = 6;
            richTextBox5.Text = "";
            richTextBox5.TextChanged += richTextBox5_TextChanged;
            // 
            // button2
            // 
            button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            button2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
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
            label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(24, 326);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(61, 20);
            label2.TabIndex = 8;
            label2.Text = "adresat";
            label2.Click += label2_Click;
            // 
            // button3
            // 
            button3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            button3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            button3.Location = new System.Drawing.Point(102, 362);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(122, 44);
            button3.TabIndex = 9;
            button3.Text = "request crypted chat";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            button4.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            button4.Location = new System.Drawing.Point(715, 311);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(73, 35);
            button4.TabIndex = 10;
            button4.Text = "clear";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ActiveBorder;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(label2);
            Controls.Add(button2);
            Controls.Add(richTextBox5);
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
        private System.Windows.Forms.RichTextBox richTextBox5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}