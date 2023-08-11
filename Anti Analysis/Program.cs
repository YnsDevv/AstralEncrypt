using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;

namespace Anti_Analysis
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);

        public static void Main()
        {
            if (DetectManufacturer() || DetectDebugger() || DetectSandboxie() || IsSmallDisk() || IsXp())
            {
                Environment.FailFast(null);
            }
        }

        private static bool IsSmallDisk()
        {
            try
            {
                long num = 61000000000L;
                if (new DriveInfo(Path.GetPathRoot(Environment.SystemDirectory)).TotalSize <= num)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, @"by amn...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private static bool IsXp()
        {
            try
            {
                if (new ComputerInfo().OSFullName.ToLower().Contains("xp"))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, @"by amn...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private static bool DetectManufacturer()
        {
            try
            {
                using (ManagementObjectSearcher managementObjectSearcher =
                       new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
                {
                    using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
                    {
                        foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
                        {
                            string text = managementBaseObject["Manufacturer"].ToString().ToLower();
                            if ((text == "microsoft corporation" && managementBaseObject["Model"].ToString()
                                    .ToUpperInvariant().Contains("VIRTUAL")) || text.Contains("vmware") ||
                                managementBaseObject["Model"].ToString() == "VirtualBox")
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return false;
        }

        private static bool DetectDebugger()
        {
            bool flag = false;
            bool result;
            try
            {
                CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref flag);
                result = flag;
            }
            catch
            {
                result = flag;
            }

            return result;
        }

        private static bool DetectSandboxie()
        {
            bool result;
            try
            {
                if (GetModuleHandle("SbieDll.dll").ToInt32() != 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}