using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    public class UserFactoryDTO : AbstractFactoryDTO<User, UserDTO>
   
    {
        public override UserDTO GetDTO(User user)
        {
            return new UserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Password = user.Password,
            };
        }

        public override User GeTModel(UserDTO userDTO)
        {
            return new User()
            {
                Id = userDTO.Id,
                Email = userDTO.Email,
                Username = userDTO.Username,
                Password = userDTO.Password
            };
        }
    }
}