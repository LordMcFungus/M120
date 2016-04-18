using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using ZenChat.Friends;
using ZenChat.Models;
using ZenChat.ZenChatService;

namespace ZenChat.Chat
{
	class EditGroupChatViewModel
	{
		public AllFriendsViewModel AllFriendsViewModel {get; private set; }
		public DelegateCommand AddUser { get; set; }

		public  void EditGroupChat(ChatRoom chatroom)
		{
	
		}

		public string ChatTopic { get; set; }

		public void AddUseres()
		{
			var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);
			var chatroom = client.CreateChatRoomAsync(Session.UserID, ChatTopic);
			foreach (var user in AllFriendsViewModel.MyFriends.Where(n => n.IsSelectet == true))
			{
				client.InviteToChatRoomAsync(Session.UserID, user.User.PhoneNumber, chatroom.Id);
			} 
		}

		public void DeleteUseres(User user)
		{

		}

		private async void Constructor()
		{
			var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);
			var friends = await client.GetFriendsAsync(Session.UserID);
			AddUser = new DelegateCommand(AddUseres);
			AllFriendsViewModel = new AllFriendsViewModel(null, null, DeleteUseres,friends,true,false,false);
		}
	}
}
