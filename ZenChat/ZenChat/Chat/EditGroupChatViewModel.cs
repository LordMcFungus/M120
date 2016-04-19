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
		private ChatRoom _chatroom;
		private AllFriendsViewModel _allFriendsViewModel;
		private AllFriendsViewModel _chatMembers;

		public EditGroupChatViewModel(ChatRoom chatroom)
		{
			_chatroom = chatroom;
			Constructor();
		}

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

		public event PropertyChangedEventHandler PropertyChanged;

		public async void AddUser()
		{
			foreach (var user in AllFriendsViewModel.MyFriends.Where(n => n.IsSelected))
			{
				await Session.Client.InviteToChatRoomAsync(Session.UserID, user.User.PhoneNumber, _chatroom.Id);
			}

			Refresh();
		}

		public async void DeleteUser(User user)
		{
			await Session.Client.RemoveFromChatRoomAsync(Session.UserID, user.PhoneNumber, _chatroom.Id);
			Refresh();
		}

		private async void Refresh()
		{
			_chatroom = await Session.Client.GetChatRoomAsync(_chatroom.Id, Session.UserID);
			Constructor();
		}

		private async void Constructor()
		{
			AllFriendsViewModel = null;
			ChatMembers = null;
			var friends = await Session.Client.GetFriendsAsync(Session.UserID);
			AllFriendsViewModel = new AllFriendsViewModel(AddUser, () => true, null, friends.Where(f => !_chatroom.Members.Select(m => m.PhoneNumber).Contains(f.PhoneNumber)), true, true, false, false);
			ChatMembers = new AllFriendsViewModel(() => { }, () => false, DeleteUser,
				_chatroom.Members.Where(user => user.PhoneNumber != Session.PhoneNumber), false, false, false, true);
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}