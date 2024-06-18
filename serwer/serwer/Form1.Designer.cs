namespace serwer
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
            connectButton = new System.Windows.Forms.Button();
            printDocument1 = new System.Drawing.Printing.PrintDocument();
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            label1 = new System.Windows.Forms.Label();
            richTextBox2 = new System.Windows.Forms.RichTextBox();
            label2 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // connectButton
            // 
            connectButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            connectButton.Location = new System.Drawing.Point(311, 237);
            connectButton.Name = "connectButton";
            connectButton.Size = new System.Drawing.Size(156, 49);
            connectButton.TabIndex = 0;
            connectButton.Text = "Open Server";
            connectButton.UseVisualStyleBackColor = true;
            connectButton.Click += button1_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            richTextBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            richTextBox1.Location = new System.Drawing.Point(290, 201);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(201, 30);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            label1.Location = new System.Drawing.Point(259, 177);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(267, 21);
            label1.TabIndex = 3;
            label1.Text = "Port na którym nasłuchuje serwer";
            label1.Click += label1_Click;
            // 
            // richTextBox2
            // 
            richTextBox2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            richTextBox2.Cursor = System.Windows.Forms.Cursors.SizeNESW;
            richTextBox2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            richTextBox2.Location = new System.Drawing.Point(300, 408);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.ReadOnly = true;
            richTextBox2.Size = new System.Drawing.Size(201, 30);
            richTextBox2.TabIndex = 4;
            richTextBox2.Text = "";
            richTextBox2.TextChanged += richTextBox2_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            label2.Location = new System.Drawing.Point(290, 374);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(218, 21);
            label2.TabIndex = 5;
            label2.Text = "Server IP (default gateway)";
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.WindowFrame;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(label2);
            Controls.Add(richTextBox2);
            Controls.Add(label1);
            Controls.Add(richTextBox1);
            Controls.Add(connectButton);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button connectButton;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Label label2;
    }
}
