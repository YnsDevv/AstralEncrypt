using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace AstralEncrypt.Fonction
{
    public class Obfuscation
    {
        private static Random _random = new Random();
        private static List<String> _names = new List<string>();
        public static string NameSpaceRunpe;
        public static string ClassRunpe;
        public static string MethodRunpe;

        public static void clean_asm(ModuleDef md)
        {
            foreach (var type in md.GetTypes())
            {
                foreach (MethodDef method in type.Methods)
                {
                    // empty method check
                    if (!method.HasBody) continue;

                    method.Body.SimplifyBranches();
                    method.Body.OptimizeBranches(); // negates simplifyBranches
                    //method.Body.OptimizeMacros();
                }
            }
        }

        public static void obfuscate_strings(ModuleDef md)
        {
            foreach (var type in md.GetTypes())
            {
                // methods in type
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                    {
                        if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                        {
                            String regString = method.Body.Instructions[i].Operand.ToString();
                            String encString = Convert.ToBase64String(Encoding.UTF8.GetBytes(regString));
                            method.Body.Instructions[i].OpCode =
                                OpCodes.Nop; // errors occur if instruction not replaced with Nop
                            method.Body.Instructions.Insert(i + 1,
                                new Instruction(OpCodes.Call,
                                    md.Import(typeof(Encoding).GetMethod("get_UTF8",
                                        new Type[] { })))); // Load string onto stack
                            method.Body.Instructions.Insert(i + 2,
                                new Instruction(OpCodes.Ldstr, encString)); // Load string onto stack
                            method.Body.Instructions.Insert(i + 3,
                                new Instruction(OpCodes.Call,
                                    md.Import(typeof(Convert).GetMethod("FromBase64String",
                                        new[]
                                        {
                                            typeof(string)
                                        })))); // call method FromBase64String with string parameter loaded from stack, returned value will be loaded onto stack
                            method.Body.Instructions.Insert(i + 4,
                                new Instruction(OpCodes.Callvirt,
                                    md.Import(typeof(Encoding).GetMethod("GetString",
                                        new[]
                                        {
                                            typeof(byte[])
                                        })))); // call method GetString with bytes parameter loaded from stack 
                            i += 4; //skip the Instructions as to not recurse on them
                        }
                    }
                }
            }
        }

        public static void obfuscate_methodsDll(ModuleDef md, string nameMethodRunpe)
        {
            foreach (var type in md.GetTypes())
            {
                // create method to obfuscation map
                foreach (MethodDef method in type.Methods)
                {
                    // Vérification si la méthode a un nom spécial réservé par le runtime
                    if (method.IsRuntimeSpecialName) continue;

                    // Vérification si la méthode est une déclaration anticipée dans une bibliothèque externe
                    if (method.DeclaringType.IsForwarder) continue;

                    // Vérification si la méthode est vide
                    if (!method.HasBody) continue;

                    // Vérification si la méthode est un constructeur
                    if (method.IsConstructor) continue;

                    // Vérification si la méthode remplace une méthode dans une classe de base
                    if (method.HasOverrides) continue;

                    string encName = RandomString.RandomStringGenerator.Generate(50);
                    if (method.Name == nameMethodRunpe) // Ne pas toucher la classe main RunPE
                    {
                        nameMethodRunpe = encName;
                        method.Name = nameMethodRunpe;
                        MethodRunpe = nameMethodRunpe;
                        continue;
                    }

                    method.Name = encName;
                }
            }
        }

        public static void obfuscate_classes(ModuleDef md, string nameClassRunpe)
        {
            foreach (var type in md.GetTypes())
            {
                string encName = RandomString.RandomStringGenerator.Generate(50);
                if (type.Name == nameClassRunpe) // Ne pas toucher la classe main RunPE
                {
                    nameClassRunpe = encName;
                    type.Name = nameClassRunpe;
                    ClassRunpe = nameClassRunpe;
                    continue;
                }

                type.Name = encName;
            }
        }

        public static void obfuscate_namespace(ModuleDef md, string namespaceRunpe)
        {
            foreach (var type in md.GetTypes())
            {
                string encName = RandomString.RandomStringGenerator.Generate(50);
                if (type.Namespace == namespaceRunpe) // Ne pas toucher la classe main RunPE
                {
                    namespaceRunpe = encName;
                    type.Namespace = namespaceRunpe;
                    NameSpaceRunpe = namespaceRunpe;
                    continue;
                }

                type.Namespace = encName;
            }
        }
        public static void obfuscate_methods(ModuleDef md)
        {
            foreach (var type in md.GetTypes())
            {
                // create method to obfuscation map
                foreach (MethodDef method in type.Methods)
                {
                    // empty method check
                    if (!method.HasBody) continue;
                    // method is a constructor
                    if (method.IsConstructor) continue;
                    // method overrides another
                    if (method.HasOverrides) continue;
                    // method has a rtspecialname, VES needs proper name
                    if (method.IsRuntimeSpecialName) continue;
                    // method foward declaration
                    if (method.DeclaringType.IsForwarder) continue;

                    string encName = RandomString.RandomStringGenerator.Generate(50);
                    method.Name = encName;
                }
            }
        }


        public static void obfuscate_resources(ModuleDef md, String name, String encName)
        {
            if (name == "") return;
            foreach (var resouce in md.Resources)
            {
                String newName = resouce.Name.Replace(name, encName);
                resouce.Name = newName;
            }
        }

        public static void obfuscate_assembly_info(ModuleDef md)
        {
            // obfuscate assembly name
            string encName = RandomString.RandomStringGenerator.Generate(50);
            md.Assembly.Name = encName;

            // obfuscate Assembly Attributes(AssemblyInfo) .rc file
            string[] attri =
            {
                "AssemblyDescriptionAttribute", "AssemblyTitleAttribute", "AssemblyProductAttribute",
                "AssemblyCopyrightAttribute", "AssemblyCompanyAttribute", "AssemblyFileVersionAttribute"
            };
            // "GuidAttribute", and assembly version can also be changed
            foreach (CustomAttribute attribute in md.Assembly.CustomAttributes)
            {
                if (attri.Any(attribute.AttributeType.Name.Contains))
                {
                    string encAttri = RandomString.RandomStringGenerator.Generate(50);
                    attribute.ConstructorArguments[0] = new CAArgument(md.CorLibTypes.String, new UTF8String(encAttri));
                }
            }
        }
        public static void ObfuscateStub(string stub, string fullPathFileNameOut,string icon = null)
        {
            Compiler.CompileCSharpFile(stub, fullPathFileNameOut,icon);
            
            ModuleDef md = ModuleDefMD.Load(fullPathFileNameOut);
            md.Name = RandomString.RandomStringGenerator.Generate(50);

            obfuscate_strings(md);
            obfuscate_methods(md);
            obfuscate_classes(md,null);
            obfuscate_namespace(md,null);
            obfuscate_assembly_info(md);

            clean_asm(md);
            if (File.Exists(fullPathFileNameOut))
                File.Delete(fullPathFileNameOut);
            md.Write(fullPathFileNameOut);
        }

        public static void ObfuscateExe(byte[] filenameExeLoad, string pathOut)
        {
            ModuleDef md = ModuleDefMD.Load(filenameExeLoad);
            md.Name = RandomString.RandomStringGenerator.Generate(50);

            obfuscate_strings(md);
            obfuscate_methods(md);
            obfuscate_classes(md,null);
            obfuscate_namespace(md,null);
            obfuscate_assembly_info(md);
            //obfuscateVariables(md); // md.Write already simplifies variable names to there type in effect mangling them i.e: aesSetup -> aes1, aesRun -> aes2
            //obfuscateComments(md); // comments are stripped during compile opitmization

            clean_asm(md);
            if (File.Exists(pathOut))
                File.Delete(pathOut);
            md.Write(pathOut);
        }

        public static byte[] Obfuscate_dll(byte[] fileRunPe, string pathFileRunpe)
        {
            try
            {
                ModuleContext modCtx = ModuleDef.CreateModuleContext();
                ModuleDefMD module = ModuleDefMD.Load(fileRunPe, modCtx);
                
                string namespaceRunpe = "RunPE";
                string classRunpe = "Go";
                string methodsRunpe = "Execute";

                clean_asm(module);
                obfuscate_strings(module);
                obfuscate_namespace(module, namespaceRunpe);
                obfuscate_classes(module, classRunpe);
                obfuscate_methodsDll(module, methodsRunpe);

                if (File.Exists(pathFileRunpe))
                    File.Delete(pathFileRunpe);
                module.Write(pathFileRunpe);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"by amn...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.Delete(pathFileRunpe);
            }

            return File.ReadAllBytes(pathFileRunpe);
        }
    }
}