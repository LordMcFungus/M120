namespace ZenChat.Friends
{
	internal class FriendModel
	{
		/// <summary>
		/// </summary>
		/// <param name="name">Name of the User</param>
		/// <param name="phoneNumber">PhoneNumber of the User</param>
		public FriendModel(string name, string phoneNumber)
		{
			Name = name;
			PhoneNumber = phoneNumber;
		}

		public string Name { get; set; }

		public string PhoneNumber { get; set; }
	}
}