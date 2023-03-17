using System;
using System.Linq;
using System.IO;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using RPGMakerDecrypter.Decrypter;
using RPGMakerDecrypter.Decrypter.Exceptions;

namespace RPGMakerDecrypter.Gui.Gtk
{
    class MainWindow : Window
    {
#pragma warning disable CS0169, CS0649, IDE0044
        [UI] private Button fileButton = null;
        [UI] private Button extractAllButton = null;
        [UI] private Button aboutButton = null;
        
        [UI] private ListBox archivedFilesListBox = null;
        [UI] private Label statusLabel = null;
        [UI] private CheckButton generateProjectCheckBox = null;
        [UI] private Entry fileNameTextBox = null;
        [UI] private Entry sizeTextBox = null;
        [UI] private Button extractFileButton = null;
#pragma warning restore CS0649, IDE0044, CS0169

        private RPGMakerVersion currentArchiveVersion;
        private RGSSAD currentArchive;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            fileButton.Clicked += FileButton_Clicked;
            extractAllButton.Clicked += ExtractAll_Clicked;
            aboutButton.Clicked += About_Clicked;
            extractFileButton.Clicked += ExtractFile_Clicked;
            archivedFilesListBox.RowSelected += archivedFilesListBox_SelectedIndexChanged;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void FileButton_Clicked(object sender, EventArgs a)
        {
            FileChooserDialog openFileDialog = new FileChooserDialog("Open File", this, FileChooserAction.Open);
            openFileDialog.AddButton(Stock.Cancel, ResponseType.Cancel);
            openFileDialog.AddButton(Stock.Open, ResponseType.Ok);
            openFileDialog.DefaultResponse = ResponseType.Ok;
            openFileDialog.SelectMultiple = false;
            {
                FileFilter rgssFilter = new FileFilter();
                rgssFilter.Name = "All RGSS Archives";
                rgssFilter.AddPattern("*.rgssad");
                rgssFilter.AddPattern("*.rgss2a");
                rgssFilter.AddPattern("*.rgss3a");
                openFileDialog.AddFilter(rgssFilter);
            }
            {
                FileFilter xpFilter = new FileFilter();
                xpFilter.Name = "RPG Maker XP Encrypted Archive (.rgssad)";
                xpFilter.AddPattern("*.rgssad");
                openFileDialog.AddFilter(xpFilter);
            }
            {
                FileFilter vxFilter = new FileFilter();
                vxFilter.Name = "RPG Maker VX Encrypted Archive (.rgss2a)";
                vxFilter.AddPattern("*.rgss2a");
                openFileDialog.AddFilter(vxFilter);
            }

            {
                FileFilter vxaFilter = new FileFilter();
                vxaFilter.Name = "RPG Maker VX Ace Encrypted Archive (.rgss3a)";
                vxaFilter.AddPattern("*.rgss3a");
                openFileDialog.AddFilter(vxaFilter);
            }

            {
                FileFilter allFilter = new FileFilter();
                allFilter.Name = "All Files";
                allFilter.AddPattern("*.*");
                openFileDialog.AddFilter(allFilter);
            }

            ResponseType response = (ResponseType) openFileDialog.Run();
            if (response != ResponseType.Ok) {
                openFileDialog.Dispose();
                return;
            }

            Reset();

            string inputFilePath = openFileDialog.Filename;
            openFileDialog.Dispose();

            currentArchiveVersion = RGSSAD.GetVersion(inputFilePath);
            if (currentArchiveVersion == RPGMakerVersion.Invalid)
            {
                MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Close, "Invalid input file.");
                md.Run();
                md.Destroy();
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
                MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "Archive is invalid or corrupted. Reading failed.");
                md.Run();
                md.Destroy();
                return;
            }
            catch (UnsupportedArchiveException)
            {
                MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Close, "Archive is not supported or it is corrupted.");
                md.Run();
                md.Destroy();
                return;
            }
            catch (Exception)
            {
                MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Close, "Something went wrong with reading or extraction. Archive is likely invalid or corrupted.");
                md.Run();
                md.Destroy();
                return;
            }

            int rowNum = 0;
            foreach (ArchivedFile archivedFile in currentArchive.ArchivedFiles)
            {
                ListBoxRow r = new ListBoxRow();
                Label l = new Label();
                l.Text = archivedFile.Name;
                l.Xalign = 0.0F;
                r.Add(l);
                archivedFilesListBox.Add(r);
                if (rowNum == 0) {
                    archivedFilesListBox.SelectRow(r);
                }
                rowNum++;
            }
            archivedFilesListBox.ShowAll();
            SetClickableElementsEnabled(true);

            statusLabel.Text = "Archive opened succesfully.";
        }

        private void SetClickableElementsEnabled(bool enabled)
        {
            archivedFilesListBox.Sensitive = enabled;
            extractAllButton.Sensitive = enabled;
        }

        private void Reset()
        {
            archivedFilesListBox.Forall((child) => archivedFilesListBox.Remove(child));
            archivedFilesListBox.ShowAll();
            SetClickableElementsEnabled(false);
            extractFileButton.Sensitive = false;

            currentArchiveVersion = RPGMakerVersion.Invalid;

            currentArchive?.Dispose();
        }

        private void ExtractAll_Clicked(object sender, EventArgs a)
        {
            if (currentArchive == null || currentArchive.ArchivedFiles.Count == 0)
            {
                return;
            }
            FileChooserDialog openFolderDialog = new FileChooserDialog("Choose Folder", this, FileChooserAction.SelectFolder);
            openFolderDialog.AddButton(Stock.Cancel, ResponseType.Cancel);
            openFolderDialog.AddButton(Stock.Save, ResponseType.Ok);
            openFolderDialog.DefaultResponse = ResponseType.Ok;
            openFolderDialog.SelectMultiple = false;

            ResponseType response = (ResponseType) openFolderDialog.Run();
            if (response != ResponseType.Ok) {
                openFolderDialog.Dispose();
                return;
            }

            string outputDirectoryPath = openFolderDialog.CurrentFolder;
            openFolderDialog.Dispose();

            try
            {
                currentArchive.ExtractAllFiles(outputDirectoryPath, true);
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong with extraction. Archive is likely invalid or corrupted.");
                return;
            }

            if (generateProjectCheckBox.Active)
            {
                ProjectGenerator.GenerateProject(currentArchiveVersion, outputDirectoryPath);
            }

            statusLabel.Text = "Archive extracted succesfully.";
        }

        private void About_Clicked(object sender, EventArgs a)
        {
            var win = new AboutWindow();
            win.Run();
            win.Dispose();
        }

        private void ExtractFile_Clicked(object sender, EventArgs a)
        {
            if (currentArchive == null || !currentArchive.ArchivedFiles.Any() || archivedFilesListBox.SelectedRow == null)
            {
                return;
            }

            ArchivedFile archivedFile = currentArchive.ArchivedFiles[archivedFilesListBox.SelectedRow.Index];

            string fileName = archivedFile.Name.Split('\\').Last();
            string extension = fileName.Split('.').Last();

            FileChooserDialog saveFileDialog = new FileChooserDialog("Save File", this, FileChooserAction.Save);
            saveFileDialog.AddButton(Stock.Cancel, ResponseType.Cancel);
            saveFileDialog.AddButton(Stock.Save, ResponseType.Ok);
            saveFileDialog.DefaultResponse = ResponseType.Ok;
            saveFileDialog.SelectMultiple = false;
            saveFileDialog.CurrentName = fileName;
            var filter = new FileFilter();
            filter = new FileFilter();
            filter.Name = $"Data file (*.{extension})";
            filter.AddPattern($"*.{extension}");
            saveFileDialog.AddFilter(filter);

            ResponseType response = (ResponseType) saveFileDialog.Run();
            if (response != ResponseType.Ok) {
                saveFileDialog.Dispose();
                return;
            }

            FileInfo fileInfo = new FileInfo(saveFileDialog.Filename);
            saveFileDialog.Dispose();
            
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

        private void archivedFilesListBox_SelectedIndexChanged(object sender, EventArgs a)
        {
            extractFileButton.Sensitive = false;

            if (currentArchive == null || !currentArchive.ArchivedFiles.Any() || archivedFilesListBox.SelectedRow == null)
            {
                return;
            }

            ArchivedFile archivedFile = currentArchive.ArchivedFiles[archivedFilesListBox.SelectedRow.Index];

            fileNameTextBox.Text = archivedFile.Name;
            sizeTextBox.Text = archivedFile.Size.ToString();

            extractFileButton.Sensitive = true;
        }
    }
}
