namespace serwer
{
    partial class Form3
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
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            richTextBox2 = new System.Windows.Forms.RichTextBox();
            richTextBox3 = new System.Windows.Forms.RichTextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            richTextBox4 = new System.Windows.Forms.RichTextBox();
            label3 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            richTextBox1.BackColor = System.Drawing.SystemColors.InfoText;
            richTextBox1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            richTextBox1.ForeColor = System.Drawing.SystemColors.Window;
            richTextBox1.Location = new System.Drawing.Point(-2, 1);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            richTextBox1.Size = new System.Drawing.Size(817, 408);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "aaaaaaaaaaaaaaaaaaa";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // button1
            // 
            button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            button1.Location = new System.Drawing.Point(713, 415);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "Exit";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            button2.Location = new System.Drawing.Point(632, 415);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 23);
            button2.TabIndex = 2;
            button2.Text = "clear";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // richTextBox2
            // 
            richTextBox2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            richTextBox2.BackColor = System.Drawing.SystemColors.MenuText;
            richTextBox2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            richTextBox2.ForeColor = System.Drawing.SystemColors.Window;
            richTextBox2.Location = new System.Drawing.Point(60, 415);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.ReadOnly = true;
            richTextBox2.Size = new System.Drawing.Size(110, 27);
            richTextBox2.TabIndex = 3;
            richTextBox2.Text = "";
            richTextBox2.TextChanged += richTextBox2_TextChanged;
            // 
            // richTextBox3
            // 
            richTextBox3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            richTextBox3.BackColor = System.Drawing.SystemColors.InfoText;
            richTextBox3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            richTextBox3.ForeColor = System.Drawing.Color.White;
            richTextBox3.Location = new System.Drawing.Point(310, 415);
            richTextBox3.Name = "richTextBox3";
            richTextBox3.ReadOnly = true;
            richTextBox3.Size = new System.Drawing.Size(99, 27);
            richTextBox3.TabIndex = 4;
            richTextBox3.Text = "xdddd";
            richTextBox3.TextChanged += richTextBox3_TextChanged;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            label1.Location = new System.Drawing.Point(12, 418);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(46, 20);
            label1.TabIndex = 5;
            label1.Text = "IPV4:";
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            label2.Location = new System.Drawing.Point(176, 418);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(128, 20);
            label2.TabIndex = 6;
            label2.Text = "Default gateway:";
            // 
            // richTextBox4
            // 
            richTextBox4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            richTextBox4.BackColor = System.Drawing.SystemColors.InfoText;
            richTextBox4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            richTextBox4.ForeColor = System.Drawing.Color.White;
            richTextBox4.Location = new System.Drawing.Point(464, 414);
            richTextBox4.Name = "richTextBox4";
            richTextBox4.ReadOnly = true;
            richTextBox4.Size = new System.Drawing.Size(99, 27);
            richTextBox4.TabIndex = 7;
            richTextBox4.Text = "xdddd";
            richTextBox4.TextChanged += richTextBox4_TextChanged;
            // 
            // label3
            // 
            label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            label3.Location = new System.Drawing.Point(415, 416);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(43, 20);
            label3.TabIndex = 8;
            label3.Text = "Port:";
            // 
            // Form3
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(label3);
            Controls.Add(richTextBox4);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(richTextBox3);
            Controls.Add(richTextBox2);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(richTextBox1);
            Name = "Form3";
            Text = "Form3";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox richTextBox4;
        private System.Windows.Forms.Label label3;
    }
}