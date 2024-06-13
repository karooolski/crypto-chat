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
            SuspendLayout();
            // 
            // connectButton
            // 
            connectButton.Location = new System.Drawing.Point(311, 237);
            connectButton.Name = "connectButton";
            connectButton.Size = new System.Drawing.Size(156, 49);
            connectButton.TabIndex = 0;
            connectButton.Text = "Connect";
            connectButton.UseVisualStyleBackColor = true;
            connectButton.Click += button1_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new System.Drawing.Point(290, 201);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(201, 30);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(richTextBox1);
            Controls.Add(connectButton);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button connectButton;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}
