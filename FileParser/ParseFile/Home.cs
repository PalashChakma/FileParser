using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessObject;
using System.IO;

namespace ParseFile
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void btnSource_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) 
            {
                string srcfile = openFileDialog1.FileName;
                lblSourceFile.Text = srcfile;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (File.Exists(lblSourceFile.Text))
            {
                try
                {
                    IFileParser fileAdapter;
                    FileFactory filefactory = new FileFactory(FileFactory.FileTypeAdapter.CsvFileParser);
                    fileAdapter = filefactory.GetFileAdapter();
                    fileAdapter.ParseFile(lblSourceFile.Text);
                    System.Windows.Forms.MessageBox.Show("Processing file complete. Please check source file directory for two output files");
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error while Parsing File. Error Details: " + ex.Message);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select file");
            }

        }
       
    }
}
