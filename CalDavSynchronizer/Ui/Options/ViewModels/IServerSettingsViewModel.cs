using System;
using System.ComponentModel;
using CalDavSynchronizer.Contracts;

namespace CalDavSynchronizer.Ui.Options.ViewModels
{
  internal interface IServerSettingsViewModel : IOptionsSection, INotifyPropertyChanged
  {
    ServerAdapterType ServerAdapterType { get; set; }
    string EmailAddress { get; }
    string UserName { get; }
    string CalenderUrl { get; set; }
    bool UseAccountPassword { get; }
    string Password { get; }
  }
}