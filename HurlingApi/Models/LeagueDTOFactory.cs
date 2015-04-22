using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    public class LeagueDTOFactory : AbstractFactoryDTO<League, LeagueDTO>
    {
        public override LeagueDTO GetDTO(League model)
        {
            return new LeagueDTO
            {
                Id = model.Id,
                Name = model.Name,
                NextFixtures = model.NextFixtures,
                Week = model.Week
            };
        }

        public override League GeTModel(LeagueDTO dto)
        {
            return new League
            {
                Id = dto.Id,
                Name = dto.Name,
                NextFixtures = dto.NextFixtures,
                Week = dto.Week
            };
        }
    }
}