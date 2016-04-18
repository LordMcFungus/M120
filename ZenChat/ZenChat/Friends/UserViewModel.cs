using System;
using Microsoft.Practices.Prism.Commands;
using ZenChat.ZenChatService;

namespace ZenChat.Friends
{
	public class UserViewModel
	{
		public UserViewModel(User friend, Action<User> removeUser)
		{
			User = friend;
			RemoveUserCommand = new DelegateCommand(() => removeUser(User));
		}

		public User User { get; }

		public bool IsSelectet { get; set; }

		public DelegateCommand RemoveUserCommand { get; }

	}
}