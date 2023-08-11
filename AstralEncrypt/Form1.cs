using System;
using System.Windows.Forms;

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
            guna2TextBox_password_stub.Text = RandomString.RandomStringGenerator.Generate(256);
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

        private void guna2Button_hide_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
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

        private void guna2GradientButton_password_Click(object sender, EventArgs e)
        {
            guna2TextBox_password_stub.Text = RandomString.RandomStringGenerator.Generate(256);
        }

        private void guna2GradientButton_encrypt_Click(object sender, EventArgs e)
        {
            string filenameExe = RandomString.RandomStringGenerator.Generate(5) + ".exe";
        }
    }
}