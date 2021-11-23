using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace 微信Hook
{
    public enum DllInjectionResult
    {
        DllNotFound,
        GameProcessNotFound,
        InjectionFailed,
        Success
    }
    public sealed class InjectUtil
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll")]
        public static extern int GetProcAddress(int hwnd, string lpname);

        [DllImport("kernel32.dll")]
        public static extern int GetModuleHandleA(string name);
        
        [DllImport("kernel32.dll")] //声明API函数
        public static extern int VirtualAllocEx(IntPtr hwnd, int lpaddress, int size, int type, int tect);
        
        [DllImport("kernel32.dll")]
        public static extern int WriteProcessMemory(IntPtr hwnd, int baseaddress, string buffer, int nsize, int filewriten);
        
        [DllImport("kernel32.dll")]
        public static extern int CreateRemoteThread(IntPtr hwnd, int attrib, int size, int address, int par, int flags, int threadid);

        static InjectUtil _instance;

        public static InjectUtil GetInstance => _instance ?? (_instance = new InjectUtil());
        
        InjectUtil(){}

        public bool Inject(Process process, string dllPath)
        {
            var hndProc = process.Handle;
            int lpAddress = VirtualAllocEx(hndProc, 0, dllPath.Length + 1, 4096, 4);
            if (lpAddress == 0)
            {
                MessageBox.Show("申请内存失败", "提示");
                return false;
            }
            // byte[] bytes = Encoding.ASCII.GetBytes(dllPath);
            int temp = 0;
            if (WriteProcessMemory(hndProc, lpAddress, dllPath, dllPath.Length + 1, temp) == 0)
            {
                MessageBox.Show("写入内存失败", "提示");
                return false;
            }
            int lpLLAddress = GetProcAddress(GetModuleHandleA("Kernel32"), "LoadLibraryA");
            if (lpLLAddress == 0)
            {
                MessageBox.Show("获取函数入口点失败", "提示");
                return false;
            }

            if (CreateRemoteThread(hndProc, 0, 0, lpLLAddress, lpAddress, 0, temp) == 0)
            {
                MessageBox.Show("创建远程线程失败", "提示");
                return false;
            }
            CloseHandle(hndProc);
            return true;
        }
    }
}