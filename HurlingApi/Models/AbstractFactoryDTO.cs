using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    public abstract class AbstractFactoryDTO<Model, DTO>
        where Model : class
        where DTO : class
    {
        public abstract DTO GetDTO(Model model);
        public abstract Model GeTModel(DTO dto);
        public IEnumerable<DTO> GetDTOCollection(IEnumerable<Model> models)
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