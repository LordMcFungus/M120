// Copyright (c) 2016 
// All rights reserved
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZenChat.Settings
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SettingsView : Page
	{
		private SettingsViewModel ViewModel;
		/// <summary>
		/// 
		/// </summary>
		public SettingsView()
		{
			this.InitializeComponent();
			ViewModel = new SettingsViewModel();
			DataContext = ViewModel;
		}
	}
}
