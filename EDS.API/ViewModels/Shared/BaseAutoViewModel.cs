using System;
using System.Collections.Generic;
using System.Linq;
using Core.DataMapper;
using Core.Service;

namespace EDS.API.ViewModels.Shared
{
    public abstract class BaseAutoViewModel<TEntity, UEntity> : IViewModel<TEntity, UEntity>
        where TEntity : class,new()
        where UEntity : class,new()
    {
        public BaseAutoViewModel()
        {
            dynamic child = this;

            try
            {
                child.CreatedBy = "";
            }
            catch { }
        }

        public UEntity FromServiceModel(TEntity inputModel)
        {
            if (inputModel == null) { return null; }
            DataMapper.Resolve<TEntity, UEntity>();
            var output = DataMapper.Map<TEntity, UEntity>(inputModel);
            return output;
        }

        public TEntity ToServiceModel(UEntity inputModel)
        {
            if (inputModel == null) { return null; }
            DataMapper.Resolve<TEntity, UEntity>();
            var output = DataMapper.Map<UEntity, TEntity>(inputModel);
            return output;
        }

        public IEnumerable<UEntity> FromServiceModelList(IEnumerable<TEntity> inputModel)
        {
            if (inputModel == null) { return null; }
            DataMapper.Resolve<TEntity, UEntity>();
            var output = DataMapper.Map<TEntity, UEntity>(inputModel);
            return output;
        }

        public IEnumerable<TEntity> ToServiceModelList(IEnumerable<UEntity> inputModel)
        {
            if (inputModel == null) { return null; }
            DataMapper.Resolve<TEntity, UEntity>();
            var output = DataMapper.Map<UEntity, TEntity>(inputModel);
            return output;
        }
    }
}
