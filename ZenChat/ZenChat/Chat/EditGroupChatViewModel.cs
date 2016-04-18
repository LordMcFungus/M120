using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ZenChat.Annotations;
using ZenChat.Friends;
using ZenChat.Models;
using ZenChat.ZenChatService;

namespace ZenChat.Chat
{
	public class EditGroupChatViewModel : INotifyPropertyChanged
	{
		private readonly ChatRoom _chatroom;
		private AllFriendsViewModel _allFriendsViewModel;
		private AllFriendsViewModel _chatMembers;

		public AllFriendsViewModel AllFriendsViewModel
		{
			get { return _allFriendsViewModel; }
			private set
			{
				_allFriendsViewModel = value; 
				OnPropertyChanged();
			}
		}

		public AllFriendsViewModel ChatMembers
		{
			get { return _chatMembers; }
			private set
			{
				_chatMembers = value;
				OnPropertyChanged();
			}
		}

		public EditGroupChatViewModel(ChatRoom chatroom)
		{
			_chatroom = chatroom;
			Constructor();
		}
		public void AddUser()
		{
			var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);
			foreach (var user in AllFriendsViewModel.MyFriends.Where(n => n.IsSelected))
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
			AllFriendsViewModel = new AllFriendsViewModel(AddUser, () => true, null, friends, true, true, false, false);
			ChatMembers = new AllFriendsViewModel(() => {}, () => false, (u) => {}, _chatroom.Members.Where(user => user.PhoneNumber != Session.PhoneNumber),false,false,false,true);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
