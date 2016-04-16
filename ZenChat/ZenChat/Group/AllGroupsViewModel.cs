// Copyright (c) 2016 
// All rights reserved

using System.Collections.ObjectModel;
using Windows.Storage;
using ZenChat.ZenChatService;

namespace ZenChat.Group
{
	internal class AllGroupsViewModel
	{
		public AllGroupsViewModel()
		{
			LoadGroupChats();
		}

		public ObservableCollection<ChatRoom> AllGroupChats { get; set; }

		public async void LoadGroupChats()
		{
			var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);
			var id = ApplicationData.Current.LocalSettings.Values["UID"] as int?;
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