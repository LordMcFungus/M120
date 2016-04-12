// Copyright (c) 2016 
// All rights reserved

using System.Collections.ObjectModel;
using ZenChat.ZenChatService;

namespace ZenChat.Group
{
	internal class AllGroupsViewModel
	{
		private ObservableCollection<ChatRoom> _allGroupChats;

		public ObservableCollection<ChatRoom> AllGroupChats
		{
			get { return _allGroupChats; }
			set { _allGroupChats = value; }
		}

		public AllGroupsViewModel()
		{
				LoadGroupChats();
		}
		public async void LoadGroupChats()
		{
			var client = new ZenChatServiceClient(ZenChatServiceClient.EndpointConfiguration.BasicHttpsBinding_ZenChatService);
			var id = Windows.Storage.ApplicationData.Current.LocalSettings.Values["UID"] as int?;
			if (id.HasValue)
			{
				var chatrooms = await client.GetAllChatRoomsAsync(id.Value);
				foreach (var chatroom in chatrooms)
				{
					AllGroupChats.Add(chatroom);
				}
			}
		}
	}
}