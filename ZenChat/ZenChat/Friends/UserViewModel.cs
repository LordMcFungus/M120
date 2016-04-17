using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;

namespace ZenChat.Friends
{
	class UserViewModel
	{
		public ZenChatService.User User { get; set; }
		public DelegateCommand RemoveUsercommand{get; set;}

		public UserViewModel()
		{
			RemoveUsercommand = new DelegateCommand();
		}

	}
