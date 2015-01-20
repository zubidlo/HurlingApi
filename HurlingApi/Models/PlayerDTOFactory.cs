using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    /// <summary></summary>
    public class PlayerDTOFactory : AbstractFactoryDTO<Player, PlayerDTO>
    {
        /// <summary></summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override PlayerDTO GetDTO(Player model)
        {
            return new PlayerDTO
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                GaaTeam = model.GaaTeam,
                LastWeekPoints = model.LastWeekPoints,
                OverallPoints = model.OverallPoints,
                Price = model.Price,
                Rating = model.Rating,
                Injured = model.Injured,
                PositionId = model.PositionId
            };
        }

        /// <summary></summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override Player GeTModel(PlayerDTO dto)
        {
            return new Player
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                GaaTeam = dto.GaaTeam,
                LastWeekPoints = dto.LastWeekPoints,
                OverallPoints = dto.OverallPoints,
                Price = dto.Price,
                Rating = dto.Rating,
                Injured = dto.Injured,
                PositionId = dto.PositionId
            };
        }
    }
}