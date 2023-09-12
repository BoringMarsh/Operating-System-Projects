
namespace OS_FileManagement
{
    partial class WelcomeWindow
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
            this.labelWelcome = new System.Windows.Forms.Label();
            this.labelQuery = new System.Windows.Forms.Label();
            this.buttonCreateNewRoot = new System.Windows.Forms.Button();
            this.buttonReadFromFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelWelcome
            // 
            this.labelWelcome.BackColor = System.Drawing.Color.LightCyan;
            this.labelWelcome.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelWelcome.Location = new System.Drawing.Point(141, 72);
            this.labelWelcome.Name = "labelWelcome";
            this.labelWelcome.Size = new System.Drawing.Size(518, 80);
            this.labelWelcome.TabIndex = 0;
            this.labelWelcome.Text = "欢迎使用文件管理系统";
            this.labelWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelQuery
            // 
            this.labelQuery.BackColor = System.Drawing.Color.LightCyan;
            this.labelQuery.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelQuery.Location = new System.Drawing.Point(141, 171);
            this.labelQuery.Name = "labelQuery";
            this.labelQuery.Size = new System.Drawing.Size(518, 80);
            this.labelQuery.TabIndex = 1;
            this.labelQuery.Text = "请选择初始化方式：";
            this.labelQuery.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonCreateNewRoot
            // 
            this.buttonCreateNewRoot.BackColor = System.Drawing.Color.MintCream;
            this.buttonCreateNewRoot.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonCreateNewRoot.Location = new System.Drawing.Point(141, 296);
            this.buttonCreateNewRoot.Name = "buttonCreateNewRoot";
            this.buttonCreateNewRoot.Size = new System.Drawing.Size(202, 104);
            this.buttonCreateNewRoot.TabIndex = 2;
            this.buttonCreateNewRoot.Text = "新建根目录";
            this.buttonCreateNewRoot.UseVisualStyleBackColor = false;
            this.buttonCreateNewRoot.Click += new System.EventHandler(this.buttonCreateNewRoot_Click);
            // 
            // buttonReadFromFile
            // 
            this.buttonReadFromFile.BackColor = System.Drawing.Color.MintCream;
            this.buttonReadFromFile.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonReadFromFile.Location = new System.Drawing.Point(457, 296);
            this.buttonReadFromFile.Name = "buttonReadFromFile";
            this.buttonReadFromFile.Size = new System.Drawing.Size(202, 104);
            this.buttonReadFromFile.TabIndex = 3;
            this.buttonReadFromFile.Text = "从文件读取";
            this.buttonReadFromFile.UseVisualStyleBackColor = false;
            this.buttonReadFromFile.Click += new System.EventHandler(this.buttonReadFromFile_Click);
            // 
            // WelcomeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleTurquoise;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonReadFromFile);
            this.Controls.Add(this.buttonCreateNewRoot);
            this.Controls.Add(this.labelQuery);
            this.Controls.Add(this.labelWelcome);
            this.Name = "WelcomeWindow";
            this.Text = "WelcomeForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WelcomeForm_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelWelcome;
        private System.Windows.Forms.Label labelQuery;
        private System.Windows.Forms.Button buttonCreateNewRoot;
        private System.Windows.Forms.Button buttonReadFromFile;
    }
}