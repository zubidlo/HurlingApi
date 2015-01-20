using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    /// <summary></summary>
    public class PositionDTOFactory : AbstractFactoryDTO<Position, PositionDTO>
    {
        /// <summary></summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override PositionDTO GetDTO(Position model)
        {
            return new PositionDTO()
            {
                Id = model.Id,
                Name = model.Name 
            };
        }

        /// <summary></summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override Position GeTModel(PositionDTO dto)
        {
            return new Position()
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
    }
}