using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 微信Hook
{
    public partial class Form1 : Form
    {
        private static string WECHAT_NAME = "WeChat";
        private static string DLL_PATH = @"E:\dbg\MyWXDll.dll";
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            // 1.判断微信进程是否存在
            var process = checkWeChatExists();
            if (process == null)
            {
                MessageBox.Show("进程不存在", "警告");
                return;
            }

            var result = InjectUtil.GetInstance.Inject( process, DLL_PATH);
            if (result)
            {
                MessageBox.Show("注入成功", "提示");
            }
        }

        /// <summary>
        /// 检查微信是否启动
        /// </summary>
        /// <returns></returns>
        private Process checkWeChatExists()
        {
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                if (process.ProcessName.Equals(WECHAT_NAME))
                {
                    var fileName = process.MainModule.FileName;
                    this.textBox1.Text = fileName.Substring(0, fileName.LastIndexOf('\\')) + @"\MyWXDll.dll";
                    DLL_PATH = this.textBox1.Text;
                    return process;
                }
            }

            return null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}