
namespace OS_FileManagement
{
    partial class TextEditor
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
            this.labelFileName = new System.Windows.Forms.Label();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelCurrentCharInfo = new System.Windows.Forms.Label();
            this.richTextBoxFontSize = new System.Windows.Forms.RichTextBox();
            this.labelFontSize = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelFileName
            // 
            this.labelFileName.BackColor = System.Drawing.Color.LightSeaGreen;
            this.labelFileName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelFileName.Location = new System.Drawing.Point(64, 35);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(197, 81);
            this.labelFileName.TabIndex = 0;
            this.labelFileName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // richTextBox
            // 
            this.richTextBox.BackColor = System.Drawing.Color.LightCyan;
            this.richTextBox.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox.Location = new System.Drawing.Point(65, 148);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox.Size = new System.Drawing.Size(572, 485);
            this.richTextBox.TabIndex = 1;
            this.richTextBox.Text = "";
            this.richTextBox.TextChanged += new System.EventHandler(this.UpdateCurrentCharInfo);
            // 
            // buttonSave
            // 
            this.buttonSave.BackColor = System.Drawing.Color.LightCyan;
            this.buttonSave.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonSave.Location = new System.Drawing.Point(114, 689);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(143, 80);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "保存";
            this.buttonSave.UseVisualStyleBackColor = false;
            this.buttonSave.Click += new System.EventHandler(this.SaveFile);
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.LightCyan;
            this.buttonCancel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonCancel.Location = new System.Drawing.Point(442, 689);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(143, 80);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.CancelEdit);
            // 
            // labelCurrentCharInfo
            // 
            this.labelCurrentCharInfo.BackColor = System.Drawing.Color.LightSeaGreen;
            this.labelCurrentCharInfo.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCurrentCharInfo.Location = new System.Drawing.Point(65, 633);
            this.labelCurrentCharInfo.Name = "labelCurrentCharInfo";
            this.labelCurrentCharInfo.Size = new System.Drawing.Size(572, 35);
            this.labelCurrentCharInfo.TabIndex = 4;
            // 
            // richTextBoxFontSize
            // 
            this.richTextBoxFontSize.BackColor = System.Drawing.Color.LightCyan;
            this.richTextBoxFontSize.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBoxFontSize.Location = new System.Drawing.Point(419, 68);
            this.richTextBoxFontSize.Name = "richTextBoxFontSize";
            this.richTextBoxFontSize.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxFontSize.Size = new System.Drawing.Size(218, 48);
            this.richTextBoxFontSize.TabIndex = 5;
            this.richTextBoxFontSize.Text = "12";
            this.richTextBoxFontSize.TextChanged += new System.EventHandler(this.SetEditorFontSize);
            // 
            // labelFontSize
            // 
            this.labelFontSize.BackColor = System.Drawing.Color.LightSeaGreen;
            this.labelFontSize.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelFontSize.Location = new System.Drawing.Point(419, 35);
            this.labelFontSize.Name = "labelFontSize";
            this.labelFontSize.Size = new System.Drawing.Size(218, 38);
            this.labelFontSize.TabIndex = 6;
            this.labelFontSize.Text = "字号";
            this.labelFontSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleTurquoise;
            this.ClientSize = new System.Drawing.Size(707, 796);
            this.Controls.Add(this.labelFontSize);
            this.Controls.Add(this.richTextBoxFontSize);
            this.Controls.Add(this.labelCurrentCharInfo);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.labelFileName);
            this.Name = "TextEditor";
            this.Text = "TextEditor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelCurrentCharInfo;
        private System.Windows.Forms.RichTextBox richTextBoxFontSize;
        private System.Windows.Forms.Label labelFontSize;
    }
}