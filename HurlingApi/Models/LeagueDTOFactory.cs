using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    /// <summary></summary>
    public class LeagueDTOFactory : AbstractFactoryDTO<League, LeagueDTO>
    {
        /// <summary></summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary></summary>
        /// <param name="dto"></param>
        /// <returns></returns>
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