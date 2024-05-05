using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RPGMakerDecrypter.Common;
using RPGMakerDecrypter.Decrypter;
using RPGMakerDecrypter.Decrypter.Exceptions;

namespace RPGMakerDecrypter.Gui
{
    public partial class MainForm : Form
    {
        private RPGMakerVersion currentArchiveVersion;
        private RGSSAD currentArchive;
        private string inputFilePath;

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

            inputFilePath = openFileDialog.FileName;

            currentArchiveVersion = RGSSAD.GetRPGMakerVersion(inputFilePath);

            if (currentArchiveVersion == RPGMakerVersion.Unknown)
            {
                MessageBox.Show(
                    "Unable to determinate RGSSAD RPG Maker version. " +
                    "Please rename RGSSAD file with an extension corresponding to version: " +
                    "XP: .rgssad, VX: .rgss2a, VX Ace: .rgss3a",
                    "Unknown RGSSAD file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                switch (currentArchiveVersion)
                {
                    case RPGMakerVersion.Xp:
                    case RPGMakerVersion.Vx:
                        currentArchive = new RGSSADv1(inputFilePath);
                        break;
                    case RPGMakerVersion.VxAce:
                        currentArchive = new RGSSADv3(inputFilePath);
                        break;
                }
            }
            catch (InvalidArchiveException)
            {
                MessageBox.Show("Archive is invalid or corrupted. " +
                    "Reading failed. Please create an issue: https://github.com/uuksu/RPGMakerDecrypter/issues",
                    "Invalid archive",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (UnsupportedArchiveException)
            {
                MessageBox.Show("Archive is not supported or it is corrupted. " +
                    "Please create an issue: https://github.com/uuksu/RPGMakerDecrypter/issues", 
                    "Archive not supported",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                var logFilePath = ExceptionLogger.LogException(ex);
                MessageBox.Show("Unexpected error occurred while trying to extract the archive. " +
                    $"Error log has been written to '{logFilePath}' " +
                    "Please create an issue and include the log contents there: https://github.com/uuksu/RPGMakerDecrypter/issues"
                    , "Archive corrupted",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (ArchivedFile archivedFile in currentArchive.ArchivedFiles)
            {
                archivedFilesListBox.Items.Add(archivedFile.Name);
            }

            SetClickableElementsEnabled(true);

            statusLabel.Text = "Archive opened successfully.";
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

            currentArchiveVersion = RPGMakerVersion.Unknown;
            inputFilePath = null;

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
            catch (Exception ex)
            {
                var logFilePath = ExceptionLogger.LogException(ex);
                MessageBox.Show("Unexpected error occurred while trying to extract the archive. " +
                    $"Error log has been written to '{logFilePath}' " +
                    "Please create an issue and include the log contents there: https://github.com/uuksu/RPGMakerDecrypter/issues"
                    , "Archive corrupted",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                statusLabel.Text = $"Archive extraction failed!";
            }

            if (generateProjectCheckBox.Checked)
            {
                var outputSameAsArchivePath = new FileInfo(inputFilePath).Directory.FullName == new DirectoryInfo(outputDirectoryPath).FullName;
                ProjectGenerator.GenerateProject(currentArchiveVersion, outputDirectoryPath, !outputSameAsArchivePath);
            }

            statusLabel.Text = "Archive extracted successfully.";
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
            catch (Exception ex)
            {
                var logFilePath = ExceptionLogger.LogException(ex);
                MessageBox.Show("Unexpected error occurred while trying to extract the archive. " +
                    $"Error log has been written to '{logFilePath}' " +
                    "Please create an issue and include the log contents there: https://github.com/uuksu/RPGMakerDecrypter/issues"
                    , "Archive corrupted",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                statusLabel.Text = $"Extracting {fileName} failed!";

                return;
            }

            statusLabel.Text = $"Extracted {fileName} successfully.";
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
