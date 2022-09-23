using System;
namespace PocketMonstersAPI.Models
{
    public class MessageList 
    {
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public string DisplayName { get; set; }

        public string UserID { get; set; }

        public string PictureLink { get; set; }

        public string Message { get; set; }

        public string Active  {get; set; } = "1";

        public string MessageID { get; set; }
    }
}

