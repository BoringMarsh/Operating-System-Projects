
namespace OS_FileManagement
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuTop = new System.Windows.Forms.MenuStrip();
            this.menuTop_View = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTop_View_Large = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTop_View_Small = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTop_View_List = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTop_View_Details = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTop_Reset = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTop_About = new System.Windows.Forms.ToolStripMenuItem();
            this.menuListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuListView_View = new System.Windows.Forms.ToolStripMenuItem();
            this.menuListView_View_Large = new System.Windows.Forms.ToolStripMenuItem();
            this.menuListView_View_Small = new System.Windows.Forms.ToolStripMenuItem();
            this.menuListView_View_List = new System.Windows.Forms.ToolStripMenuItem();
            this.menuListView_View_Details = new System.Windows.Forms.ToolStripMenuItem();
            this.menuListView_Refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuListView_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.menuListView_Create = new System.Windows.Forms.ToolStripMenuItem();
            this.menuListView_Create_Folder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuListView_Create_File = new System.Windows.Forms.ToolStripMenuItem();
            this.menuListView_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuListView_Rename = new System.Windows.Forms.ToolStripMenuItem();
            this.menuListView_Property = new System.Windows.Forms.ToolStripMenuItem();
            this.currentPathDataLabel = new System.Windows.Forms.Label();
            this.fileTreeView = new System.Windows.Forms.TreeView();
            this.imageListMicro = new System.Windows.Forms.ImageList(this.components);
            this.fileListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageListLarge = new System.Windows.Forms.ImageList(this.components);
            this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.buttonPathBackward = new System.Windows.Forms.Button();
            this.buttonPathForward = new System.Windows.Forms.Button();
            this.curFolderInfoLabel = new System.Windows.Forms.Label();
            this.menuTop.SuspendLayout();
            this.menuListView.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuTop
            // 
            this.menuTop.AutoSize = false;
            this.menuTop.BackColor = System.Drawing.Color.SkyBlue;
            this.menuTop.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuTop.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTop_View,
            this.menuTop_Reset,
            this.menuTop_About});
            this.menuTop.Location = new System.Drawing.Point(0, 0);
            this.menuTop.MaximumSize = new System.Drawing.Size(1280, 200);
            this.menuTop.Name = "menuTop";
            this.menuTop.Size = new System.Drawing.Size(1273, 69);
            this.menuTop.TabIndex = 0;
            this.menuTop.Text = "menuStrip1";
            // 
            // menuTop_View
            // 
            this.menuTop_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTop_View_Large,
            this.menuTop_View_Small,
            this.menuTop_View_List,
            this.menuTop_View_Details});
            this.menuTop_View.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.menuTop_View.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.menuTop_View.Name = "menuTop_View";
            this.menuTop_View.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.menuTop_View.Size = new System.Drawing.Size(86, 65);
            this.menuTop_View.Text = "查看";
            // 
            // menuTop_View_Large
            // 
            this.menuTop_View_Large.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuTop_View_Large.Image = ((System.Drawing.Image)(resources.GetObject("menuTop_View_Large.Image")));
            this.menuTop_View_Large.Name = "menuTop_View_Large";
            this.menuTop_View_Large.Size = new System.Drawing.Size(186, 34);
            this.menuTop_View_Large.Text = "大图标";
            this.menuTop_View_Large.Click += new System.EventHandler(this.SetListViewLarge);
            // 
            // menuTop_View_Small
            // 
            this.menuTop_View_Small.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuTop_View_Small.Image = ((System.Drawing.Image)(resources.GetObject("menuTop_View_Small.Image")));
            this.menuTop_View_Small.Name = "menuTop_View_Small";
            this.menuTop_View_Small.Size = new System.Drawing.Size(186, 34);
            this.menuTop_View_Small.Text = "小图标";
            this.menuTop_View_Small.Click += new System.EventHandler(this.SetListViewSmall);
            // 
            // menuTop_View_List
            // 
            this.menuTop_View_List.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuTop_View_List.Image = ((System.Drawing.Image)(resources.GetObject("menuTop_View_List.Image")));
            this.menuTop_View_List.Name = "menuTop_View_List";
            this.menuTop_View_List.Size = new System.Drawing.Size(186, 34);
            this.menuTop_View_List.Text = "列表";
            this.menuTop_View_List.Click += new System.EventHandler(this.SetListViewList);
            // 
            // menuTop_View_Details
            // 
            this.menuTop_View_Details.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuTop_View_Details.Image = ((System.Drawing.Image)(resources.GetObject("menuTop_View_Details.Image")));
            this.menuTop_View_Details.Name = "menuTop_View_Details";
            this.menuTop_View_Details.Size = new System.Drawing.Size(186, 34);
            this.menuTop_View_Details.Text = "详细信息";
            this.menuTop_View_Details.Click += new System.EventHandler(this.SetListViewDetails);
            // 
            // menuTop_Reset
            // 
            this.menuTop_Reset.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.menuTop_Reset.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.menuTop_Reset.Name = "menuTop_Reset";
            this.menuTop_Reset.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.menuTop_Reset.Size = new System.Drawing.Size(162, 65);
            this.menuTop_Reset.Text = "格式化磁盘";
            this.menuTop_Reset.Click += new System.EventHandler(this.ResetDisk);
            // 
            // menuTop_About
            // 
            this.menuTop_About.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.menuTop_About.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.menuTop_About.Name = "menuTop_About";
            this.menuTop_About.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.menuTop_About.Size = new System.Drawing.Size(86, 65);
            this.menuTop_About.Text = "关于";
            this.menuTop_About.Click += new System.EventHandler(this.ShowAbout);
            // 
            // menuListView
            // 
            this.menuListView.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuListView_View,
            this.menuListView_Refresh,
            this.menuListView_Open,
            this.menuListView_Create,
            this.menuListView_Delete,
            this.menuListView_Rename,
            this.menuListView_Property});
            this.menuListView.Name = "contextMenuStrip1";
            this.menuListView.Size = new System.Drawing.Size(143, 228);
            // 
            // menuListView_View
            // 
            this.menuListView_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuListView_View_Large,
            this.menuListView_View_Small,
            this.menuListView_View_List,
            this.menuListView_View_Details});
            this.menuListView_View.Image = ((System.Drawing.Image)(resources.GetObject("menuListView_View.Image")));
            this.menuListView_View.Name = "menuListView_View";
            this.menuListView_View.Size = new System.Drawing.Size(142, 32);
            this.menuListView_View.Text = "查看";
            // 
            // menuListView_View_Large
            // 
            this.menuListView_View_Large.Image = ((System.Drawing.Image)(resources.GetObject("menuListView_View_Large.Image")));
            this.menuListView_View_Large.Name = "menuListView_View_Large";
            this.menuListView_View_Large.Size = new System.Drawing.Size(182, 34);
            this.menuListView_View_Large.Text = "大图标";
            this.menuListView_View_Large.Click += new System.EventHandler(this.SetListViewLarge);
            // 
            // menuListView_View_Small
            // 
            this.menuListView_View_Small.Image = ((System.Drawing.Image)(resources.GetObject("menuListView_View_Small.Image")));
            this.menuListView_View_Small.Name = "menuListView_View_Small";
            this.menuListView_View_Small.Size = new System.Drawing.Size(182, 34);
            this.menuListView_View_Small.Text = "小图标";
            this.menuListView_View_Small.Click += new System.EventHandler(this.SetListViewSmall);
            // 
            // menuListView_View_List
            // 
            this.menuListView_View_List.Image = ((System.Drawing.Image)(resources.GetObject("menuListView_View_List.Image")));
            this.menuListView_View_List.Name = "menuListView_View_List";
            this.menuListView_View_List.Size = new System.Drawing.Size(182, 34);
            this.menuListView_View_List.Text = "列表";
            this.menuListView_View_List.Click += new System.EventHandler(this.SetListViewList);
            // 
            // menuListView_View_Details
            // 
            this.menuListView_View_Details.Image = ((System.Drawing.Image)(resources.GetObject("menuListView_View_Details.Image")));
            this.menuListView_View_Details.Name = "menuListView_View_Details";
            this.menuListView_View_Details.Size = new System.Drawing.Size(182, 34);
            this.menuListView_View_Details.Text = "详细信息";
            this.menuListView_View_Details.Click += new System.EventHandler(this.SetListViewDetails);
            // 
            // menuListView_Refresh
            // 
            this.menuListView_Refresh.Image = ((System.Drawing.Image)(resources.GetObject("menuListView_Refresh.Image")));
            this.menuListView_Refresh.Name = "menuListView_Refresh";
            this.menuListView_Refresh.Size = new System.Drawing.Size(142, 32);
            this.menuListView_Refresh.Text = "刷新";
            this.menuListView_Refresh.Click += new System.EventHandler(this.RefreshFromMenuList);
            // 
            // menuListView_Open
            // 
            this.menuListView_Open.Image = ((System.Drawing.Image)(resources.GetObject("menuListView_Open.Image")));
            this.menuListView_Open.Name = "menuListView_Open";
            this.menuListView_Open.Size = new System.Drawing.Size(142, 32);
            this.menuListView_Open.Text = "打开";
            this.menuListView_Open.Click += new System.EventHandler(this.OpenFromListView);
            // 
            // menuListView_Create
            // 
            this.menuListView_Create.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuListView_Create_Folder,
            this.menuListView_Create_File});
            this.menuListView_Create.Image = ((System.Drawing.Image)(resources.GetObject("menuListView_Create.Image")));
            this.menuListView_Create.Name = "menuListView_Create";
            this.menuListView_Create.Size = new System.Drawing.Size(142, 32);
            this.menuListView_Create.Text = "新建";
            this.menuListView_Create.Click += new System.EventHandler(this.CreateFolderInListView);
            // 
            // menuListView_Create_Folder
            // 
            this.menuListView_Create_Folder.Image = ((System.Drawing.Image)(resources.GetObject("menuListView_Create_Folder.Image")));
            this.menuListView_Create_Folder.Name = "menuListView_Create_Folder";
            this.menuListView_Create_Folder.Size = new System.Drawing.Size(182, 34);
            this.menuListView_Create_Folder.Text = "文件夹";
            this.menuListView_Create_Folder.Click += new System.EventHandler(this.CreateFolderInListView);
            // 
            // menuListView_Create_File
            // 
            this.menuListView_Create_File.Image = ((System.Drawing.Image)(resources.GetObject("menuListView_Create_File.Image")));
            this.menuListView_Create_File.Name = "menuListView_Create_File";
            this.menuListView_Create_File.Size = new System.Drawing.Size(182, 34);
            this.menuListView_Create_File.Text = "文本文件";
            this.menuListView_Create_File.Click += new System.EventHandler(this.CreateFileInListView);
            // 
            // menuListView_Delete
            // 
            this.menuListView_Delete.Image = ((System.Drawing.Image)(resources.GetObject("menuListView_Delete.Image")));
            this.menuListView_Delete.Name = "menuListView_Delete";
            this.menuListView_Delete.Size = new System.Drawing.Size(142, 32);
            this.menuListView_Delete.Text = "删除";
            this.menuListView_Delete.Click += new System.EventHandler(this.DeleteFromMenuList);
            // 
            // menuListView_Rename
            // 
            this.menuListView_Rename.Image = ((System.Drawing.Image)(resources.GetObject("menuListView_Rename.Image")));
            this.menuListView_Rename.Name = "menuListView_Rename";
            this.menuListView_Rename.Size = new System.Drawing.Size(142, 32);
            this.menuListView_Rename.Text = "重命名";
            this.menuListView_Rename.Click += new System.EventHandler(this.RenameFileNode);
            // 
            // menuListView_Property
            // 
            this.menuListView_Property.Image = ((System.Drawing.Image)(resources.GetObject("menuListView_Property.Image")));
            this.menuListView_Property.Name = "menuListView_Property";
            this.menuListView_Property.Size = new System.Drawing.Size(142, 32);
            this.menuListView_Property.Text = "属性";
            this.menuListView_Property.Click += new System.EventHandler(this.ShowFileNodeInfo);
            // 
            // currentPathDataLabel
            // 
            this.currentPathDataLabel.BackColor = System.Drawing.Color.LightSeaGreen;
            this.currentPathDataLabel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.currentPathDataLabel.ForeColor = System.Drawing.Color.White;
            this.currentPathDataLabel.Location = new System.Drawing.Point(243, 105);
            this.currentPathDataLabel.Name = "currentPathDataLabel";
            this.currentPathDataLabel.Size = new System.Drawing.Size(1001, 82);
            this.currentPathDataLabel.TabIndex = 4;
            this.currentPathDataLabel.Text = "root\\";
            this.currentPathDataLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fileTreeView
            // 
            this.fileTreeView.BackColor = System.Drawing.Color.LightCyan;
            this.fileTreeView.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fileTreeView.ImageIndex = 0;
            this.fileTreeView.ImageList = this.imageListMicro;
            this.fileTreeView.Location = new System.Drawing.Point(36, 215);
            this.fileTreeView.Name = "fileTreeView";
            this.fileTreeView.SelectedImageIndex = 0;
            this.fileTreeView.Size = new System.Drawing.Size(392, 592);
            this.fileTreeView.TabIndex = 5;
            this.fileTreeView.DoubleClick += new System.EventHandler(this.OpenFromTreeView);
            // 
            // imageListMicro
            // 
            this.imageListMicro.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMicro.ImageStream")));
            this.imageListMicro.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMicro.Images.SetKeyName(0, "文件夹(空).png");
            this.imageListMicro.Images.SetKeyName(1, "文件夹（非空）.png");
            this.imageListMicro.Images.SetKeyName(2, "文本文件(空).png");
            this.imageListMicro.Images.SetKeyName(3, "文本文件（非空）.png");
            // 
            // fileListView
            // 
            this.fileListView.BackColor = System.Drawing.Color.LightCyan;
            this.fileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.fileListView.ContextMenuStrip = this.menuListView;
            this.fileListView.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fileListView.HideSelection = false;
            this.fileListView.LargeImageList = this.imageListLarge;
            this.fileListView.Location = new System.Drawing.Point(448, 215);
            this.fileListView.Name = "fileListView";
            this.fileListView.Size = new System.Drawing.Size(796, 592);
            this.fileListView.SmallImageList = this.imageListSmall;
            this.fileListView.TabIndex = 6;
            this.fileListView.UseCompatibleStateImageBehavior = false;
            this.fileListView.View = System.Windows.Forms.View.Details;
            this.fileListView.SelectedIndexChanged += new System.EventHandler(this.ShowCurrentSelection);
            this.fileListView.DoubleClick += new System.EventHandler(this.OpenFromListView);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "名称";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "修改日期";
            this.columnHeader2.Width = 165;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "类型";
            this.columnHeader3.Width = 105;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "大小";
            this.columnHeader4.Width = 105;
            // 
            // imageListLarge
            // 
            this.imageListLarge.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLarge.ImageStream")));
            this.imageListLarge.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListLarge.Images.SetKeyName(0, "文件夹(空).png");
            this.imageListLarge.Images.SetKeyName(1, "文件夹（非空）.png");
            this.imageListLarge.Images.SetKeyName(2, "文本文件(空).png");
            this.imageListLarge.Images.SetKeyName(3, "文本文件（非空）.png");
            // 
            // imageListSmall
            // 
            this.imageListSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSmall.ImageStream")));
            this.imageListSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSmall.Images.SetKeyName(0, "文件夹(空).png");
            this.imageListSmall.Images.SetKeyName(1, "文件夹（非空）.png");
            this.imageListSmall.Images.SetKeyName(2, "文本文件(空).png");
            this.imageListSmall.Images.SetKeyName(3, "文本文件（非空）.png");
            // 
            // buttonPathBackward
            // 
            this.buttonPathBackward.BackColor = System.Drawing.SystemColors.Control;
            this.buttonPathBackward.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonPathBackward.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonPathBackward.Location = new System.Drawing.Point(36, 105);
            this.buttonPathBackward.Name = "buttonPathBackward";
            this.buttonPathBackward.Size = new System.Drawing.Size(82, 82);
            this.buttonPathBackward.TabIndex = 7;
            this.buttonPathBackward.Text = "<";
            this.buttonPathBackward.UseVisualStyleBackColor = false;
            this.buttonPathBackward.Click += new System.EventHandler(this.PathStepBackward);
            // 
            // buttonPathForward
            // 
            this.buttonPathForward.BackColor = System.Drawing.SystemColors.Control;
            this.buttonPathForward.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonPathForward.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonPathForward.Location = new System.Drawing.Point(124, 105);
            this.buttonPathForward.Name = "buttonPathForward";
            this.buttonPathForward.Size = new System.Drawing.Size(82, 82);
            this.buttonPathForward.TabIndex = 8;
            this.buttonPathForward.Text = ">";
            this.buttonPathForward.UseVisualStyleBackColor = false;
            this.buttonPathForward.Click += new System.EventHandler(this.PathStepForward);
            // 
            // curFolderInfoLabel
            // 
            this.curFolderInfoLabel.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.curFolderInfoLabel.Location = new System.Drawing.Point(36, 828);
            this.curFolderInfoLabel.Name = "curFolderInfoLabel";
            this.curFolderInfoLabel.Size = new System.Drawing.Size(392, 36);
            this.curFolderInfoLabel.TabIndex = 9;
            this.curFolderInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleTurquoise;
            this.ClientSize = new System.Drawing.Size(1273, 873);
            this.Controls.Add(this.curFolderInfoLabel);
            this.Controls.Add(this.buttonPathForward);
            this.Controls.Add(this.buttonPathBackward);
            this.Controls.Add(this.fileListView);
            this.Controls.Add(this.fileTreeView);
            this.Controls.Add(this.currentPathDataLabel);
            this.Controls.Add(this.menuTop);
            this.MainMenuStrip = this.menuTop;
            this.Name = "MainWindow";
            this.Text = "File Management Simulator v1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IO_SaveDiskFile);
            this.menuTop.ResumeLayout(false);
            this.menuTop.PerformLayout();
            this.menuListView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuTop;
        private System.Windows.Forms.ToolStripMenuItem menuTop_Reset;
        private System.Windows.Forms.ToolStripMenuItem menuTop_About;
        private System.Windows.Forms.ToolStripMenuItem menuTop_View;
        private System.Windows.Forms.ContextMenuStrip menuListView;
        private System.Windows.Forms.Label currentPathDataLabel;
        private System.Windows.Forms.TreeView fileTreeView;
        private System.Windows.Forms.ListView fileListView;
        private System.Windows.Forms.ToolStripMenuItem menuListView_View;
        private System.Windows.Forms.ToolStripMenuItem menuListView_View_Large;
        private System.Windows.Forms.ToolStripMenuItem menuListView_View_Small;
        private System.Windows.Forms.ToolStripMenuItem menuListView_View_List;
        private System.Windows.Forms.ToolStripMenuItem menuListView_View_Details;
        private System.Windows.Forms.ToolStripMenuItem menuListView_Open;
        private System.Windows.Forms.ToolStripMenuItem menuListView_Create;
        private System.Windows.Forms.ToolStripMenuItem menuListView_Create_Folder;
        private System.Windows.Forms.ToolStripMenuItem menuListView_Create_File;
        private System.Windows.Forms.ToolStripMenuItem menuListView_Delete;
        private System.Windows.Forms.ToolStripMenuItem menuListView_Rename;
        private System.Windows.Forms.ToolStripMenuItem menuListView_Property;
        private System.Windows.Forms.ToolStripMenuItem menuTop_View_Large;
        private System.Windows.Forms.ToolStripMenuItem menuTop_View_Small;
        private System.Windows.Forms.ToolStripMenuItem menuTop_View_List;
        private System.Windows.Forms.ToolStripMenuItem menuTop_View_Details;
        private System.Windows.Forms.ImageList imageListLarge;
        private System.Windows.Forms.ImageList imageListSmall;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ImageList imageListMicro;
        private System.Windows.Forms.ToolStripMenuItem menuListView_Refresh;
        private System.Windows.Forms.Button buttonPathBackward;
        private System.Windows.Forms.Button buttonPathForward;
        private System.Windows.Forms.Label curFolderInfoLabel;
    }
}