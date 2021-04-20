using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace W3S2PCSC
{
    public partial class Form1 : Form
    {
        string path = "";
        string fileName = "";
        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = "sav";
            fileDialog.Title = "Open Save";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                path = fileDialog.FileName;
                fileName = Path.GetFileName(path);
            }
            button1.Enabled = true;
            
        }

        private void FileProcessing()
        { 
            Process offzipProcess = new Process();
            offzipProcess.StartInfo.FileName = @"offzip.exe";
            offzipProcess.StartInfo.Arguments = $@"-a {path} .\pc_save";
            offzipProcess.StartInfo.UseShellExecute = true;
            offzipProcess.StartInfo.CreateNoWindow = false;

            //MessageBox.Show(offzipProcess.StartInfo.Arguments);
            offzipProcess.Start();
            
            offzipProcess.WaitForExit(500);
            offzipProcess.Close();
            var path1 = @".\pc_save\0000000c.snf";
            HexProcessing(path1);
            
        }
        private void HexProcessing(string path)
        {
            
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(path)))
            {
                bw.Seek(0xC16, SeekOrigin.Begin); 

                bw.Write(0x12);
                bw.Close();
            }
            File.Copy(path, @".\pc_save\" + fileName,true);
            File.Delete(path);
            MessageBox.Show("Done");
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileProcessing();
        }
    }
}
