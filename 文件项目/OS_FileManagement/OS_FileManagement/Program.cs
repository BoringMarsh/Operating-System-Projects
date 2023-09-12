using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS_FileManagement
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainWindow mainWindow = new MainWindow();  //先创建主窗口，并选择初始化方式

            if (!mainWindow.IsDisposed)  //若直接关闭了欢迎窗口，则主窗口对象被释放，不做任何操作；否则开始运行
                Application.Run(mainWindow);
        }
    }
}
