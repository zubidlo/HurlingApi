using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    public abstract class AbstractFactoryDTO<MODEL,DTO>
        where MODEL : User
        where DTO : UserDTO
    {
        public abstract DTO GetDTO(MODEL model);
        public abstract MODEL GeTModel(DTO dto);
        
        public IEnumerable<DTO> GetCollection(IEnumerable<MODEL> models)
        {
            var DTOs = new HashSet<DTO>();
            foreach (var model in models)
            {
                DTOs.Add(GetDTO(model));
            }
            return DTOs;
        }
    }
}