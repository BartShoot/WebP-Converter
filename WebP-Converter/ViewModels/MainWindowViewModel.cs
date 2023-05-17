using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Management.Automation;
using System.Windows.Input;

namespace WebP_Converter.ViewModels
{
	public partial class MainWindowViewModel : ViewModelBase
	{
		[ObservableProperty]
		public string detectedImagesLabel;
		[ObservableProperty]
		public string folderDirectory;
		private string[] allPNGFiles;
		public string Greeting => "Welcome to Avalonia!";
		public ICommand Convert { get; set; }
		public ICommand OpenDirectory { get; set; }
		public async void OpenDirectoryFunction()
		{

			OpenFolderDialog open = new();
			FolderDirectory = await open.ShowAsync(new Window());
			allPNGFiles = Directory.GetFiles(FolderDirectory, "*.png");
			DetectedImagesLabel = $"You found {allPNGFiles.Length} files! {allPNGFiles[0]}";
		}
		
		public void ConvertToWebP()
		{
			PowerShell ps = PowerShell.Create();
			foreach (var img in allPNGFiles)
			{
				ps.AddCommand("cwebp").AddParameter("q", 80).AddArgument(Path.GetFileName(img)).AddParameter("o", Path.GetFileNameWithoutExtension(img)+".webp");
			}
			ps.Invoke();
		}

		public MainWindowViewModel()
		{
			OpenDirectory = new RelayCommand(OpenDirectoryFunction);
			Convert = new RelayCommand(ConvertToWebP);
		}
	}
}