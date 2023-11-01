using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Service
{
    public interface IServiceModel<TEntity, UEntity>
    {
        TEntity ToDataModel(UEntity inputModel);
        UEntity FromDataModel(TEntity inputModel);
        IEnumerable<UEntity> FromDataModelList(IEnumerable<TEntity> inputModel);
        IEnumerable<TEntity> ToDataModelList(IEnumerable<UEntity> inputModel);
    }
}
