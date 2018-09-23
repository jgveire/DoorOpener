namespace DoorService
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.Background;
    using Windows.Storage;

    public sealed class StartupTask : IBackgroundTask
    {
        private bool _isClosing;
        private BackgroundTaskDeferral _deferral;
        private PressButtonCommand _pressButtonCommand;
        private PressButtonEventHandler _pressButtonEventHandler;
        private FileSystemWatcher _watcher;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += TaskInstance_Canceled;

            _pressButtonCommand = new PressButtonCommand();
            _pressButtonEventHandler = new PressButtonEventHandler();
            _pressButtonEventHandler.ButtonPressed += ButtonPressed;

            var localFolder = ApplicationData.Current.LocalFolder;
            var fileName = $"{localFolder.Path}\\Watch.txt";
            
            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, "watch");
            }

            _watcher = new FileSystemWatcher(localFolder.Path);
            _watcher.Changed += FileChanged;
            _watcher.Created += FileCreated;
            _watcher.EnableRaisingEvents = true;

            while (!_isClosing)
            {
                
            }

            _deferral.Complete();
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            _isClosing = true;
        }

        private void FileCreated(object sender, FileSystemEventArgs e)
        {
            Task.Run(() => _pressButtonCommand.Execute());
        }

        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            Task.Run(() => _pressButtonCommand.Execute());
        }

        private void ButtonPressed(object sender, EventArgs e)
        {
            Task.Run(() => _pressButtonCommand.Execute());
        }
    }
}
