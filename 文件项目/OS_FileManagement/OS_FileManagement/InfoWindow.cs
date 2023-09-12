using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS_FileManagement
{
    public partial class InfoWindow : Form
    {
        /// <summary>
        /// 创建属性窗口
        /// </summary>
        /// <param name="node"> 待显示属性的文件结点 </param>
        public InfoWindow(FileNode node)
        {
            InitializeComponent();

            if (node.Type == FileNode.FileNodeType.Folder)  //文件结点是文件夹
            {
                this.Text = "About " + node.Name;     //设置窗口标题
                this.labelNameData.Text = node.Name;  //显示文件名
                this.labelTypeData.Text = "文件夹";   //显示文件结点类型
                this.richTextBoxPathData.Text = node.folder.Path;  //显示路径
                this.labelSizeData.Text = FileTools.DiscribeSize(node.folder.Size);  //处理大小并显示
                this.labelSizeData.Font = new Font("微软雅黑", 12);
                this.labelChildData.Text = node.folder.nodeList.Count.ToString();     //显示子项数量
                this.labelCreatedTimeData.Text = node.folder.CreatedTime.ToString();  //显示创建时间
                this.labelUpdatedTimeData.Text = node.folder.UpdatedTime.ToString();  //显示修改时间
                this.pictureBox.Image = this.imageList.Images[0];  //设置图标
            }

            else  //文件结点是文件
            {
                this.Text = "About " + node.Name + File.suffixNames[(int)node.file.Type];     //设置窗口标题
                this.labelNameData.Text = node.Name + File.suffixNames[(int)node.file.Type];  //显示文件名
                this.labelTypeData.Text = node.file.Type.ToString() + "文件";  //显示文件结点类型
                this.richTextBoxPathData.Text = node.file.Path + File.suffixNames[(int)node.file.Type];  //显示路径
                this.labelSizeData.Text = FileTools.DiscribeSize(node.file.Size);  //处理大小并显示
                this.labelSizeData.Font = new Font("微软雅黑", 12);
                this.labelChildData.Text = "-";  //显示子项数量
                this.labelCreatedTimeData.Text = node.file.CreatedTime.ToString();  //显示创建时间
                this.labelUpdatedTimeData.Text = node.file.UpdatedTime.ToString();  //显示修改时间
                this.pictureBox.Image = this.imageList.Images[(int)(node.file.Type + 1)];  //设置图标
            }
        }

        /// <summary>
        /// 创建根文件夹的属性窗口
        /// </summary>
        /// <param name="root"> 根文件夹 </param>
        public InfoWindow(Folder root)
        {
            if (root != MainWindow.RootFolder)  //若参数不等于根文件夹，则不做任何操作
                return;

            InitializeComponent();

            this.labelNameData.Text = root.Name;  //显示根文件夹名
            this.labelTypeData.Text = "文件夹（根）";  //显示根文件夹类型
            this.richTextBoxPathData.Text = root.Path;  //显示根文件夹路径

            this.labelSizeData.Text = FileTools.DiscribeSize(root.Size);  //处理根文件夹的大小并显示
            this.labelSizeData.Text += " (块占用:" + MainWindow.disk.CapturedBlockCount + '/' + MainWindow.disk.DiskSize + ')';  //显示根文件夹块占用情况
            
            this.labelSizeData.Font = new Font("微软雅黑", 8);
            this.labelChildData.Text = root.nodeList.Count.ToString();  //显示根文件夹子结点数
            this.labelCreatedTimeData.Text = root.CreatedTime.ToString();  //显示根文件夹创建时间
            this.labelUpdatedTimeData.Text = root.UpdatedTime.ToString();  //显示根文件夹修改时间
            this.pictureBox.Image = this.imageList.Images[0];  //设置根文件夹图标
        }

        /// <summary>
        /// 按下确认按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
