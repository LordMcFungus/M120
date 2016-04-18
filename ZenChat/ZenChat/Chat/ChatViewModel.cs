// Copyright (c) 2016 
// All rights reserved

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Practices.Prism.Commands;
using ZenChat.Annotations;
using ZenChat.Models;
using ZenChat.ZenChatService;

namespace ZenChat.Chat
{
	public class ChatViewModel : INotifyPropertyChanged
	{
		private ChatRoom _chatRoom;
		private string _newMessageText;
		private PrivateConversation _privateChat;

		public ChatViewModel()
		{
			SendMessageCommand = new DelegateCommand(SendMessage, CanSendMessage);
		}

		public ChatRoom Chatroom
		{
			get { return _chatRoom; }
			set
			{
				_chatRoom = value;

				if (_chatRoom.Messages.Any())
				{
					LoadMessages(_chatRoom.Messages);
				}
				ChatName = _chatRoom.Topic;
			}
		}

		public ObservableCollection<ChatMessage> OrderedMessages { get; } = new ObservableCollection<ChatMessage>();

		public PrivateConversation PrivateChat
		{
			get { return _privateChat; }
			set
			{
				_privateChat = value;

				if (_privateChat.Messages.Any())
				{
					LoadMessages(_privateChat.Messages);
				}
				var users = _privateChat.Members.First(m => !Equals(m.PhoneNumber, Session.PhoneNumber));
				ChatName = users.Name;
			}
		}

		public DateTime? LastSentMessage { get; private set; }

		public User LastSentUser { get; private set; }

		public string ChatName { get; private set; }

		public int UnreadMessages { get; private set; }

		public string NewMessageText
		{
			get { return _newMessageText; }
			set
			{
				_newMessageText = value;
				OnPropertyChanged();
				SendMessageCommand.RaiseCanExecuteChanged();
			}
		}

		public DelegateCommand SendMessageCommand { get; }

		/// <summary>Tritt ein, wenn sich ein Eigenschaftswert ändert.</summary>
		public event PropertyChangedEventHandler PropertyChanged;

		private bool CanSendMessage()
		{
			return !string.IsNullOrEmpty(NewMessageText);
		}

		private async void SendMessage()
		{
			dynamic chat;
			if (Chatroom != null)
			{
				await Session.Client.WriteGroupChatMessageAsync(Session.UserID, Chatroom.Id, NewMessageText);
				chat = await Session.Client.GetChatRoomAsync(Chatroom.Id, Session.UserID);
			}
			else
			{
				await
					Session.Client.WritePrivateChatMessageAsync(Session.UserID,
						PrivateChat.Members.First(m => !Equals(m.PhoneNumber, Session.PhoneNumber)).PhoneNumber, NewMessageText);
				chat =
					await
						Session.Client.GetPrivateConversationAsync(Session.UserID,
							PrivateChat.Members.First(m => !Equals(m.PhoneNumber, Session.PhoneNumber)).PhoneNumber);
			}

			NewMessageText = string.Empty;

			LoadMessages(chat.Messages);
		}

		private void LoadMessages(IEnumerable<ChatMessage> messages)
		{
			OrderedMessages.Clear();

			foreach (var message in messages.OrderBy(m => m.Created))
			{
				OrderedMessages.Add(message);
			}

			LastSentMessage = OrderedMessages.Last().Created;
			LastSentUser = OrderedMessages.Last().Author;

			//Mark all Messages as received
			foreach (
				var message in OrderedMessages.Where(m => !m.ArrivedAt.Contains(Session.PhoneNumber)))
			{
				Session.Client.ReceiveChatMessageAsync(Session.UserID, message.Id);
			}

			UnreadMessages = OrderedMessages.Count(m => !m.ReadBy.Contains(Session.PhoneNumber));
		}

		public async void ReadMessages()
		{
			foreach (
				var message in OrderedMessages.Where(m => !m.ReadBy.Contains(Session.PhoneNumber)))
			{
				await Session.Client.ReadChatMessageAsync(Session.UserID, message.Id);
			}
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}