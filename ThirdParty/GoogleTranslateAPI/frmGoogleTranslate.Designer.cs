namespace com.google.translate.api
{
    partial class frmGoogleTranslate
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
            this.txtInputText = new System.Windows.Forms.RichTextBox();
            this.txtTranslatedText = new System.Windows.Forms.RichTextBox();
            this.cmbLanguages = new System.Windows.Forms.ComboBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnTranslate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtInputText
            // 
            this.txtInputText.Location = new System.Drawing.Point(12, 40);
            this.txtInputText.Name = "txtInputText";
            this.txtInputText.Size = new System.Drawing.Size(383, 225);
            this.txtInputText.TabIndex = 0;
            this.txtInputText.Text = "";
            // 
            // txtTranslatedText
            // 
            this.txtTranslatedText.Location = new System.Drawing.Point(414, 40);
            this.txtTranslatedText.Name = "txtTranslatedText";
            this.txtTranslatedText.ReadOnly = true;
            this.txtTranslatedText.Size = new System.Drawing.Size(414, 225);
            this.txtTranslatedText.TabIndex = 1;
            this.txtTranslatedText.Text = "";
            // 
            // cmbLanguages
            // 
            this.cmbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguages.FormattingEnabled = true;
            this.cmbLanguages.Location = new System.Drawing.Point(414, 11);
            this.cmbLanguages.Name = "cmbLanguages";
            this.cmbLanguages.Size = new System.Drawing.Size(191, 23);
            this.cmbLanguages.TabIndex = 2;
            this.cmbLanguages.SelectedIndexChanged += new System.EventHandler(this.cmbLanguages_SelectedIndexChanged);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(719, 271);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(109, 33);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnTranslate
            // 
            this.btnTranslate.Enabled = false;
            this.btnTranslate.Location = new System.Drawing.Point(604, 271);
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.Size = new System.Drawing.Size(109, 33);
            this.btnTranslate.TabIndex = 4;
            this.btnTranslate.Text = "Translate";
            this.btnTranslate.UseVisualStyleBackColor = true;
            this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);
            // 
            // frmGoogleTranslate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 316);
            this.Controls.Add(this.btnTranslate);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.cmbLanguages);
            this.Controls.Add(this.txtTranslatedText);
            this.Controls.Add(this.txtInputText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGoogleTranslate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Google Translation";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtInputText;
        private System.Windows.Forms.RichTextBox txtTranslatedText;
        private System.Windows.Forms.ComboBox cmbLanguages;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnTranslate;
    }
}