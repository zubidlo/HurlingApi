using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurlingApi.Models
{
    /// <summary></summary>
    /// <typeparam name="MODEL"></typeparam>
    /// <typeparam name="DTO"></typeparam>
    public abstract class AbstractFactoryDTO<MODEL,DTO>
        where MODEL : class
        where DTO : class
    {
        /// <summary</summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract DTO GetDTO(MODEL model);

        /// <summary></summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract MODEL GeTModel(DTO dto);
        
        /// <summary></summary>
        /// <param name="models"></param>
        /// <returns></returns>
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