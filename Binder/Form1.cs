﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace MultiBinder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        string scriptpath = Path.GetTempPath() + "stub.au3";     
        string output = null;

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = "Executable|*.exe";
            if(SFD.ShowDialog() == DialogResult.OK)
            {
                button1.Enabled = false;
                thread.RunWorkerAsync();
                output = SFD.FileName;            
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "All|*.*";
            OFD.Multiselect = true;

            if(OFD.ShowDialog() == DialogResult.OK)
            {
                foreach(string file in OFD.FileNames)
                {
                    if(file.Length > 0)
                    {
                        listView1.Items.Add(file);
                    }
                }
            }
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                menu.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem item in listView1.SelectedItems)
            {
                listView1.Items.Remove(item);
            }
        }

        private void thread_DoWork(object sender, DoWorkEventArgs e)
        {
            string stub = Properties.Resources.Base64 + "\n";
            foreach (ListViewItem item in listView1.Items)
            {
                string binder = Properties.Resources.Binder;
                string varname = Stub.RandomString(10);
                binder = binder.Replace("%binder%", Stub.RandomString(10))
                    .Replace("%path%", Stub.RandomString(10))
                    .Replace("%filename%", Path.GetFileName(item.Text))
                    .Replace("%var%", varname)
                    .Replace("%base64%", Stub.ChuckSplit(Stub.Base64String(item.Text), varname, 400));
                stub += binder + "\n";
            }

            File.WriteAllText(scriptpath, stub);
            Compiler.Compil(scriptpath, output);
            MessageBox.Show("Done", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            button1.Enabled = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCRwBzjhENxzlhw9jWOKwKWw?view_as=subscriber");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://discord.gg/8CwZWK6");
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            label1.Text = DateTime.Now.ToLongTimeString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ABOUT_SUTAK_BINDER about = new ABOUT_SUTAK_BINDER();
            about.Show();
        }
    }
}
