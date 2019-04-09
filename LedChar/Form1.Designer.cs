namespace WindowsFormsApplication1
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
            this.designButton = new System.Windows.Forms.Button();
            this.clockButton = new System.Windows.Forms.Button();
            this.scrollButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.hideButton = new System.Windows.Forms.Button();
            this.ledDisplay1 = new LedControl.LedDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.ledDisplay1)).BeginInit();
            this.SuspendLayout();
            // 
            // designButton
            // 
            this.designButton.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.designButton.Location = new System.Drawing.Point(54, 446);
            this.designButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.designButton.Name = "designButton";
            this.designButton.Size = new System.Drawing.Size(222, 44);
            this.designButton.TabIndex = 24;
            this.designButton.Text = "Character design";
            this.designButton.UseVisualStyleBackColor = true;
            this.designButton.Click += new System.EventHandler(this.designButton_Click);
            // 
            // clockButton
            // 
            this.clockButton.Location = new System.Drawing.Point(332, 446);
            this.clockButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.clockButton.Name = "clockButton";
            this.clockButton.Size = new System.Drawing.Size(222, 44);
            this.clockButton.TabIndex = 26;
            this.clockButton.Text = "Start/Stop Clock";
            this.clockButton.UseVisualStyleBackColor = true;
            this.clockButton.Click += new System.EventHandler(this.clockButton_Click);
            // 
            // scrollButton
            // 
            this.scrollButton.Location = new System.Drawing.Point(610, 446);
            this.scrollButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.scrollButton.Name = "scrollButton";
            this.scrollButton.Size = new System.Drawing.Size(222, 44);
            this.scrollButton.TabIndex = 27;
            this.scrollButton.Text = "Start/Stop Scroll";
            this.scrollButton.UseVisualStyleBackColor = true;
            this.scrollButton.Click += new System.EventHandler(this.scrollButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(54, 306);
            this.textBox1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(742, 31);
            this.textBox1.TabIndex = 28;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 265);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 25);
            this.label1.TabIndex = 29;
            this.label1.Text = "Text to scroll";
            // 
            // hideButton
            // 
            this.hideButton.Location = new System.Drawing.Point(888, 446);
            this.hideButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.hideButton.Name = "hideButton";
            this.hideButton.Size = new System.Drawing.Size(222, 44);
            this.hideButton.TabIndex = 30;
            this.hideButton.Text = "Hide/Show form";
            this.hideButton.UseVisualStyleBackColor = true;
            this.hideButton.Click += new System.EventHandler(this.hideButton_Click);
            // 
            // ledDisplay1
            // 
            this.ledDisplay1.BackColor = System.Drawing.Color.Black;
            this.ledDisplay1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ledDisplay1.ForeColor = System.Drawing.Color.Gold;
            this.ledDisplay1.HorizontalSegments = 6F;
            this.ledDisplay1.Location = new System.Drawing.Point(0, 0);
            this.ledDisplay1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.ledDisplay1.Name = "ledDisplay1";
            this.ledDisplay1.Size = new System.Drawing.Size(1168, 115);
            this.ledDisplay1.TabIndex = 25;
            this.ledDisplay1.Text = "CREATIVE";
            this.ledDisplay1.VerticalSegments = 8F;
            this.ledDisplay1.Click += new System.EventHandler(this.ledDisplay1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1168, 562);
            this.Controls.Add(this.hideButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.scrollButton);
            this.Controls.Add(this.clockButton);
            this.Controls.Add(this.ledDisplay1);
            this.Controls.Add(this.designButton);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.ledDisplay1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button designButton;
        private LedControl.LedDisplay ledDisplay1;
        private System.Windows.Forms.Button clockButton;
        private System.Windows.Forms.Button scrollButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button hideButton;
    }
}

