﻿namespace EMS.Models
{
    public class Message
    {
   
            public int Id { get; set; }
            public string SenderId { get; set; }
            public string ReceiverId { get; set; }
            public string Text { get; set; }
            public DateTime Timestamp { get; set; }
            public bool IsRead { get; set; }
        
    }
}
