using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;

namespace Cocktail
{
    public partial class Principal : Form
    {
        ArrayList files;
        ArrayList editors;
        //MemoryStream userInput = new MemoryStream();

        public Principal()
        {
            files = new ArrayList();
            editors = new ArrayList();
            InitializeComponent();
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            // Load previous tabs
        }

        // Open file
        private void abrirArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            // If no tabs then create once
            if (!(tabs.SelectedIndex >= 0)) this.addNewTab();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // Change current tab name
                            tabs.SelectedTab.Text = Path.GetFileNameWithoutExtension(openFileDialog.SafeFileName);
                            tabs.SelectedTab.ToolTipText = openFileDialog.FileName;
                            tabs.SelectedTab.ImageKey = getIconFromExt(Path.GetExtension(openFileDialog.SafeFileName).Substring(1)) + ".png";

                            // Take file extension
                            codeLang.Text = Path.GetExtension(openFileDialog.SafeFileName).Substring(1).ToUpper();

                            // Load file to Scintilla
                            SuEditor editor = (SuEditor)editors[tabs.SelectedIndex];
                            editor.Text = File.ReadAllText(openFileDialog.FileName);

                            // Get hash code of file
                            hashFile.Text = myStream.GetHashCode().ToString();
                            myStream.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo leer el fichero.", "Error fatal");
                }
            }
        }

        private string getIconFromExt(string ext)
        {
            if (Properties.Settings.Default.extSupport.Contains(ext))
            {
                return ext;
            }
            else
            {
                return Properties.Settings.Default.extOther;
            }
        }

        // Save file
        private void guardarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "All files (*.*)|*.*";
            saveFileDialog.RestoreDirectory = true;

            if (tabs.SelectedIndex >= 0)
            {
                try
                {
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        if ((myStream = saveFileDialog.OpenFile()) != null)
                        {
                            using (myStream)
                            {
                                // Change current tab name
                                tabs.SelectedTab.Text = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
                                tabs.SelectedTab.ToolTipText = saveFileDialog.ToString();
                                tabs.SelectedTab.ImageKey = getIconFromExt(Path.GetExtension(saveFileDialog.FileName).Substring(1)) + ".png";

                                // Take file extension
                                codeLang.Text = Path.GetExtension(saveFileDialog.FileName).Substring(1).ToUpper();

                                // Load file to editor
                                SuEditor editor = (SuEditor)editors[tabs.SelectedIndex];
                                //editor.SaveFile(myStream, RichTextBoxStreamType.PlainText);

                                // Get hash code of file
                                hashFile.Text = myStream.GetHashCode().ToString();
                                myStream.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo escribir el fichero.", "Error fatal");
                }
            }
        }

        private void nuevaPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.addNewTab();
        }

        private void addNewTab()
        {
            string title = "Nuevo " + (tabs.TabCount + 1).ToString();
            TabPage myTabPage = new TabPage(title);
            // Add event to the new TabPage
            //myTabPage.MouseClick += new MouseEventHandler(tabs_MouseClick);

            tabs.TabPages.Add(myTabPage);
            tabs.SelectedIndex = tabs.TabCount - 1;

            // Create internal component: SuEditor
            SuEditor editor = new SuEditor(new Scintilla());

            // Styling the editor
            editor.Dock = DockStyle.Fill;

            editors.Add(editor);
            tabs.SelectedTab.Controls.Add(editor);
            //Console.Write("selected=" + tabs.SelectedIndex + " total=" + tabs.TabCount);
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 box = new AboutBox1();
            box.ShowDialog();
        }

        private void cerrarPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeCurrentTab(tabs.SelectedTab);
        }

        private void closeCurrentTab(TabPage tabPage)
        {
            if (tabs.SelectedIndex >= 0)
            {
                int previousSelected = tabs.SelectedIndex;
                tabs.TabPages.Remove(tabPage);
                tabs.SelectedIndex = previousSelected - 1;
            }
        }

        private void tabs_MouseClick(object sender, MouseEventArgs e)
        {
            //TabControl control = sender as TabControl;
            //if (e.Button == MouseButtons.Middle)
            //{
            //    int previousSelected = tabs.SelectedIndex;
            //    tabs.TabPages.Remove(control.SelectedTab);
            //    tabs.SelectedIndex = previousSelected - 1;
            //}
        }

        private void licenciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.licenseURL);
        }

        private void reportarBugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.issuesURL);
        }

    }
}
