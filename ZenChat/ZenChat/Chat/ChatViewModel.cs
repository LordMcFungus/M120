// Copyright (c) 2016 
// All rights reserved

using System;
using System.Collections.Generic;
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
		private PrivateConversation _privateChat;
		private ChatRoom _chatRoom;
		private string _newMessageText;

		public ChatViewModel()
		{
			SendMessageCommand = new DelegateCommand(SendMessage, CanSendMessage);
		}

		private bool CanSendMessage()
		{
			return !string.IsNullOrEmpty(NewMessageText);
		}

		private void SendMessage()
		{
			var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);
			if (Chatroom != null)
			{
				client.WriteGroupChatMessageAsync(Session.UserID, Chatroom.Id, NewMessageText);
			}
			else
			{
				client.WritePrivateChatMessageAsync(Session.UserID, PrivateChat.Members.First(m => !Equals(m.PhoneNumber, Session.PhoneNumber)).PhoneNumber, NewMessageText);
			}

			NewMessageText = string.Empty;
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

			}
		}

		private void LoadMessages(IEnumerable<ChatMessage> messages)
		{
			var orderedMessages = messages.OrderBy(m => m.Created);
			LastSentMessage = orderedMessages.First().Created;
			LastSentUser = orderedMessages.First().Author;

			//Mark all Messages as received
			var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);
			foreach (var message in orderedMessages.Where(m => m.ArrivedAt.Select(u => u.PhoneNumber).Contains(Session.PhoneNumber)))
			{
				client.ReceiveChatMessageAsync(Session.UserID, message.Id);
			}

			UnreadMessages = orderedMessages.Count(m => !m.ReadBy.Select(u => u.PhoneNumber).Contains(Session.PhoneNumber));
		}

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
				
			}
		}

		public DateTime? LastSentMessage { get; private set; }

		public User LastSentUser { get; private set; }

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

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}