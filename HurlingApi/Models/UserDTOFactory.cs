using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    public class UserDTOFactory : AbstractFactoryDTO<User, UserDTO>
    {
        public override UserDTO GetDTO(User model)
        {
            return new UserDTO()
            {
                Id = model.Id,
                Email = model.Email,
                Username = model.Username,
                Password = model.Password,
            };
        }

        public override User GeTModel(UserDTO dto)
        {
            return new User()
            {
                Id = dto.Id,
                Email = dto.Email,
                Username = dto.Username,
                Password = dto.Password
            };
        }
    }
}