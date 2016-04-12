using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenChat.Friends
{
	class FriendModel
	{
		private string _name;
		private string _phoneNumber;

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string PhoneNumber
		{
			get { return _phoneNumber; }
			set { _phoneNumber = value; }
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name">Name of the User</param>
		/// <param name="phoneNumber">PhoneNumber of the User</param>
		public FriendModel(string name, string phoneNumber)
		{
			Name = name;
			PhoneNumber = phoneNumber;
		}
	}
}
