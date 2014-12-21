using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    public class DTOFactory
    {
        public PositionDTO GetPositionDTO(Position pos)
        {
           return new PositionDTO()
            {
                Id = pos.Id,
                Name = pos.Name,
            };
        }

        public ICollection<PositionDTO> GetAllPositionDTOs(ICollection<Position> positions)
        {
            var positionDTOList = new HashSet<PositionDTO>();
            
            foreach(var position in positions)
            {
                positionDTOList.Add(GetPositionDTO(position));
            }

            return positionDTOList;
        }

    }

   
}