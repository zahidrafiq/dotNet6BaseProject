using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Service
{
    public interface IViewModel<TEntity, UEntity>
    {
        TEntity ToServiceModel(UEntity inputModel);
        UEntity FromServiceModel(TEntity inputModel);
        IEnumerable<UEntity> FromServiceModelList(IEnumerable<TEntity> inputModel);
        IEnumerable<TEntity> ToServiceModelList(IEnumerable<UEntity> inputModel);
    }
}
