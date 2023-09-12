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
    public partial class TextEditor : Form
    {
        /// <summary>
        /// 输入结束后操作的委托类型
        /// </summary>
        public delegate void TextEditorDelegate();

        private File editFile;  //正在编辑的文本文件
        private bool ifEdit;    //本次编辑是否发生了更改操作
        public TextEditorDelegate callBack;  //编辑结束后操作的委托

        /// <summary>
        /// 创建一个文本编辑窗口
        /// </summary>
        /// <param name="editFile"> 待编辑的文本文件 </param>
        public TextEditor(ref File editFile)
        {
            InitializeComponent();

            this.ifEdit = false;  //未发生更改操作
            this.editFile = editFile;  //初始化正在编辑的文本文件
            this.labelFileName.Text = editFile.Name + File.suffixNames[(int)editFile.Type];  //初始化文件名标签
            this.richTextBox.LanguageOption = RichTextBoxLanguageOptions.UIFonts;  //指定新输入的字符与用户界面的字体一致
            this.richTextBoxFontSize.LanguageOption = RichTextBoxLanguageOptions.UIFonts;  //指定新输入的字符与用户界面的字体一致
            this.richTextBox.Text = editFile.Read(MainWindow.disk);  //从磁盘空间读出文件的数据，并显示在编辑窗口中
            this.labelCurrentCharInfo.Text = editFile.Size.ToString() + " 个字符";  //显示当前文件中的字符数量
        }

        /// <summary>
        /// 输入框文本发生改变时，同步显示当前文件中的字符数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateCurrentCharInfo(object sender, EventArgs e)
        {
            this.ifEdit = true;  //发生了更改操作，标志设为真
            this.labelCurrentCharInfo.Text = this.richTextBox.Text.Length.ToString() + " 个字符";  //同步显示字符数量
        }

        /// <summary>
        /// 按下“保存”键，完成编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveFile(object sender, EventArgs e)
        {
            if (this.ifEdit)  //若发生了更改，则弹出消息框进行二次确认
            {
                DialogResult result = MessageBox.Show("确定保存吗？",
                        "Save File",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Cancel)  //若点击取消则不做任何操作
                    return;
            }

            else  //若未发生更改则直接关闭窗口
                Close();

            bool writeResult = this.editFile.Write(ref MainWindow.disk, this.richTextBox.Text);  //尝试向文件中写入数据

            if (!writeResult)  //若写入失败，则弹出消息框告知磁盘已满
            {
                MessageBox.Show("磁盘空间已满，写入失败！",
                    "Disk Full Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);

                return;
            }

            else if (this.callBack != null)  //若委托不为空，则调用委托挂接的方法
                this.callBack();

            Close();  //关闭窗口
        }

        /// <summary>
        /// 按下“取消”键，取消编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelEdit(object sender, EventArgs e)
        {
            if (this.ifEdit)  //若发生了更改，则弹出消息框进行二次确认
            {
                DialogResult result = MessageBox.Show("确定放弃更改吗？",
                        "Cancel Edit",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                if (result == DialogResult.OK)  //若点击确定则关闭窗口
                    Close();
            }

            else  //若未发生更改则直接关闭窗口
                Close();
        }

        /// <summary>
        /// 设置文本编辑窗口的字号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetEditorFontSize(object sender, EventArgs e)
        {
            int fontSize = 12;

            try
            {
                fontSize = Convert.ToInt32(this.richTextBoxFontSize.Text);
            }
            catch
            {
                this.richTextBox.Font = new Font("微软雅黑", 12);
            }
            

            if (fontSize > 0 && fontSize <= 72)
                this.richTextBox.Font = new Font("微软雅黑", fontSize);
            else
                this.richTextBox.Font = new Font("微软雅黑", 12);
        }
    }
}
