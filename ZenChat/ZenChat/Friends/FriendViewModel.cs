using System;
using Microsoft.Practices.Prism.Commands;
using ZenChat.ZenChatService;

namespace ZenChat.Friends
{
	public class FriendViewModel
	{
		public FriendViewModel(User friend, Action<User> removeUser)
		{
			User = friend;
			RemoveUserCommand = new DelegateCommand(() => removeUser(User));
		}

		public User User { get; }

		public DelegateCommand RemoveUserCommand { get; }

	}
}