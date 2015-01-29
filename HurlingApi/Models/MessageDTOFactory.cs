using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageDTOFactory : AbstractFactoryDTO<Message, MessageDTO>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
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