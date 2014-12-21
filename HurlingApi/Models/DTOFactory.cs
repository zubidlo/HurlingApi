using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    public class DTOFactory
    {
        public PositionDTO GetPosition(Position pos)
        {
            var playerIdList = new HashSet<int>();
            
            foreach(Player player in pos.Players)
            {
                playerIdList.Add(player.Id);
            }

            return new PositionDTO()
            {
                Id = pos.Id,
                Name = pos.Name,
                PlayerIds = playerIdList
            };
        }

        public List<PositionDTO> GetAllPositions(List<Position> positions)
        {
            var positionDTOList = new List<PositionDTO>();
            
            foreach(var position in positions)
            {
                positionDTOList.Add(GetPosition(position));
            }

            return positionDTOList;
        }

    }

   
}