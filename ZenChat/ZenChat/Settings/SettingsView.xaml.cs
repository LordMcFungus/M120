// Copyright (c) 2016 
// All rights reserved

using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZenChat.Settings
{
	/// <summary>
	///     An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SettingsView : Page
	{
		private readonly SettingsViewModel ViewModel;

		/// <summary>
		/// </summary>
		public SettingsView()
		{
			InitializeComponent();
			ViewModel = new SettingsViewModel();
			DataContext = ViewModel;
		}
	}
}