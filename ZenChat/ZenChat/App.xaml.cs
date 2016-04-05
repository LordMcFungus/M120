﻿// Copyright (c) 2016 
// All rights reserved

using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ZenChat.Models;
using ZenChat.ZenChatService;

namespace ZenChat
{
	/// <summary>
	///     Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public sealed partial class App : Application
	{
		/// <summary>
		///     Initializes the singleton application object.  This is the first line of authored code
		///     executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			InitializeComponent();
			Suspending += OnSuspending;
		}

		/// <summary>
		///     Invoked when the application is launched normally by the end user.  Other entry points
		///     will be used such as when the application is launched to open a specific file.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			var rootFrame = Window.Current.Content as Frame;

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (rootFrame == null)
			{
				// Create a Frame to act as the navigation context and navigate to the first page
				rootFrame = new Frame();

				rootFrame.NavigationFailed += OnNavigationFailed;

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					//TODO: Load state from previously suspended application
				}

				// Place the frame in the current Window
				Window.Current.Content = rootFrame;
			}

			if (rootFrame.Content == null)
			{
				// When the navigation stack isn't restored navigate to the first page,
				// configuring the new page by passing required information as a navigation
				// parameter
				// var id = Windows.Storage.ApplicationData.Current.LocalSettings.Values["UID"] as int?;
				var id = 21;
				if (id == 0/*.HasValue*/)
				{
					rootFrame.Navigate(typeof(LoginRegisterPage), e.Arguments);
				}
				else
				{
					DoStuff(rootFrame, id/*.Value*/);
				}
			}
			// Ensure the current window is active
			Window.Current.Activate();
		}

		private static async void DoStuff(INavigate rootFrame, int id)
		{
			var client = new ZenChatServiceClient(ZenChatServiceClient.EndpointConfiguration.BasicHttpsBinding_ZenChatService);

			var user = await client.GetUserFromIdAsync(id);
			Session.Username = user.Name;
			Session.PhoneNumber = user.PhoneNumber;
			rootFrame.Navigate(typeof(MainPage));
		}

		/// <summary>
		///     Invoked when Navigation to a certain page fails
		/// </summary>
		/// <param name="sender">The Frame which failed navigation</param>
		/// <param name="e">Details about the navigation failure</param>
		private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		/// <summary>
		///     Invoked when application execution is being suspended.  Application state is saved
		///     without knowing whether the application will be terminated or resumed with the contents
		///     of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			//TODO: Save application state and stop any background activity
			deferral.Complete();
		}
	}
}