using System;
using System.IO;

namespace AstralEncrypt.Fonction
{
    public class Deplacer
    {
        public static string CopyLireStubLoaderStub(string filenamePlusExtensions)
        {
            string resourcesPathStub = Environment.CurrentDirectory.Replace("\\bin\\Debug","") + "\\Ressources\\StubLoaderStub\\"+filenamePlusExtensions;
            string resourcesPathStubDestination = Environment.CurrentDirectory + "\\"+filenamePlusExtensions;
            if(File.Exists(resourcesPathStubDestination))
                File.Delete(resourcesPathStubDestination);
            File.Copy(resourcesPathStub,resourcesPathStubDestination,true);
            return File.ReadAllText(resourcesPathStubDestination);
        }
        public static byte[] CopyLireComposants(string filenamePlusExtensions)
        {
            string resourcesPathAmsiEtw = Environment.CurrentDirectory.Replace("\\bin\\Debug","") + "\\Ressources\\Composant\\"+filenamePlusExtensions;
            string resourcesPathAmsiEtwDestination = Environment.CurrentDirectory + "\\"+filenamePlusExtensions;
            if(File.Exists(resourcesPathAmsiEtwDestination))
                File.Delete(resourcesPathAmsiEtwDestination);
            File.Copy(resourcesPathAmsiEtw,resourcesPathAmsiEtwDestination,true);
            return File.ReadAllBytes(resourcesPathAmsiEtwDestination);
        }
    }
}