using System;
using Microsoft.AspNetCore.Mvc;
using PocketMonstersAPI.Models;

namespace PocketMonstersAPI.Controllers
{
    public interface IPaginationMessagesController
    {
        Task<ActionResult<List<MessageList>>> GetMessageListJustData(int pg);

        Task<ActionResult> GetMessageJustPage(int pg);

        Task<IResult> PostMessageList(MessageList mess);

        Task<IResult> DeleteMesage(MessageOutput mess);
    }
}

