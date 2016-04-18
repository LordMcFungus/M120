using ZenChat.ZenChatService;

namespace ZenChat.Models
{
	public class Session
	{
		public static string Username { get; set; }
		public static string PhoneNumber { get; set; }
		public static int UserID { get; set; }
		public static ZenClient Client { get; set; }
	}
}