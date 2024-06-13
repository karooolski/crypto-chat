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
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = System.Drawing.SystemColors.InfoText;
            richTextBox1.ForeColor = System.Drawing.SystemColors.Window;
            richTextBox1.Location = new System.Drawing.Point(-2, 1);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(803, 455);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // Form3
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(richTextBox1);
            Name = "Form3";
            Text = "Form3";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}