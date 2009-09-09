﻿using System;
using System.Collections.ObjectModel;
using System.IO;

namespace CDP.ViewModel
{
    public class Options : Core.ViewModelBase
    {
        public Header Header { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand ApplyCommand { get; private set; }

        // Steam.
        public string SteamExeFullPath { get; set; }
        public string SteamAccountName { get; set; }
        public string SteamAdditionalLaunchParameters { get; set; }
        public DelegateCommand BrowseForSteamExeCommand { get; private set; }
        public ObservableCollection<string> SteamAccountNames { get; private set; }

        private readonly INavigationService navigationService;
        private readonly Core.ISettings settings;
        private readonly Core.IFileSystem fileSystem;

        public Options()
            : this(NavigationService.Instance, Core.Settings.Instance, new Core.FileSystem())
        {
        }

        public Options(INavigationService navigationService, Core.ISettings settings, Core.IFileSystem fileSystem)
        {
            this.navigationService = navigationService;
            this.settings = settings;
            this.fileSystem = fileSystem;
        }

        public override void Initialise()
        {
            Header = new Header();
            CancelCommand = new DelegateCommand(CancelCommandExecute);
            ApplyCommand = new DelegateCommand(ApplyCommandExecute);
            BrowseForSteamExeCommand = new DelegateCommand(BrowseForSteamExeCommandExecute);
            SteamAccountNames = new ObservableCollection<string>();
            SteamExeFullPath = (string)settings["SteamExeFullPath"];
            SteamAccountName = (string)settings["SteamAccountName"];
            SteamAdditionalLaunchParameters = (string)settings["SteamAdditionalLaunchParameters"];
            UpdateSteamAccountNames();
        }

        public override void Initialise(object parameter)
        {
            throw new NotImplementedException();
        }

        public void CancelCommandExecute()
        {
            navigationService.Home();
        }

        public void ApplyCommandExecute()
        {
            settings["SteamExeFullPath"] = SteamExeFullPath;
            settings["SteamAccountName"] = SteamAccountName;
            settings["SteamAdditionalLaunchParameters"] = SteamAdditionalLaunchParameters;
            navigationService.Home();
        }

        public void BrowseForSteamExeCommandExecute()
        {
            string initialPath = null;

            if (File.Exists(SteamExeFullPath))
            {
                initialPath = Path.GetDirectoryName(SteamExeFullPath);
            }

            string newSteamExeFullPath = navigationService.BrowseForFile("Steam.exe", initialPath);

            if (newSteamExeFullPath != null)
            {
                SteamExeFullPath = newSteamExeFullPath;
                OnPropertyChanged("SteamExeFullPath");
                UpdateSteamAccountNames();
            }
        }

        public void UpdateSteamAccountNames()
        {
            SteamAccountNames.Clear();

            if (string.IsNullOrEmpty(SteamExeFullPath) || !File.Exists(SteamExeFullPath))
            {
                SteamAccountName = null;
                return;
            }

            string steamAppsPath = fileSystem.PathCombine(Path.GetDirectoryName(SteamExeFullPath), "SteamApps");

            foreach (string folder in fileSystem.GetFolderNames(steamAppsPath))
            {
                SteamAccountNames.Add(folder);
            }
        }
    }
}