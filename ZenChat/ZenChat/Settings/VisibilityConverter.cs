using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ZenChat.Settings
{
	/// <summary>
	/// Convert a Boolean status to Visibility.Visible (true) or Visibility.Collapsed (false).
	/// Assists in avoiding having the view model keep references to UI types.
	/// </summary>
	public class VisibilityConverter : IValueConverter
	{
		/// <summary>
		/// Convert a boolean into a member of the Visibility enum, true for Visible, false for Collapsed.
		/// </summary>
		/// <param name="value">The bool passed in</param>
		/// <param name="targetType">Ignored.</param>
		/// <param name="parameter">Ignored</param>
		/// <param name="language">Ignored</param>
		/// <returns>Visibility.Visible if value was a true bool, otherwise Visibility.Collapsed</returns>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (!(value is bool)) return Visibility.Collapsed;

			if ((bool)value)
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}

		/// <summary>
		/// Not used, converter is not intended for two-way binding. 
		/// </summary>
		/// <param name="value">Ignored</param>
		/// <param name="targetType">Ignored</param>
		/// <param name="parameter">Ignored</param>
		/// <param name="language">Ignored</param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}