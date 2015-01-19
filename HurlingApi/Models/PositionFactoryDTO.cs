using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    public class PositionFactoryDTO : AbstractFactoryDTO<Position, PositionDTO>
    {
        public override PositionDTO GetDTO(Position model)
        {
            return new PositionDTO()
            {
                Id = model.Id,
                Name = model.Name 
            };
        }

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