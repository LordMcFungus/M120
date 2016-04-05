// Copyright (c) 2016 
// All rights reserved
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZenChat.ZenChatService;

namespace ZenChat.Friends
{
	class AllFriendsViewModel
	{
		private ObservableCollection<User> _myFriends;

		public ObservableCollection<User> MyFriends
		{
			get
			{
				return _myFriends;
			}
			set
			{
				_myFriends = value;
			}
		}
	}
}
