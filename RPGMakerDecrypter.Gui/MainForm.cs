﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RPGMakerDecrypter.Decrypter;
using RPGMakerDecrypter.Decrypter.Exceptions;

namespace RPGMakerDecrypter.Gui
{
    public partial class MainForm : Form
    {
        private RPGMakerVersion currentArchiveVersion;
        private RGSSAD currentArchive;

        public enum PackVersion
        {
            Invalid = -1,
            Vx = 1,
            VxAce = 3,
            Fux2Pack = (int)('k'),
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void openRGSSADToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            StringBuilder fileTypesStringBuilder = new StringBuilder();
            fileTypesStringBuilder.Append("RPG Maker XP Encrypted Archive (.rgssad)|*.rgssad|");
            fileTypesStringBuilder.Append("RPG Maker VX Encrypted Archive (.rgss2a)|*.rgss2a|");
            fileTypesStringBuilder.Append("RPG Maker VX Ace Encrypted Archive (.rgss3a)|*.rgss3a|");
            fileTypesStringBuilder.Append("All Files (*.*)|*.*");

            openFileDialog.Filter = fileTypesStringBuilder.ToString();

            var result = openFileDialog.ShowDialog();

            if (result == DialogResult.Abort || result == DialogResult.Cancel)
            {
                return;
            }

            // It's ok to reset here because user has decided to select other file
            Reset();

            string inputFilePath = openFileDialog.FileName;

            currentArchiveVersion = RGSSAD.GetVersion(inputFilePath);

            if (currentArchiveVersion == RPGMakerVersion.Invalid)
            {
                MessageBox.Show("Invalid input file.", "Invalid input file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            try
            {
                var binaryReader = new BinaryReader(new FileStream(inputFilePath, FileMode.Open));
                string header;
                try
                {
                    header = BinaryUtils.ReadString(binaryReader, 7);
                }
                catch (Exception)
                {
                    throw new InvalidArchiveException("Archive is in invalid format.");
                }
                
                if (!Constants.RGSSADHeader.Contains(header))
                {
                    throw new InvalidArchiveException("Header was not found for archive.");
                }
                int versionNumber = binaryReader.ReadByte();

                if (!Constants.SupportedRGSSVersions.Contains(versionNumber))
                {
                    versionNumber =  -1;
                }

                binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);

                switch ((PackVersion)versionNumber)
                {
                    case PackVersion.Vx:
                        currentArchive = new RGSSADv1(binaryReader);
                        break;
                    case PackVersion.VxAce:
                        currentArchive = new RGSSADv3(binaryReader);
                        break;
                    case PackVersion.Fux2Pack:
                        currentArchive = new RGSSADv3Fux2Pack(binaryReader);
                        break;
                    default:
                        throw new UnsupportedArchiveException("Invalid version number from binary reader");
                }
            }
            catch (InvalidArchiveException)
            {
                MessageBox.Show("Archive is invalid or corrupted. Reading failed.", "Invalid archive",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (UnsupportedArchiveException)
            {
                MessageBox.Show("Archive is not supported or it is corrupted.", "Archive not supported",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong with reading or extraction. Archive is likely invalid or corrupted.", "Archive corrupted",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (ArchivedFile archivedFile in currentArchive.ArchivedFiles)
            {
                archivedFilesListBox.Items.Add(archivedFile.Name);
            }

            SetClickableElementsEnabled(true);

            statusLabel.Text = "Archive opened succesfully.";
        }

        private void SetClickableElementsEnabled(bool enabled)
        {
            archivedFilesListBox.Enabled = enabled;
            extractToolStripMenuItem.Enabled = enabled;
        }

        private void Reset()
        {
            archivedFilesListBox.Items.Clear();
            SetClickableElementsEnabled(false);
            extractFileButton.Enabled = false;

            currentArchiveVersion = RPGMakerVersion.Invalid;

            currentArchive?.Dispose();
        }

        private void archivedFilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            extractFileButton.Enabled = false;

            if (currentArchive == null || !currentArchive.ArchivedFiles.Any() || archivedFilesListBox.SelectedIndex == -1)
            {
                return;
            }

            ArchivedFile archivedFile = currentArchive.ArchivedFiles[archivedFilesListBox.SelectedIndex];

            fileNameTextBox.Text = ArchivedFileNameUtils.GetFileName(archivedFile.Name);
            sizeTextBox.Text = archivedFile.Size.ToString();

            extractFileButton.Enabled = true;
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentArchive == null || !currentArchive.ArchivedFiles.Any())
            {
                return;
            }

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            var result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.Abort || result == DialogResult.Cancel)
            {
                return;
            }

            string outputDirectoryPath = folderBrowserDialog.SelectedPath;

            try
            {
                currentArchive.ExtractAllFiles(outputDirectoryPath, true);
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong with extraction. Archive is likely invalid or corrupted.");
                return;
            }

            if (generateProjectCheckBox.Checked)
            {
                ProjectGenerator.GenerateProject(currentArchiveVersion, outputDirectoryPath);
            }

            statusLabel.Text = "Archive extracted succesfully.";
        }

        private void extractFileButton_Click(object sender, EventArgs e)
        {
            if (currentArchive == null || !currentArchive.ArchivedFiles.Any() || archivedFilesListBox.SelectedIndex == -1)
            {
                return;
            }

            ArchivedFile archivedFile = currentArchive.ArchivedFiles[archivedFilesListBox.SelectedIndex];

            string fileName = ArchivedFileNameUtils.GetFileName(archivedFile.Name);
            string extension = fileName.Split('.').Last();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = fileName;
            saveFileDialog.Filter = $"Data file (.{extension})|*.{extension}";

            var result = saveFileDialog.ShowDialog();

            if (result == DialogResult.Abort || result == DialogResult.Cancel)
            {
                return;
            }

            FileInfo fileInfo = new FileInfo(saveFileDialog.FileName);
            
            try
            {
                currentArchive.ExtractFile(archivedFile, fileInfo.DirectoryName, true, false);
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong with extraction. Archive is likely invalid or corrupted.");
                return;
            }

            statusLabel.Text = $"Extracted {fileName} succesfully.";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentArchive?.Dispose();
            Environment.Exit(0);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
    }
}
