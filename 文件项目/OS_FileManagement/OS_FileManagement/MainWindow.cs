using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OS_FileManagement
{
    public partial class MainWindow : Form
    {
        public static Disk disk;   //磁盘空间对象
        public static string dir;  //程序可执行文件路径
        public static string diskFolderDir;    //装有磁盘文件的目录
        public static string diskCatalogDir;   //rootCatalog.txt路径
        public static string diskBitmapDir;    //bitMap.txt路径
        private static string rootName;        //根文件夹名
        private static TreeNode rootTreeNode;  //树形视图中的根结点
        private static FileNode rootFileNode;  //根文件结点（内含根文件夹）
        private static Folder rootFolder;      //根文件夹
        private static Folder curFolder;       //当前文件夹
        private static List<ListViewItem> listViewItems;  //当前列表视图中的表项
        private static List<Folder> pathRecord;  //记录访问过的文件夹的列表
        private static int pathPointer;  //当前文件夹在pathRecord中的位置

        /// <summary>
        /// 取根文件夹
        /// </summary>
        public static Folder RootFolder
        {
            get { return rootFolder; }
        }

        /// <summary>
        /// 取根文件夹名
        /// </summary>
        public static string RootName
        {
            get { return rootName; }
        }

        /// <summary>
        /// 初始化主窗口，并从磁盘文件中读取文件信息
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            dir = Application.StartupPath;  //取可执行文件所在目录
            diskFolderDir = System.IO.Path.Combine(dir, "diskFiles");  //取装有磁盘文件的目录
            diskCatalogDir = System.IO.Path.Combine(dir, "diskFiles\\rootCatalog.txt");  //取rootCatalog.txt路径
            diskBitmapDir = System.IO.Path.Combine(dir, "diskFiles\\bitMap.txt");  //取bitMap.txt路径
            rootName = "root";  //初始化根文件夹名

            WelcomeWindow window = new WelcomeWindow();  //创建一个欢迎窗口
            if (!System.IO.Directory.Exists(diskFolderDir)
                || !System.IO.File.Exists(diskCatalogDir)
                || !System.IO.File.Exists(diskBitmapDir))
                window.SetReadFromFileButton(false);  //若目录和两个txt文件有缺失，则从磁盘读取键不可用
            window.ShowDialog();  //显示欢迎窗口并进行选择

            switch (window.Reply)
            {
                case WelcomeWindow.WelcomeReply.Close:  //直接关闭欢迎窗口，则直接把主窗口也关闭，结束运行
                    Close();
                    return;
                case WelcomeWindow.WelcomeReply.CreateNewRoot:  //新建根目录
                    disk = new Disk();
                    rootTreeNode = new TreeNode(rootName);
                    rootFileNode = new FileNode(FileNode.FileNodeType.Folder, null, rootName);
                    break;
                case WelcomeWindow.WelcomeReply.ReadFromFile:  //从磁盘读入
                    IO_ReadDiskFile();
                    break;
                default:
                    break;
            }

            rootFolder = rootFileNode.folder;  //初始化根文件夹
            curFolder = rootFolder;            //初始化当前文件夹
            listViewItems = new List<ListViewItem>();  //初始化当前列表视图中的表项
            pathRecord = new List<Folder>();  //初始化记录访问过的文件夹的列表
            pathRecord.Add(rootFolder);  //在pathRecord中添加根文件夹
            pathPointer = 0;             //当前文件夹在pathRecord中的0号位置
            this.buttonPathBackward.Enabled = false;  //在根文件夹无法回退，回退按钮不可用
            this.buttonPathForward.Enabled = false;   //未访问过根文件夹的任何子文件夹，无法前进，前进按钮不可用
            this.fileListView.View = View.LargeIcon;  //列表视图初始化为大图标

            RefreshAllOver();  //更新所有视图，将从磁盘文件中读取到的信息更新到视图上
        }

        /// <summary>
        /// 更新所有视图和控件
        /// </summary>
        private void RefreshAllOver()
        {
            RefreshTreeView();  //更新树形视图
            RefreshListView();  //更新列表视图
            this.currentPathDataLabel.Text = curFolder.Path;  //更新当前路径标签

            if (curFolder.nodeList.Count != 0)  //更新当前文件夹子结点情况
                this.curFolderInfoLabel.Text = curFolder.nodeList.Count.ToString() + "个项目    " + FileTools.DiscribeSize(curFolder.Size);
            else
                this.curFolderInfoLabel.Text = curFolder.nodeList.Count.ToString() + "个项目";

            if (rootFolder.nodeList.Count == 0)  //若根文件夹没有子结点（磁盘空间为空），则格式化按钮不可用
                this.menuTop_Reset.Enabled = false;
            else
                this.menuTop_Reset.Enabled = true;
        }

        /// <summary>
        /// 更新树形视图
        /// </summary>
        private void RefreshTreeView()
        {
            fileTreeView.Nodes.Clear();   //清空所有现有结点
            rootTreeNode = new TreeNode(rootName);  //创建根结点

            if (rootFolder.nodeList.Count != 0)  //设置根结点有子结点时的图标
            {
                rootTreeNode.ImageIndex = 1;
                rootTreeNode.SelectedImageIndex = 1;
            }

            else  //设置根结点无子结点时的图标
            {
                rootTreeNode.ImageIndex = 0;
                rootTreeNode.SelectedImageIndex = 0;
            }

            FileTools.TreeViewAddNode(rootTreeNode, rootFolder);  //对于根文件夹的所有子结点，在树形视图中添加对应的结点
            fileTreeView.Nodes.Add(rootTreeNode);  //树形视图中添加根结点
            rootTreeNode.ExpandAll();  //展开所有子树结点
        }

        /// <summary>
        /// 更新列表视图
        /// </summary>
        private void RefreshListView()
        {
            listViewItems.Clear();            //当前列表视图中表项的记录表清空
            this.fileListView.Items.Clear();  //当前列表视图清空

            if (curFolder.nodeList.Count() == 0)  //若当前文件夹无子结点，则不做任何操作
                return;

            foreach (FileNode node in curFolder.nodeList)
            {
                ListViewItem item;

                if (node.Type == FileNode.FileNodeType.File)  //若子结点是文件
                {
                    string[] info = new string[]  //取详细信息视图需要的信息
                    {
                            node.file.Name + File.suffixNames[(int)node.file.Type],  //文件名
                            node.file.UpdatedTime.ToString(),  //修改时间
                            "文本文件",  //类型
                            FileTools.DiscribeSize(node.file.Size)  //大小
                    };

                    item = new ListViewItem(info);  //创建列表视图新表项

                    if (node.file.Size != 0)
                        item.ImageIndex = 3;  //设置文件不为空时的图标
                    else
                        item.ImageIndex = 2;  //设置文件为空时的图标
                }

                else  //若子结点是文件夹
                {
                    string[] info = new string[]  //取详细信息视图需要的信息
                    {
                            node.folder.Name,  //文件名
                            node.folder.UpdatedTime.ToString(),  //修改时间
                            "文件夹",  //类型
                            FileTools.DiscribeSize(node.folder.Size)  //大小
                    };

                    item = new ListViewItem(info);  //创建列表视图新表项

                    if (node.folder.nodeList.Count != 0)
                        item.ImageIndex = 1;  //设置文件夹有子结点时的图标
                    else
                        item.ImageIndex = 0;  //设置文件夹无子结点时的图标
                }

                listViewItems.Add(item);       //将新表项添加到当前列表视图中表项的记录表
                fileListView.Items.Add(item);  //将新表项添加到当前列表视图中
            }
        }

        /// <summary>
        /// 双击列表视图时的打开操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFromListView(object sender, EventArgs e)
        {
            if (this.fileListView.SelectedItems.Count == 0)  //若列表视图没有项被选中，则不做任何操作
                return;

            ListViewItem item = this.fileListView.SelectedItems[0];  //若有选中则取第一个选中的项

            for (int i = 0; i < curFolder.nodeList.Count(); i++)
            {
                if (listViewItems[i] == item)  //在当前列表视图中找到文件树中对应的结点
                {
                    FileNode curNode = curFolder.nodeList[i];
                    OpenFileNode(ref curNode);  //将该文件结点打开
                    break;
                }
            }
        }

        /// <summary>
        /// 双击树形视图时的打开操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFromTreeView(object sender, EventArgs e)
        {
            if (this.fileTreeView.SelectedNode == null)  //若树形视图没有项被选中，则不做任何操作
                return;

            TreeNode curNode = this.fileTreeView.SelectedNode;  //当前树结点，初始为选中的结点
            TreeNode parentNode = curNode.Parent;  //双亲树结点，初始为选中结点的双亲结点
            List<int> indexes = new List<int>();   //路径记录列表

            while (parentNode != null)  //向上寻找直至根结点
            {
                for (int i = 0; i < parentNode.Nodes.Count; i++)  //将curNode在parentNode子结点列表中的下标记录在indexes中
                {
                    if (parentNode.Nodes[i] == curNode)
                    {
                        indexes.Add(i);
                        break;
                    }
                }

                //两指针向上一层
                curNode = curNode.Parent;
                parentNode = parentNode.Parent;
            }

            if (indexes.Count == 0)  //若indexes为空，则打开根文件夹
            {
                if (curFolder == rootFolder)  //若当前文件夹就是根文件夹，则不做任何操作，避免冗余操作和记录
                    return;

                OpenFileNode(ref rootFileNode);
            }
            else  //否则向下寻找打开结点
            {
                indexes.Reverse();  //记录是自下而上记录，现在自上而下寻找，反转列表的顺序
                FileNode current = rootFolder.nodeList[indexes[0]];  //初始是第一层的子项

                if (indexes.Count == 1)  //若第一层的子项就是路径终点，则直接打开
                {
                    if (current.Type == FileNode.FileNodeType.File
                        || (current.Type == FileNode.FileNodeType.Folder && curFolder != current.folder))
                        OpenFileNode(ref current);  //只有在current是文件，或与当前文件夹不同的文件夹时才打开，避免冗余操作和记录

                    return;
                }

                for (int index = 1; index < indexes.Count; index++)  //根据indexes的记录一层层向下
                {
                    current = current.folder.nodeList[indexes[index]];

                    if (index == indexes.Count - 1)  //若遇到了最后一位，则说明到达记录路径的终点，将该结点打开
                    {
                        if (current.Type == FileNode.FileNodeType.File
                        || (current.Type == FileNode.FileNodeType.Folder && curFolder != current.folder))
                            OpenFileNode(ref current);  //只有在current是文件，或与当前文件夹不同的文件夹时才打开，避免冗余操作和记录
                    }
                }
            }
        }

        /// <summary>
        /// 打开文件结点
        /// </summary>
        /// <param name="node"> 待打开的文件结点 </param>
        private void OpenFileNode(ref FileNode node)
        {
            if (node.Type == FileNode.FileNodeType.Folder)  //若代打开结点是文件夹
            {
                curFolder = node.folder;  //更新当前文件夹

                if (pathPointer < pathRecord.Count - 1)  //若pathPointer指向pathRecord的中间位置，则将这个位置之后的记录全部删除
                    pathRecord.RemoveRange(pathPointer + 1, pathRecord.Count - pathPointer - 1);

                pathRecord.Add(curFolder);  //在pathRecord中添加当前文件夹
                pathPointer++;  //pathPointer指向位置向前1位

                currentPathDataLabel.Text = curFolder.Path;  //更新当前路径标签
                buttonPathBackward.Enabled = true;  //顺序向前一步，可以回退，回退按钮可用
                buttonPathForward.Enabled = false;  //主动向前，原位置之后的记录全部清空无法前进，前进按钮不可用
                RefreshAllOver();  //更新所有视图
            }

            else
            {
                TextEditor window = new TextEditor(ref node.file);
                window.callBack += RefreshAllOver;
                window.ShowDialog();
            }
        }

        /// <summary>
        /// 设置列表视图为大图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetListViewLarge(object sender, EventArgs e)
        {
            fileListView.View = View.LargeIcon;
        }

        /// <summary>
        /// 设置列表视图为小图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetListViewSmall(object sender, EventArgs e)
        {
            fileListView.View = View.SmallIcon;
        }

        /// <summary>
        /// 设置列表视图为列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetListViewList(object sender, EventArgs e)
        {
            fileListView.View = View.List;
        }

        /// <summary>
        /// 设置列表视图为详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetListViewDetails(object sender, EventArgs e)
        {
            fileListView.View = View.Details;
        }

        /// <summary>
        /// 列表视图右键菜单：新建——文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateFolderInListView(object sender, EventArgs e)
        {
            NameInputWindow window = new NameInputWindow(ref curFolder, null);  //创建名称输入窗口，传入参数为文件夹的参数
            window.callBack += RefreshAllOver;  //挂接更新所有视图的方法，在输入完成后调用
            window.ShowDialog();  //显示名称输入窗口，并完成输入及检查过程
        }

        /// <summary>
        /// 列表视图右键菜单：新建——文本文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateFileInListView(object sender, EventArgs e)
        {
            NameInputWindow window = new NameInputWindow(ref curFolder, File.FileType.TXT);  //创建名称输入窗口，传入参数为文本文件的参数
            window.callBack += RefreshAllOver;  //挂接更新所有视图的方法，在输入完成后调用
            window.ShowDialog();  //显示名称输入窗口，并完成输入及检查过程
        }

        /// <summary>
        /// 列表视图右键菜单：刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshFromMenuList(object sender, EventArgs e)
        {
            RefreshAllOver();
        }

        /// <summary>
        /// 列表视图右键菜单：删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteFromMenuList(object sender, EventArgs e)
        {
            if (fileListView.SelectedItems.Count == 0)  //若列表视图没有项被选中，则不做任何操作
                return;

            ListViewItem item = fileListView.SelectedItems[0];  //若有选中则取第一个选中的项

            for (int i = 0; i < curFolder.nodeList.Count; i++)
            {
                if (listViewItems[i] == item)  //在当前列表视图中找到文件树中对应的结点
                {
                    DialogResult result = MessageBox.Show("确定删除" + curFolder.nodeList[i].Name +"吗？",
                        "Delete",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);  //显示消息框进行二次确认

                    if (result == DialogResult.Cancel)  //若点击取消则不做任何操作
                        return;

                    int deleteSize = 0;  //删除的大小总和，初始为0

                    FileTools.DeleteFileNode(curFolder, i, ref deleteSize);  //删除该结点
                    FileTools.UpdateFolderSize(curFolder, deleteSize * -1);  //更新所有有关文件夹的大小
                    FileTools.UpdateFolderUpdatedTime(curFolder, DateTime.Now);  //更新修改时间
                    RefreshAllOver();  //更新所有视图

                    break;
                }
            }
        }

        /// <summary>
        /// 格式化磁盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetDisk(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定格式化磁盘吗？",
                "Reset The Whole Disk",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);  //显示消息框进行二次确认

            if (result == DialogResult.Cancel)  //若点击取消则不做任何操作
                return;

            int deleteSize = 0;  //删除的大小总和，初始为0
            int listCount = rootFolder.nodeList.Count;  //根文件夹子节点数量

            for (int i = 0; i < listCount; i++)
            {
                FileTools.DeleteFileNode(rootFolder, 0, ref deleteSize);  //逐个删除子结点
            }

            rootFolder.Size -= deleteSize;  //更新根文件夹大小
            curFolder = rootFolder;  //当前文件夹回到根文件夹
            pathRecord.Clear();      //访问过的文件夹的列表清空，并加入根文件夹
            pathRecord.Add(rootFolder);
            pathPointer = 0;         //pathPointer指向0号位
            buttonPathForward.Enabled = false;   //根文件夹当前无子文件夹，无法前进，前进按钮不可用
            buttonPathBackward.Enabled = false;  //在根文件夹无法回退，回退按钮不可用
            RefreshAllOver();  //更新所有视图
        }

        /// <summary>
        /// 列表视图右键菜单：属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowFileNodeInfo(object sender, EventArgs e)
        {
            if (fileListView.SelectedItems.Count == 0)  //若列表视图没有项被选中，则显示当前文件夹的属性
            {
                if (curFolder.Parent != null)  //若当前文件夹有双亲文件夹，则在双亲文件夹的子结点列表中找到对应的文件结点
                {
                    foreach (FileNode node in curFolder.Parent.nodeList)
                    {
                        if (node.Name == curFolder.Name)
                        {
                            InfoWindow window = new InfoWindow(node);  //显示当前文件夹的属性
                            window.Show();
                            return;
                        }
                    }
                }

                else  //若当前文件夹无双亲文件夹，则显示根结点的属性
                {
                    InfoWindow window = new InfoWindow(rootFolder);
                    window.Show();
                    return;
                }
            }

            ListViewItem item = fileListView.SelectedItems[0];  //若列表视图有项被选中，则选择第一个项显示属性

            for (int i = 0; i < curFolder.nodeList.Count; i++)
            {
                if (listViewItems[i] == item)
                {
                    InfoWindow window = new InfoWindow(curFolder.nodeList[i]);
                    window.Show();
                    break;
                }
            }
        }

        /// <summary>
        /// 当前文件夹回退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PathStepBackward(object sender, EventArgs e)
        {
            pathPointer--;  //pathPointer指向位置回退1位
            buttonPathForward.Enabled = true;  //顺序回退，可以前进，前进按钮可用

            if (pathPointer == 0)  //若回退后指向0号位，则回到了根文件夹，回退按钮不可用
                buttonPathBackward.Enabled = false;

            curFolder = pathRecord[pathPointer];  //更新当前文件夹
            currentPathDataLabel.Text = curFolder.Path;  //更新当前路径标签
            RefreshAllOver();  //更新所有视图
        }

        /// <summary>
        /// 当前文件夹前进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PathStepForward(object sender, EventArgs e)
        {
            pathPointer++;  //pathPointer指向位置前进1位
            buttonPathBackward.Enabled = true;  //顺序前进，可以回退，回退按钮可用

            if (pathPointer == pathRecord.Count - 1)  //若回退后指向最后1位，则到了记录表的末尾，前进按钮不可用
                buttonPathForward.Enabled = false;

            curFolder = pathRecord[pathPointer];  //更新当前文件夹
            currentPathDataLabel.Text = curFolder.Path;  //更新当前路径标签
            RefreshAllOver();  //更新所有视图
        }

        /// <summary>
        /// 列表视图右键菜单：重命名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameFileNode(object sender, EventArgs e)
        {
            if (fileListView.SelectedItems.Count == 0)  //若列表视图没有项被选中，则不做任何操作
                return;

            ListViewItem item = fileListView.SelectedItems[0];  //若有选中则取第一个选中的项

            for (int i = 0; i < curFolder.nodeList.Count; i++)
            {
                if (listViewItems[i] == item)  //在当前列表视图中找到文件树中对应的结点
                {
                    FileNode node = curFolder.nodeList[i];

                    if (node.Type == FileNode.FileNodeType.Folder)
                    {
                        NameInputWindow window = new NameInputWindow(ref curFolder, null, node);  //创建名称输入窗口，传入参数为该文件夹的参数
                        window.callBack += RefreshAllOver;  //挂接更新所有视图的方法，在输入完成后调用
                        window.ShowDialog();  //显示名称输入窗口，并完成输入及检查过程
                    }

                    else
                    {
                        NameInputWindow window = new NameInputWindow(ref curFolder, node.file.Type, node);  //创建名称输入窗口，传入参数为该文件的参数
                        window.callBack += RefreshAllOver;  //挂接更新所有视图的方法，在输入完成后调用
                        window.ShowDialog();  //显示名称输入窗口，并完成输入及检查过程
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// 将磁盘文件的信息读入
        /// </summary>
        /// <param name="diskFolderDir"> 磁盘文件所在文件夹路径 </param>
        /// <param name="diskCatalogDir"> rootCatalog.txt所在路径 </param>
        /// <param name="diskBitMapDir"> bitMap.txt所在路径 </param>
        private void IO_ReadDiskFile()
        {
            if (!System.IO.Directory.Exists(diskFolderDir)
                || !System.IO.File.Exists(diskCatalogDir)
                || !System.IO.File.Exists(diskBitmapDir))
                return;

            FileStream ostreamTree, ostreamBitmap;
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            ostreamTree = new FileStream(diskCatalogDir, FileMode.Open, FileAccess.Read, FileShare.Read);
            rootFileNode = binaryFormatter.Deserialize(ostreamTree) as FileNode;
            ostreamTree.Close();

            ostreamBitmap = new FileStream(diskBitmapDir, FileMode.Open, FileAccess.Read, FileShare.Read);
            disk = binaryFormatter.Deserialize(ostreamBitmap) as Disk;
            ostreamBitmap.Close();
        }

        /// <summary>
        /// 关闭主窗口时将所有信息保存到磁盘文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IO_SaveDiskFile(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("是否保存到磁盘文件？",
                "Quit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);  //显示对话框，询问是否保存

            if (result == DialogResult.No)  //若按否键，则不做任何操作
                return;

            //若文件夹缺失，则创建文件夹（若文件缺失会在FileStream初始化时创建，故在此不处理）
            if (!System.IO.Directory.Exists(diskFolderDir))
                System.IO.Directory.CreateDirectory(diskFolderDir);

            FileStream ostreamTree, ostreamBitmap;
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            ostreamTree = new FileStream(diskCatalogDir, FileMode.Create);
            binaryFormatter.Serialize(ostreamTree, rootFileNode);
            ostreamTree.Close();

            ostreamBitmap = new FileStream(diskBitmapDir, FileMode.Create);
            binaryFormatter.Serialize(ostreamBitmap, disk);
            ostreamBitmap.Close();
        }

        /// <summary>
        /// 显示关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAbout(object sender, EventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.Show();
        }

        /// <summary>
        /// 选中项时，下方标签内容更新为显示选中项的数目和总计大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowCurrentSelection(object sender, EventArgs e)
        {
            if (this.fileListView.SelectedItems.Count != 0)  //若有选中的项
            {
                this.curFolderInfoLabel.Text = "选中了 " + this.fileListView.SelectedItems.Count + " 个项目  ";  //显示选中项目数
                int totalSelectedSize = 0;  //选取项目的大小总和

                for (int i = 0; i < this.fileListView.SelectedItems.Count; i++)
                {
                    for (int j = 0; j < listViewItems.Count; j++)
                    {
                        if (this.fileListView.SelectedItems[i] == listViewItems[j])  //找到每个选中项目对应的文件结点
                        {
                            if (curFolder.nodeList[j].Type == FileNode.FileNodeType.Folder)  //将结点的大小累加到totalSelectedSize
                                totalSelectedSize += curFolder.nodeList[j].folder.Size;
                            else
                                totalSelectedSize += curFolder.nodeList[j].file.Size;
                        }
                    }
                }

                this.curFolderInfoLabel.Text += FileTools.DiscribeSize(totalSelectedSize);  //处理累计大小后显示
            }

            else  //若无选中的项
            {
                if (curFolder.nodeList.Count != 0)  //更新当前文件夹子结点情况
                    this.curFolderInfoLabel.Text = curFolder.nodeList.Count.ToString() + "个项目    " + FileTools.DiscribeSize(curFolder.Size);
                else
                    this.curFolderInfoLabel.Text = curFolder.nodeList.Count.ToString() + "个项目";
            }
        }
    }

    /// <summary>
    /// 磁盘中的块
    /// </summary>
    [Serializable]
    public class Block
    {
        private const int size = 512;  //块大小
        private char[] data;  //块中的数据

        /// <summary>
        /// 初始化块
        /// </summary>
        public Block()
        {
            this.data = new char[size];  //开辟块的空间，大小为512B
        }

        /// <summary>
        /// 向块中写入数据
        /// </summary>
        /// <param name="newData"> 待写入的数据 </param>
        public void Write(string newData)
        {
            int length = newData.Length > Block.size ? Block.size : newData.Length;  //数据长度超过块的总长则只写入512B的数据

            for (int i = 0; i < length; i++)  //逐个字符写入块
            {
                this.data[i] = newData[i];
            }
        }

        /// <summary>
        /// 读块中的数据
        /// </summary>
        /// <returns> 字符串形式的块中数据 </returns>
        public string Read()
        {
            return new string(this.data);
        }
    }

    /// <summary>
    /// 模拟的磁盘空间
    /// </summary>
    [Serializable]
    public class Disk
    {
        private const int diskSize = 1000 * 1000;  //磁盘空间容量（按磁盘块数计）
        private int capturedBlockCount;  //占用的块总数
        private Block[] blockList;    //块列表
        private bool[] bitMap;        //空间位图

        /// <summary>
        /// 获取磁盘空间的块列表
        /// </summary>
        public Block[] BlockList
        {
            get { return this.blockList; }
        }

        /// <summary>
        /// 获取磁盘空间的空间位图
        /// </summary>
        public bool[] BitMap
        {
            get { return this.bitMap; }
        }

        /// <summary>
        /// 获取磁盘空间总块数
        /// </summary>
        public int DiskSize
        {
            get { return Disk.diskSize; }
        }

        /// <summary>
        /// 获取占用的块总数
        /// </summary>
        public int CapturedBlockCount
        {
            get { return this.capturedBlockCount; }
        }

        /// <summary>
        /// 初始化磁盘空间
        /// </summary>
        public Disk()
        {
            this.blockList = new Block[diskSize];  //创建块列表
            this.bitMap = new bool[diskSize];      //创建空间位图
            this.capturedBlockCount = 0;           //占用的块为0个

            for (int i = 0; i < diskSize; i++)  //位图所有位初始化为1
            {
                this.bitMap[i] = true;
            }
        }

        /// <summary>
        /// 在磁盘空间中找一个空闲块，并返回其位置，若未找到则返回-1
        /// </summary>
        /// <returns> 第一个空闲位置在整个空间位图中的下标 </returns>
        public int GetFreeBlock()
        {
            for (int bitNum = 0; bitNum < diskSize; bitNum++)  //遍历位图的每一个位置
            {
                if (this.bitMap[bitNum])  //若该位置空闲则返回该位置
                    return bitNum;
            }

            return -1;
        }

        /// <summary>
        /// 将块设置为空闲状态
        /// </summary>
        /// <param name="blockNum"> 待设置的块在整个空间位图中的下标 </param>
        public void FreeBlock(int blockNum)
        {
            if (!this.bitMap[blockNum] && blockNum >= 0 && blockNum <= diskSize)  //若该块已被占用且下标合法，则设为空闲
            {
                this.bitMap[blockNum] = true;
                this.capturedBlockCount--;
            }
        }

        /// <summary>
        /// 将块设置为占用状态
        /// </summary>
        /// <param name="blockNum"> 待设置的块在整个空间位图中的下标 </param>
        public void CaptureBlock(int blockNum)
        {
            if (this.bitMap[blockNum] && blockNum >= 0 && blockNum <= diskSize)  //若该块没有被占用且下标合法，则设为占用
            {
                this.bitMap[blockNum] = false;
                this.capturedBlockCount++;
            }
        }
    }

    /// <summary>
    /// 文件类
    /// </summary>
    [Serializable]
    public class File
    {
        /// <summary>
        /// 文件的种类
        /// </summary>
        public enum FileType
        {
            TXT
        }

        /// <summary>
        /// 各类文件的后缀名
        /// </summary>
        public static string[] suffixNames =
        {
            ".txt"
        };

        private FileType type;  //文件类型
        private int size;       //文件大小
        private string name;    //文件名
        private DateTime createdTime;   //创建时间
        private DateTime updatedTime;   //修改时间
        private List<int> blockList;  //索引表
        private string path;    //文件路径
        private Folder parent;  //双亲文件夹

        /// <summary>
        /// 获取或设置文件名
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// 获取或设置文件路径
        /// </summary>
        public string Path
        {
            get { return this.path; }
            set { this.path = value; }
        }

        /// <summary>
        /// 获取或设置双亲文件夹
        /// </summary>
        public Folder Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        /// <summary>
        /// 获取文件创建时间
        /// </summary>
        public DateTime CreatedTime
        {
            get { return this.createdTime; }
        }

        /// <summary>
        /// 获取文件修改时间
        /// </summary>
        public DateTime UpdatedTime
        {
            get { return this.updatedTime; }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        public int Size
        {
            get { return this.size; }
        }

        /// <summary>
        /// 获取文件种类
        /// </summary>
        public FileType Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// 创建一个文件
        /// </summary>
        /// <param name="type"> 文件类型 </param>
        /// <param name="name"> 文件名 </param>
        /// <param name="parentFolder"> 双亲文件夹 </param>
        /// <param name="parentPath"> 双亲文件夹路径 </param>
        public File(FileType type, string name, Folder parentFolder)
        {
            this.type = type;  //初始化文件类型
            this.name = name;  //初始化文件名
            this.createdTime = DateTime.Now;  //初始化创建时间
            this.updatedTime = DateTime.Now;  //初始化修改时间
            this.size = 0;  //初始化文件大小为0
            this.blockList = new List<int>();  //创建块索引表
            this.path = parentFolder.Path + '\\' + name;  //初始化文件路径
            this.parent = parentFolder;  //初始化双亲文件夹
        }

        /// <summary>
        /// 清空文件的内容
        /// </summary>
        /// <param name="disk"> 文件所在的磁盘空间 </param>
        public void SetEmpty(ref Disk disk)
        {
            for (int i = 0; i < this.blockList.Count(); i++)  //逐个释放索引项对应的磁盘块
            {
                disk.FreeBlock(this.blockList[i]);
            }

            this.blockList.Clear();  //索引表清空
            this.size = 0;  //文件大小为0

            //之后只涉及到向文件内写入数据或删除该文件，都有后续的自下而上修改文件大小和时间的操作，在此不提供
        }

        /// <summary>
        /// 向文件中写入数据
        /// </summary>
        /// <param name="disk"> 文件所在的磁盘空间 </param>
        /// <param name="data"> 待写入的数据 </param>
        /// <returns> 写入是否成功 </returns>
        public bool Write(ref Disk disk, string data)
        {
            string oldData = this.Read(disk);  //读出文件原数据
            int oldSize = this.size;           //取得文件原大小
            int totalLength = data.Length;     //写入数据的总长
            List<int> oldBlockList = this.blockList;  //取得文件原索引表
            SetEmpty(ref disk);  //清空文件原内容

            int newBlockNum = data.Length / 512 + (data.Length % 512 == 0 ? 0 : 1);  //按数据长度除以512（向上取整）取得所需块数
            List<int> newBlockList = new List<int>();  //新索引表

            for (int i = 0; i < newBlockNum; i++)
            {
                int freeBlockPos = disk.GetFreeBlock();  //取得一个空闲块位置

                if (freeBlockPos == -1)  //若取得-1，则剩余空间不够
                {
                    foreach (int pos in newBlockList)  //将检查过程中占用的块释放
                    {
                        disk.FreeBlock(pos);
                    }

                    FileTools.WriteDataToDisk(ref disk, oldBlockList, oldData);  //将原数据写回
                    this.size = oldSize;  //文件大小恢复
                    return false;  //写入失败，返回假
                }

                disk.CaptureBlock(freeBlockPos);  //若取得块号正常，则预先占用该块
                newBlockList.Add(freeBlockPos);   //将该块加入新索引表
            }

            FileTools.WriteDataToDisk(ref disk, newBlockList, data);  //将新数据写入
            this.blockList = newBlockList;  //更新索引表

            //除更新文件自身的以外，自下而上更新大小和修改时间
            DateTime timeStamp = DateTime.Now;
            this.size = totalLength;
            this.updatedTime = timeStamp;
            FileTools.UpdateFolderUpdatedTime(this.parent, timeStamp);
            FileTools.UpdateFolderSize(this.parent, totalLength - oldSize);

            return true;
        }

        /// <summary>
        /// 读取文件中的数据
        /// </summary>
        /// <param name="disk"> 文件所在的磁盘空间 </param>
        /// <returns> 文件中的数据 </returns>
        public string Read(Disk disk)
        {
            string data = "";

            for (int i = 0; i < this.blockList.Count(); i++)  //根据索引表逐块从磁盘空间取出数据
            {
                data += disk.BlockList[this.blockList[i]].Read();
            }

            return data;
        }
    }

    /// <summary>
    /// 文件夹类
    /// </summary>
    [Serializable]
    public class Folder
    {
        public List<FileNode> nodeList;  //文件夹内含的结点表
        private string name;      //文件夹名
        private string path;      //文件夹路径
        private int size;         //文件夹大小
        private DateTime createdTime;  //创建时间
        private DateTime updatedTime;  //更新时间
        private Folder parent;  //双亲结点
        private Dictionary<NameTypePair, LinkedList<int>> nameList;  //子结点的名字字典

        /// <summary>
        /// 获取或设置文件夹名
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// 获取或设置文件夹路径
        /// </summary>
        public string Path
        {
            get { return this.path; }
            set { this.path = value; }
        }

        /// <summary>
        /// 获取文件夹子结点的名字字典
        /// </summary>
        public Dictionary<NameTypePair, LinkedList<int>> NameList
        {
            get { return this.nameList; }
        }

        /// <summary>
        /// 获取文件夹的双亲文件夹
        /// </summary>
        public Folder Parent
        {
            get { return this.parent; }
        }

        /// <summary>
        /// 获取文件夹的创建时间
        /// </summary>
        public DateTime CreatedTime
        {
            get { return this.createdTime; }
        }

        /// <summary>
        /// 获取或设置文件夹的修改时间
        /// </summary>
        public DateTime UpdatedTime
        {
            get { return this.updatedTime; }
            set { this.updatedTime = value; }
        }

        /// <summary>
        /// 获取或修改文件夹的大小
        /// </summary>
        public int Size
        {
            get { return this.size; }
            set { this.size = value; }
        }

        /// <summary>
        /// 创建一个文件夹
        /// </summary>
        /// <param name="name"> 文件夹名 </param>
        /// <param name="parentFolder"> 双亲文件夹（若创建的是根文件夹则为空） </param>
        public Folder(string name, Folder parentFolder = null)
        {
            if (parentFolder == null)  //双亲文件夹为空，创建根文件夹
                this.path = name + ':';  //初始化文件夹路径

            else  //双亲文件夹为空，创建一般文件夹
                this.path = parentFolder.path + '\\' + name;  //初始化文件夹路径


            this.nodeList = new List<FileNode>();  //创建子结点列表
            this.name = name;  //初始化文件夹名
            this.createdTime = DateTime.Now;  //初始化创建时间
            this.updatedTime = DateTime.Now;  //初始化修改时间
            this.size = 0;  //初始化文件夹大小为0
            this.parent = parentFolder;  //初始化双亲文件夹
            this.nameList = new Dictionary<NameTypePair, LinkedList<int>>();  //创建子结点名字字典
        }

        /// <summary>
        /// 向文件夹中添加子结点
        /// </summary>
        /// <param name="childNodeType"> 子结点类型 </param>
        /// <param name="childFileType"> 子结点文件类型（若是文件夹则为空）</param>
        /// <param name="childName"> 子结点名 </param>
        public void AddChild(FileNode.FileNodeType childNodeType, File.FileType? childFileType, string childName)
        {
            FileNode child = new FileNode(childNodeType, childFileType, childName, this);  //创建文件结点

            if (childNodeType == FileNode.FileNodeType.Folder)  //设置文件结点的双亲文件夹（新生成的子结点，不用改变大小）
                child.folder.parent = this;
            else
                child.file.Parent = this;

            this.nodeList.Add(child);  //将文件结点加入文件夹的子结点列表

            //自下而上修改时间
            FileTools.UpdateFolderUpdatedTime(this, DateTime.Now);
        }
    }

    /// <summary>
    /// 文件夹结点类，其构成文件树的基本元素，内含一个文件夹或一个文件
    /// </summary>
    [Serializable]
    public class FileNode
    {
        /// <summary>
        /// 文件结点类型
        /// </summary>
        public enum FileNodeType
        {
            Folder,
            File
        }

        private FileNodeType type;  //文件结点类型
        public File file;      //文件结点内含的文件
        public Folder folder;  //文件结点内含的文件夹

        /// <summary>
        /// 获取文件结点的文件结点类型
        /// </summary>
        public FileNodeType Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// 获取或设置文件结点的名字（不直接作为成员，而是取其内含对象的名字进行操作）
        /// </summary>
        public string Name
        {
            get
            {
                if (this.type == FileNodeType.Folder)  //根据文件结点内含的对象取到名字
                    return this.folder.Name;
                else
                    return this.file.Name;
            }

            set
            {
                if (this.type == FileNodeType.Folder)  //根据文件结点内含的对象设置名字
                {
                    this.folder.Name = value;
                    this.folder.Path = this.folder.Parent.Path + '\\' + value;
                }
                else
                {
                    this.file.Name = value;
                    this.file.Path = this.file.Parent.Path + '\\' + value;
                }
            }
        }

        /// <summary>
        /// 设置文件结点的路径（不直接作为成员，而是取其内含对象的名字进行操作）
        /// </summary>
        public string Path
        {
            set
            {
                if (this.type == FileNodeType.Folder)  //根据文件结点内含的对象设置路径
                    this.folder.Path = value;
                else
                    this.file.Path = value + File.suffixNames[(int)this.file.Type];
            }
        }

        /// <summary>
        /// 创建一个文件结点
        /// </summary>
        /// <param name="nodeType"> 文件结点类型 </param>
        /// <param name="fileType"> 文件类型（若是文件夹则为空） </param>
        /// <param name="name"> 文件结点名 </param>
        /// <param name="parentFolder"> 双亲文件夹 </param>
        /// <param name="parentPath"> 双亲文件夹路径 </param>
        public FileNode(FileNodeType nodeType, File.FileType? fileType, string name, Folder parentFolder = null)
        {
            this.type = nodeType;

            if (type == FileNodeType.Folder)
            {
                this.file = null;

                if (parentFolder == null)  //若无双亲文件夹，则创建根文件夹
                    this.folder = new Folder(MainWindow.RootName);
                else  //若有双亲文件夹，则创建一般文件夹
                    this.folder = new Folder(name, parentFolder);
            }

            else
            {
                this.file = new File(fileType.Value, name, parentFolder);
                this.folder = null;
            }
        }
    }

    /// <summary>
    /// 一些文件相关的工具函数
    /// </summary>
    public class FileTools
    {
        /// <summary>
        /// 对于文件树中的一个文件夹，将其所有子节点在树形视图中添加对应的结点
        /// </summary>
        /// <param name="treeNode"> 树形视图的双亲节点 </param>
        /// <param name="folder"> 文件树中的双亲结点 </param>
        public static void TreeViewAddNode(TreeNode treeNode, Folder folder)
        {
            if (folder.nodeList.Count() == 0)  //文件夹没有子结点，不做任何操作
                return;

            foreach (FileNode node in folder.nodeList)
            {
                if (node.Type == FileNode.FileNodeType.Folder)  //若子结点是文件夹
                {
                    TreeNode newNode = new TreeNode(node.Name);  //创建子结点
                    TreeViewAddNode(newNode, node.folder);  //递归添加该文件夹所有的子结点

                    if (node.folder.nodeList.Count != 0)  //设置文件夹有子结点时的图标
                    {
                        newNode.ImageIndex = 1;
                        newNode.SelectedImageIndex = 1;
                    }

                    else  //设置文件夹无子结点时的图标
                    {
                        newNode.ImageIndex = 0;
                        newNode.SelectedImageIndex = 0;
                    }

                    treeNode.Nodes.Add(newNode);  //将该子结点在树形视图中添加对应的结点
                }

                else  //若子结点是文件
                {
                    TreeNode newNode = new TreeNode(node.Name + File.suffixNames[(int)node.file.Type]);  //创建子结点

                    if (node.file.Size != 0)  //设置文件不为空时的图标
                    {
                        newNode.ImageIndex = 3;
                        newNode.SelectedImageIndex = 3;
                    }

                    else  //设置文件为空时的图标
                    {
                        newNode.ImageIndex = 2;
                        newNode.SelectedImageIndex = 2;
                    }

                    treeNode.Nodes.Add(newNode);  //将该子结点在树形视图中添加对应的结点
                }
            }
        }

        /// <summary>
        /// 删除文件夹中的一个结点（若该结点为文件夹则递归删除其子结点）
        /// </summary>
        /// <param name="folder"> 待删除结点的双亲文件夹 </param>
        /// <param name="index"> 待删除结点，在双亲文件夹子结点列表中的下标 </param>
        /// <param name="deleteSize"> 本次删除文件的大小 </param>
        public static void DeleteFileNode(Folder folder, int index, ref int deleteSize)
        {
            if (folder.nodeList.Count == 0)  //若双亲文件夹无子结点，则不做任何操作
                return;

            string nodeName = folder.nodeList[index].Name;  //取待删除结点的名字
            List<NameTypePair> emptyPairs = new List<NameTypePair>();  //删除后为空的键值对

            foreach (var pair in folder.NameList)  //检查双亲文件夹的名字列表
            {
                int result = NameInputWindow.SubNameCheck(pair.Key.name, ref nodeName, false);  //将键值对的名字与待删除结点的名字比对

                if (result == -1)  //不符合则检查下一个对
                    continue;
                else  //符合则在值的链表中删除对应的下标，若删除后链表为空，则记录下该键值对
                {
                    pair.Value.Remove(result);

                    if (pair.Value.Count == 0)
                        emptyPairs.Add(new NameTypePair(pair.Key));
                }
            }

            foreach (var pair in emptyPairs)  //删除值的链表已经为空的键值对
            {
                folder.NameList.Remove(pair);
            }

            if (folder.nodeList[index].Type == FileNode.FileNodeType.Folder)  //若待删除结点为文件夹
            {
                for (int i = 0; i < folder.nodeList[index].folder.nodeList.Count; i++)
                {
                    DeleteFileNode(folder.nodeList[index].folder, 0, ref deleteSize);  //递归删除每一个子结点
                }

                folder.nodeList.RemoveAt(index);  //删除该结点
            }

            else  //若待删除结点为文件
            {
                deleteSize += folder.nodeList[index].file.Size;  //将该文件大小累加到deleteSize上
                folder.nodeList[index].file.SetEmpty(ref MainWindow.disk);  //SetEmpty之后就清空，故先不在此改变时间
                folder.nodeList.RemoveAt(index);  //删除该结点
            }
        }

        /// <summary>
        /// 更改文件夹的大小（若其有双亲文件夹则向上都改变）
        /// </summary>
        /// <param name="folder"> 待修改的文件夹 </param>
        /// <param name="updateSize"> 文件夹大小的变化量 </param>
        public static void UpdateFolderSize(Folder folder, int updateSize)
        {
            while (folder != null && updateSize != 0)
            {
                folder.Size += updateSize;
                folder = folder.Parent;
            }
        }

        /// <summary>
        /// 更改文件夹的修改时间（若其有双亲文件夹则向上都改变）
        /// </summary>
        /// <param name="startFolder"> 待修改的文件夹 </param>
        /// <param name="timeStamp"> 修改后的时间 </param>
        public static void UpdateFolderUpdatedTime(Folder startFolder, DateTime timeStamp)
        {
            Folder folder = startFolder;

            while (folder != null)
            {
                folder.UpdatedTime = timeStamp;
                folder = folder.Parent;
            }
        }

        /// <summary>
        /// 若重命名的是一个文件夹，则递归更改其所有子结点的路径
        /// </summary>
        public static void RenameFolderPath(Folder folder)
        {
            if (folder.nodeList.Count == 0)
                return;

            foreach (FileNode node in folder.nodeList)
            {
                node.Path = folder.Path + '\\' + node.Name;

                if (node.Type == FileNode.FileNodeType.Folder)
                    RenameFolderPath(node.folder);
            }
        }

        /// <summary>
        /// 直接将数据写入磁盘
        /// </summary>
        /// <param name="disk"> 磁盘空间 </param>
        /// <param name="blockList"> 寻找好的磁盘块号序列 </param>
        /// <param name="data"> 待写入的数据 </param>
        public static void WriteDataToDisk(ref Disk disk, List<int> blockList, string data)
        {
            //若磁盘块号序列为空，则不做任何操作
            if (blockList.Count == 0)
                return;

            //数据长度若超过了磁盘块的容量，则逐块写入
            for (int i = 0; i < blockList.Count && data.Length > 512; i++)
            {
                disk.BlockList[blockList[i]] = new Block();  //对应位置创建块实例
                disk.BlockList[blockList[i]].Write(data);  //将数据写入块
                disk.CaptureBlock(blockList[i]);   //将块设置为占用状态
                data = data.Remove(0, 512);  //将数据前512B截取
            }

            //写入剩余不足一个块的数据
            disk.BlockList[blockList[blockList.Count - 1]] = new Block();  //对应位置创建块实例
            disk.BlockList[blockList[blockList.Count - 1]].Write(data);  //将数据写入块
            disk.CaptureBlock(blockList[blockList.Count - 1]);   //将块设置为占用状态
        }

        /// <summary>
        /// 输入字节数返回包括单位的字符串
        /// </summary>
        /// <param name="size"> 字节数 </param>
        /// <returns> 包括单位的字符串 </returns>
        public static string DiscribeSize(int size)
        {
            if (size < 1024)
                return size.ToString() + " B";

            else if (size < 1048576)
            {
                double kiloCount = Math.Round((double)size / 1024, 2);
                return kiloCount.ToString() + " KB";
            }

            else if (size < 1073741824)
            {
                double megaCount = Math.Round((double)size / 1048576, 2);
                return megaCount.ToString() + " MB";
            }

            else
            {
                double gigaCount = Math.Round((double)size / 1073741824, 2);
                return gigaCount.ToString() + " GB";
            }
        }
    }
}
