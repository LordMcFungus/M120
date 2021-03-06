﻿// Copyright (c) 2016 
// All rights reserved

using System;
using Microsoft.Practices.Prism.Commands;
using ZenChat.ZenChatService;

namespace ZenChat.Friends
{
	public class FriendViewModel
	{
		private readonly bool _allowSelection;
		private bool _isSelected;

		public FriendViewModel(User friend, Action<User> removeUser, bool allowSelection = false, bool displayRemove = true)
		{
			User = friend;
			RemoveUserCommand = new DelegateCommand(() => removeUser(User));
			_allowSelection = allowSelection;
			CanRemoveUser = displayRemove;
		}

		public User User { get; }

		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				if (!_allowSelection) return;
				_isSelected = value;
			}
		}

		public DelegateCommand RemoveUserCommand { get; }

		public bool CanRemoveUser { get; }
	}
}