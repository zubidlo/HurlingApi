using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    public class MessageDTOFactory : AbstractFactoryDTO<Message, MessageDTO>
    {
        public override MessageDTO GetDTO(Message model)
        {
            return new MessageDTO
            {
                Id = model.Id,
                Text = model.Text,
                UserId = model.UserId,
                Created = model.Created
            };
        }

        public override Message GeTModel(MessageDTO dto)
        {
            return new Message
            {
                Id = dto.Id,
                Text = dto.Text,
                UserId = dto.UserId,
                Created = DateTime.Now
            };
        }
    }
}