using System.Linq;
using ZenChat.Friends;
using ZenChat.Models;
using ZenChat.ZenChatService;

namespace ZenChat.Chat
{
	public class EditGroupChatViewModel
	{
		private ChatRoom _chatroom;
		public AllFriendsViewModel AllFriendsViewModel {get; private set; }

		public EditGroupChatViewModel(ChatRoom chatroom)
		{
			_chatroom = chatroom;
			Constructor();
		}
		public void AddUser()
		{
			var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);
			foreach (var user in AllFriendsViewModel.MyFriends.Where(n => n.IsSelectet))
			{
				client.InviteToChatRoomAsync(Session.UserID, user.User.PhoneNumber, _chatroom.Id);
			} 
		}

		public void DeleteUser(User user)
		{

		}

		private async void Constructor()
		{
			var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);
			var friends = await client.GetFriendsAsync(Session.UserID);
			AllFriendsViewModel = new AllFriendsViewModel(AddUser, () => true, DeleteUser, friends, true, true, false, false);
		}
	}
}
