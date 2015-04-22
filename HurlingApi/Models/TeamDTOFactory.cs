using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    public class TeamDTOFactory : AbstractFactoryDTO<Team, TeamDTO>
    {
        public override TeamDTO GetDTO(Team model)
        {
            return new TeamDTO
            {
                Id = model.Id,
                Name = model.Name,
                OverAllPoints = model.OverAllPoints,
                LastWeekPoints = model.LastWeekPoints,
                Budget = model.Budget,
                LeagueId = model.LeagueId,
                UserId = model.UserId
            };
        }

        public override Team GeTModel(TeamDTO dto)
        {
            return new Team
            {
                Id = dto.Id,
                Name = dto.Name,
                OverAllPoints = dto.OverAllPoints,
                LastWeekPoints = dto.LastWeekPoints,
                Budget = dto.Budget,
                LeagueId = dto.LeagueId,
                UserId = dto.UserId
            };
        }
    }
}