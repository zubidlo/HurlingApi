using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HurlingApi.Models
{
    public class DTOFactory
    {
        private HurlingModelContext db = new HurlingModelContext();

        public PositionDTO GetPositionDTO(Position pos)
        {
           return new PositionDTO()
            {
                Id = pos.Id,
                Name = pos.Name,
            };
        }

        public UserDTO GetUserDTO(User user)
        {
            var userDTO = new UserDTO()
            {
                //Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Password = user.Password,
            };

            return userDTO;
        }

        public User GetUser(UserDTO userDTO)
        {
            return new User()
            {
                Email = userDTO.Email,
                Username = userDTO.Username,
                Password = userDTO.Password
            };
        }

        public IEnumerable<PositionDTO> GetAllPositionDTOs(IEnumerable<Position> positions)
        {
            var positionDTOSet = new HashSet<PositionDTO>();
            
            foreach(var position in positions)
            {
                positionDTOSet.Add(GetPositionDTO(position));
            }

            return positionDTOSet;
        }

        public IEnumerable<UserDTO> GetAllUserDTOs(IEnumerable<User> users)
        {
            var userDTOset = new HashSet<UserDTO>();

            foreach(var user in users)
            {
                userDTOset.Add(GetUserDTO(user));
            }

            return userDTOset;
        }

    }

   
}