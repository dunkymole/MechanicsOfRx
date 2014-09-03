namespace Demo
{
    partial class Form
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
            this.WordCount = new System.Windows.Forms.NumericUpDown();
            this.NaugtyNumbers = new System.Windows.Forms.ListBox();
            this.Go = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.Concurrency = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.WordCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Concurrency)).BeginInit();
            this.SuspendLayout();
            // 
            // WordCount
            // 
            this.WordCount.Location = new System.Drawing.Point(106, 11);
            this.WordCount.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.WordCount.Name = "WordCount";
            this.WordCount.Size = new System.Drawing.Size(86, 20);
            this.WordCount.TabIndex = 0;
            this.WordCount.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // NaugtyNumbers
            // 
            this.NaugtyNumbers.FormattingEnabled = true;
            this.NaugtyNumbers.Location = new System.Drawing.Point(13, 138);
            this.NaugtyNumbers.Name = "NaugtyNumbers";
            this.NaugtyNumbers.Size = new System.Drawing.Size(179, 95);
            this.NaugtyNumbers.TabIndex = 1;
            // 
            // Go
            // 
            this.Go.Location = new System.Drawing.Point(13, 80);
            this.Go.Name = "Go";
            this.Go.Size = new System.Drawing.Size(179, 23);
            this.Go.TabIndex = 2;
            this.Go.Text = "Go";
            this.Go.UseVisualStyleBackColor = true;
            // 
            // Stop
            // 
            this.Stop.Location = new System.Drawing.Point(13, 109);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(179, 23);
            this.Stop.TabIndex = 3;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            // 
            // Concurrency
            // 
            this.Concurrency.Location = new System.Drawing.Point(106, 35);
            this.Concurrency.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Concurrency.Name = "Concurrency";
            this.Concurrency.Size = new System.Drawing.Size(86, 20);
            this.Concurrency.TabIndex = 4;
            this.Concurrency.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Numbers To Check";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Max Concurrency";
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 253);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Concurrency);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.Go);
            this.Controls.Add(this.NaugtyNumbers);
            this.Controls.Add(this.WordCount);
            this.Name = "Form";
            this.Text = "Law Of Four";
            ((System.ComponentModel.ISupportInitialize)(this.WordCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Concurrency)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown WordCount;
        private System.Windows.Forms.ListBox NaugtyNumbers;
        private System.Windows.Forms.Button Go;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.NumericUpDown Concurrency;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

