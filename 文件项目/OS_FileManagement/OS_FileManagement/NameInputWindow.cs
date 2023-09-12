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
    public partial class NameInputWindow : Form
    {
        /// <summary>
        /// 输入名称的目的操作种类
        /// </summary>
        public enum OperatonType
        {
            Create,
            Rename
        }

        /// <summary>
        /// 输入结束后操作的委托类型
        /// </summary>
        public delegate void NameInputWindowDelegate();

        private Folder curFolder;     //待操作文件结点所在的文件夹
        private FileNode renameNode;  //待改名的文件结点
        private FileNode.FileNodeType fileNodeType;  //待操作文件结点的类型
        private File.FileType? fileType;    //待操作文件结点的文件类型（若是文件夹则为空）
        private OperatonType operatonType;  //本次操作类型
        public NameInputWindowDelegate callBack;  //输入结束后操作的委托

        /// <summary>
        /// 创建一个名称输入窗口
        /// </summary>
        /// <param name="curFolder"> 待操作文件结点所在的文件夹 </param>
        /// <param name="fileType"> 待操作文件结点的文件类型（若是文件夹则为空） </param>
        /// <param name="renameNode"> 待改名的文件结点（若不是改名则为空） </param>
        public NameInputWindow(ref Folder curFolder, File.FileType? fileType, FileNode renameNode = null)
        {
            InitializeComponent();

            if (renameNode == null)  //若待改名的文件结点为空
            {
                this.operatonType = OperatonType.Create;  //本次操作是给新建文件命名

                if (fileType.HasValue)  //新建的是文件
                    this.Text = "Create New File:" + fileType.Value.ToString();
                else    //新建的是文件夹
                    this.Text = "Create New Folder";
            }

            else  //若待改名的文件结点不为空
            {
                this.operatonType = OperatonType.Rename;  //本次操作是给现有文件改名

                if (fileType.HasValue)  //改名的是文件
                    this.Text = "Rename A " + fileType.Value.ToString() + " File";
                else    //改名的是文件夹
                    this.Text = "Rename A Folder";
            }

            if (fileType.HasValue)
                this.fileNodeType = FileNode.FileNodeType.File;    //文件类型不为空，结点类型为文件
            else
                this.fileNodeType = FileNode.FileNodeType.Folder;  //文件类型为空，结点类型为文件

            this.curFolder = curFolder;    //初始化所在文件夹
            this.fileType = fileType;      //初始化文件类型
            this.renameNode = renameNode;  //初始化待改名的文件结点
        }

        /// <summary>
        /// 检查testName是否为以下形式：compareName(数字)，并返回该数字。
        /// 若不符合或数字为负则返回-1，testName变为compareName
        /// </summary>
        /// <param name="compareName"> 作比较的名字 </param>
        /// <param name="testName"> 待检测的名字 </param>
        /// <param name="ifTrimTestName"> 若符合该形式，是否在检测完成后把testName修改为compareName </param>
        /// <returns> int：括号内的数字 </returns>
        public static int SubNameCheck(string compareName, ref string testName, bool ifTrimTestName)
        {
            if (compareName == testName)  //若两者相等，相当于testName形式为compareName(1)，返回1
                return 1;

            if (compareName.Length >= testName.Length)  //若compareName更长，则肯定不符合
                return -1;

            string checkName = testName.Remove(0, compareName.Length);  //将testName截去前compareName.Length个字符

            if (checkName[0] != '(')  //若接下来第一个字符不是左括号，则不符合
                return -1;

            int subIndex = 0;  //返回值，初始为0

            for (int i = 1; i < checkName.Length - 1; i++)  //逐个检查checkName掐头去尾的其它字符
            {
                if (checkName[i] >= '0' && checkName[i] <= '9')  //若是数字，则更改返回值
                    subIndex = subIndex * 10 + (int)(checkName[i] - '0');
                else  //若存在不是数字的字符，则不符合
                    return -1;
            }

            if (ifTrimTestName)  //按要求修改testName
                testName = compareName;

            if (checkName[checkName.Length - 1] != ')')  //若最后一个字符不是右括号，则不符合
                return -1;
            else  //否则符合，返回括号内的数字
                return subIndex;
        }

        /// <summary>
        /// 检查新建的名字是否有重名，并在改变文件夹名字表后，返回最终名字
        /// </summary>
        /// <param name="originalName"> 初始输入的名字 </param>
        /// <param name="originalNodeType"> 操作文件结点的结点类型 </param>
        /// <param name="originalFileType"> 操作文件结点的文件类型（若是文件夹则为空） </param>
        /// <returns> 经过检查后得出的名字 </returns>
        private string CheckCreateName(string originalName, FileNode.FileNodeType nodeType, File.FileType? fileType)
        {
            Dictionary<NameTypePair, LinkedList<int>> nameList = this.curFolder.NameList;  //取所在文件夹的名字字典

            foreach (var pair in nameList)
            {
                string resultName = originalName;  //返回值，初始化为originalName

                if (pair.Key.nodeType != nodeType)  //该结点类型不符，检查下一键值对
                    continue;

                else if (pair.Key.fileType.HasValue && fileType.HasValue && pair.Key.fileType.Value != fileType.Value)  //该结点文件类型不符，检查下一键值对
                    continue;

                //输入的值正好与键的名称相等，则看链表
                if (pair.Key.name == resultName)
                {
                    LinkedListNode<int> curNode = pair.Value.First;

                    if (curNode.Value != 1)  //链表第一项不是1
                    {
                        pair.Value.AddBefore(curNode, 1);
                        return resultName;
                    }

                    else if (curNode.Next == null)  //链表第一项是1，但长度仅为1
                    {
                        pair.Value.AddAfter(curNode, 2);
                        resultName += "(2)";
                        return resultName;
                    }
                    
                    while (curNode != null)  //检查链表中间标号不连续的位置
                    {
                        if (curNode.Next != null && curNode.Next.Value - curNode.Value != 1)
                        {
                            pair.Value.AddAfter(curNode, curNode.Value + 1);
                            resultName += '(' + (curNode.Value + 1).ToString() + ')';
                            return resultName;
                        }

                        curNode = curNode.Next;
                    }
                }

                //输入的值可能是“守株待兔”式，做进一步检查
                else
                {
                    int subIndex = SubNameCheck(pair.Key.name, ref resultName, true);

                    if (subIndex == -1)
                        continue;

                    LinkedListNode<int> curNode = pair.Value.First;

                    //链表中不存在该值且取得编号不为0
                    if (!pair.Value.Contains(subIndex) && subIndex != 0)
                    {
                        //若第一个位置就比值大，则追加到头部
                        if (curNode.Value > subIndex)
                        {
                            pair.Value.AddFirst(subIndex);

                            if (subIndex != 1)
                                resultName += '(' + subIndex.ToString() + ')';
                            return resultName;
                        }

                        //找最后一个小于等于取得编号的位置插入
                        while (curNode != null)
                        {
                            if (curNode.Next != null && curNode.Next.Value > subIndex)
                            {
                                pair.Value.AddAfter(curNode, subIndex);
                                resultName += '(' + subIndex.ToString() + ')';
                                return resultName;
                            }

                            curNode = curNode.Next;
                        }

                        //没有则追加到尾部
                        pair.Value.AddLast(subIndex);
                        resultName += '(' + subIndex.ToString() + ')';
                        return resultName;
                    }

                    //链表中存在该值或值为0
                    else
                    {
                        while (curNode != null)  //找第一个不连续的位置插入，没有则追加到尾部
                        {
                            if (curNode.Next != null && curNode.Next.Value - curNode.Value != 1)
                            {
                                pair.Value.AddAfter(curNode, curNode.Value + 1);
                                resultName += '(' + (curNode.Value + 1).ToString() + ')';
                                return resultName;
                            }

                            curNode = curNode.Next;
                        }
                    }
                }

                //上两种情况都未满足，则往尾部添加
                pair.Value.AddLast(pair.Value.Count + 1);
                resultName += '(' + (pair.Value.Count).ToString() + ')';
                return resultName;
            }

            //运行到此，说明originalName不存在重名情况，创建新的键值对
            LinkedList<int> newValue = new LinkedList<int>();  //新建链表并加入1
            newValue.AddLast(1);

            if (fileType.HasValue)  //根据文件结点类型创建键对象，并将新的键值对加入字典
                nameList.Add(new NameTypePair(originalName, nodeType, fileType.Value), newValue);
            else
                nameList.Add(new NameTypePair(originalName, nodeType, null), newValue);

            return originalName;
        }

        /// <summary>
        /// 检查重命名后的名字是否已存在，若存在则返回null，若不存在则改变文件夹名字表后返回该名字
        /// </summary>
        private string CheckRenameName(string originalName, string newName, FileNode.FileNodeType nodeType, File.FileType? fileType)
        {
            if (originalName == newName)  //如果新名字与原名字完全相同，则返回空
                return null;

            int newResult = 0, originalResult = 0;  //新旧名字的比对结果，初始为0
            string resultName = newName;  //返回值，初始化为newName
            NameTypePair existingPair = new NameTypePair();

            foreach (var pair in this.curFolder.NameList)
            {
                newResult = SubNameCheck(pair.Key.name, ref newName, false);  //将新名字与键中的名字进行比对
                originalResult = SubNameCheck(pair.Key.name, ref originalName, false);  //将原名字与键中的名字进行比对

                if (originalResult != -1)
                    existingPair = new NameTypePair(pair.Key);

                if (newResult == -1)  //若不符合，则检查下一键值对
                    continue;
                else  //若符合则已存在，返回空
                    return null;
            }

            //运行到此，说明newName不存在重名情况，先将原来的名字从字典中去掉
            LinkedList<int> valueList;
            this.curFolder.NameList.TryGetValue(existingPair, out valueList);
            valueList.Remove(originalResult);

            //若删除后链表为空，则从字典中删除该键值对
            if (valueList.Count == 0)
                this.curFolder.NameList.Remove(existingPair);

            //创建新的键值对
            LinkedList<int> list = new LinkedList<int>();  //新建链表并加入1
            list.AddLast(1);

            //根据文件结点类型创建键对象，并将新的键值对加入字典
            this.curFolder.NameList.Add(new NameTypePair(resultName, nodeType, fileType), list);
            return resultName;
        }

        /// <summary>
        /// 点击“确定”键完成输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputName(object sender, EventArgs e)
        {
            bool inputSuccess = false;  //输入是否成功的标志

            if (this.operatonType == OperatonType.Create)  //若当前操作是为新建文件命名
            {
                if (this.textBox.Text != "")  //若输入文本不为空
                {
                    string resultName = CheckCreateName(this.textBox.Text, this.fileNodeType, this.fileType);  //将文本框里的输入内容经过检查后返回合法的名字
                    this.curFolder.AddChild(this.fileNodeType, this.fileType, resultName);  //用这个名字创建新的子结点
                    inputSuccess = true;  //输入成功
                }

                else  //若输入文本为空，则弹出消息框提示
                {
                    MessageBox.Show("名称不能为空！",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                }
            }

            else  //若当前操作是为现有文件改名
            {
                if (this.textBox.Text != "")  //若输入文本不为空
                {
                    string resultName = CheckRenameName(this.renameNode.Name, this.textBox.Text, this.fileNodeType, this.fileType);  //将文本框里的输入内容进行检查

                    if (resultName != null)  //若名字合法，则更名成功
                    {
                        this.renameNode.Name = resultName;  //给结点更名

                        if (!this.fileType.HasValue)  //若该结点为文件夹，则改变其所有子结点的路径
                            FileTools.RenameFolderPath(this.renameNode.folder);

                        inputSuccess = true;  //输入成功
                    }

                    else  //若名字已存在，则提示名字已存在
                    {
                        MessageBox.Show("名字已存在！",
                        "New Name Repeated",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                    }
                }

                else  //若输入文本为空，则弹出消息框提示
                {
                    MessageBox.Show("名称不能为空！",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                }
            }

            if (inputSuccess && this.callBack != null)  //若输入成功且委托不为空，则调用委托挂接的方法
                this.callBack();

            if (inputSuccess)  //若输入成功则关闭窗口
                Close();
        }
    }

    /// <summary>
    /// 名字和文件种类的结构体，用来组成文件夹名字字典的键
    /// </summary>
    [Serializable]
    public struct NameTypePair
    {
        public string name;  //名字
        public FileNode.FileNodeType nodeType;  //文件结点种类
        public File.FileType? fileType;  //文件种类（若是文件夹则为空）

        /// <summary>
        /// 用现有的结构体创建一个NameTypePair实例
        /// </summary>
        /// <param name="pair"> 现有的NameTypePair结构体 </param>
        public NameTypePair(NameTypePair pair)
        {
            this.name = pair.name;
            this.nodeType = pair.nodeType;
            this.fileType = pair.fileType;
        }

        /// <summary>
        /// 用相关信息创建一个NameTypePair实例
        /// </summary>
        /// <param name="name"> 名字 </param>
        /// <param name="nodeType"> 文件结点种类 </param>
        /// <param name="fileType"> 文件种类（若是文件夹则为空） </param>
        public NameTypePair(string name, FileNode.FileNodeType nodeType, File.FileType? fileType)
        {
            this.name = name;
            this.nodeType = nodeType;
            this.fileType = fileType;
        }
    }
}
