using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Debug = System.Diagnostics.Debug;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NoteApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            GetFile();
        }


        private void Submit(object sender, RoutedEventArgs e)
        {
            var name = CreateNoteFile(this.RebCustom.Text);
            var content = ReadNoteFile(name);
            Debug.WriteLine(content);
        }
        private static string CreateNoteFile(string input)
        {
            var name = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-tt") + ".txt";
            var storageFolder = ApplicationData.Current.LocalFolder;
            var noteFile = storageFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting).GetAwaiter().GetResult();
            FileIO.WriteTextAsync(noteFile, input).GetAwaiter().GetResult();
            return name;
        }

        private static string ReadNoteFile(string fileName)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var sampleFile = storageFolder.GetFileAsync(fileName).GetAwaiter().GetResult();
            Debug.WriteLine(sampleFile.Path);
            var content = FileIO.ReadTextAsync(sampleFile).GetAwaiter().GetResult();
            return content;
        }

        private async void GetFile()
        {
            var appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var assets = ApplicationData.Current.LocalFolder;
            var files = await assets.GetFilesAsync();
            if (FileManager.Children.Count > 0)
            {
                FileManager.Children.RemoveAt(FileManager.Children.Count - 1);
            }
            foreach (var file in files)
            {
                var content = ReadNoteFile(file.Name);
                var textBlock = new TextBlock();
                textBlock.Text = file.Name + " - " + content;
                textBlock.Width = 400;
                textBlock.IsTextSelectionEnabled = true;
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.Name = file.Name;
                FileManager.Children.Add(textBlock);
            }
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            GetFile();
        }
    }

}
