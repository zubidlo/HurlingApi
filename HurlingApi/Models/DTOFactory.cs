using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HurlingApi.Models
{
    /// <summary>
    /// Object mapper
    /// </summary>
    public class DTOFactory
    {
        private HurlingModelContext db = new HurlingModelContext();

        /// <summary>
        /// Makes new UserDTO object from given User object
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserDTO UserDTO(User user)
        {
            var userDTO = new UserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Password = user.Password,
            };

            return userDTO;
        }

        /// <summary>
        /// Makes new User object from given UserDTO object
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public User User(UserDTO userDTO)
        {
            return new User()
            {
                Id = userDTO.Id,
                Email = userDTO.Email,
                Username = userDTO.Username,
                Password = userDTO.Password
            };
        }

        /// <summary>
        /// Makes new collection of UserDTOs from given collection of Users
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public IEnumerable<UserDTO> UserDTOs(IEnumerable<User> users)
        {
            var userDTOs = new HashSet<UserDTO>();

            foreach (var user in users)
            {
                userDTOs.Add(UserDTO(user));
            }

            return userDTOs;
        }

        public PositionDTO PositionDTO(Position pos)
        {
            return new PositionDTO()
            {
                Id = pos.Id,
                Name = pos.Name,
            };
        }

        public IEnumerable<PositionDTO> PositionDTOs(IEnumerable<Position> positions)
        {
            var positionDTOSet = new HashSet<PositionDTO>();
            
            foreach(var position in positions)
            {
                positionDTOSet.Add(PositionDTO(position));
            }

            return positionDTOSet;
        }


        

    }

   
}