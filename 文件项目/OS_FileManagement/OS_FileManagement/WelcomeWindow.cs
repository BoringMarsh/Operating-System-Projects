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
    public partial class WelcomeWindow : Form
    {
        /// <summary>
        /// 对于欢迎窗口的回应种类
        /// </summary>
        public enum WelcomeReply
        {
            /// <summary>
            /// 还未做出任何回应的状态，仅在窗口未关闭时处于
            /// </summary>
            None,
            /// <summary>
            /// 直接关闭欢迎窗口
            /// </summary>
            Close,
            /// <summary>
            /// 选择创建根目录
            /// </summary>
            CreateNewRoot,
            /// <summary>
            /// 选择从磁盘读取
            /// </summary>
            ReadFromFile
        }

        private WelcomeReply reply;  //用户对欢迎窗口的回应

        /// <summary>
        /// 获取用户对欢迎窗口的回应
        /// </summary>
        public WelcomeReply Reply
        {
            get { return this.reply; }
        }

        /// <summary>
        /// 创建一个欢迎窗口
        /// </summary>
        public WelcomeWindow()
        {
            InitializeComponent();
            this.reply = WelcomeReply.None;  //还未作出任何回应
        }

        /// <summary>
        /// 设置从文件读取键是否可用
        /// </summary>
        /// <param name="ability"></param>
        public void SetReadFromFileButton(bool ability)
        {
            this.buttonReadFromFile.Enabled = ability;
        }

        /// <summary>
        /// 按下新建根目录键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreateNewRoot_Click(object sender, EventArgs e)
        {
            this.reply = WelcomeReply.CreateNewRoot;
            Close();
        }

        /// <summary>
        /// 按下从磁盘读入键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonReadFromFile_Click(object sender, EventArgs e)
        {
            this.reply = WelcomeReply.ReadFromFile;
            Close();
        }

        /// <summary>
        /// 直接关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WelcomeForm_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.reply == WelcomeReply.None)  //若关闭时仍未作出回应，则说明是主动关闭窗口
                this.reply = WelcomeReply.Close;
        }
    }
}
