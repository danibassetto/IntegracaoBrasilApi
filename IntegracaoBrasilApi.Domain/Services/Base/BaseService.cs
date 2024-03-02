﻿using IntegracaoBrasilApi.Arguments;
using IntegracaoBrasilApi.Domain.ApiManagement;
using IntegracaoBrasilApi.Domain.Entities;
using IntegracaoBrasilApi.Domain.Interfaces.Repository;
using IntegracaoBrasilApi.Domain.Interfaces.Service;

namespace IntegracaoBrasilApi.Domain.Services;

public class BaseService<TIRefit, TBaseRepository, TInputCreate, TInputUpdate, TInputIdentityUpdate, TEntity, TOutput, TInputIdentifier>(TIRefit? refit, TBaseRepository? repository) : IBaseService<TInputCreate, TInputUpdate, TInputIdentityUpdate, TOutput, TInputIdentifier>
    where TIRefit : class
    where TBaseRepository : IBaseRepository<TEntity, TInputIdentifier>
    where TInputIdentityUpdate : BaseInputIdentityUpdate<TInputUpdate>
    where TEntity : BaseEntity<TEntity>
{
    public Guid _guidApiDataRequest;
    public TIRefit? _refit = refit;
    protected TBaseRepository? _repository = repository;

    public void SetGuid(Guid guidApiDataRequest)
    {
        _guidApiDataRequest = guidApiDataRequest;
        GenericModule.SetGuidApiDataRequest(this, guidApiDataRequest);
    }

    //public BaseResponseApiContent<TTypeResult, TTypeException> ReturnResponse<TTypeResult, TTypeException>(ApiResponse<string> response, bool isHarmonitIntegration = false)
    //{

    //}

    #region Read
    public List<TOutput> GetAll()
    {
        return FromEntityToOutput(_repository.GetAll());
    }

    public TOutput Get(string id)
    {
        return GetListByListId([id]).FirstOrDefault();
    }

    public List<TOutput> GetListByListId(List<string> listId)
    {
        return FromEntityToOutput(_repository.GetListByListId(listId));
    }

    public TOutput GetByIdentifier(TInputIdentifier inputIdentifier)
    {
        return FromEntityToOutput(_repository.GetByIdentifier(inputIdentifier));
    }

    public List<TOutput> GetListByListIdentifier(List<TInputIdentifier> listInputIdentifier)
    {
        return FromEntityToOutput(_repository.GetListByListIdentifier(listInputIdentifier));
    }
    #endregion

    #region Create
    public string? Create(TInputCreate inputCreate)
    {
        return Create([inputCreate])?.FirstOrDefault();
    }

    public virtual List<string>? Create(List<TInputCreate> listInputCreate)
    {
        return [.. _repository.Create(FromInputCreateToEntity(listInputCreate))];
    }
    #endregion

    #region Update
    public string? Update(TInputIdentityUpdate inputIdentityUpdate)
    {
        return Update([inputIdentityUpdate])?.FirstOrDefault() ?? "";
    }

    public virtual List<string>? Update(List<TInputIdentityUpdate> listInputIdentityUpdate)
    {
        var listEntity = (from i in listInputIdentityUpdate
                          let oldEntity = FromOutputToEntity(Get(i.Id ?? string.Empty))
                          select UpdateEntity(oldEntity, i.InputUpdate)).ToList();

        return [.. _repository.Update(listEntity)];
    }

    protected TEntity UpdateEntity(TEntity oldEntity, TInputUpdate inputUpdate)
    {
        foreach (var property in typeof(TInputUpdate).GetProperties())
        {
            var correspondingProperty = typeof(TEntity).GetProperty(property.Name);
            if (correspondingProperty != null)
            {
                var value = property.GetValue(inputUpdate, null);

                correspondingProperty?.SetValue(oldEntity, value, null);
            }
        }
        return oldEntity;
    }
    #endregion

    #region Delete
    public bool? Delete(string id)
    {
        return Delete([id])?.FirstOrDefault();
    }

    public virtual List<bool>? Delete(List<string> listId)
    {
        return _repository.Delete(listId);
    }
    #endregion

    #region Mapper
    public TOutput FromEntityToOutput(TEntity entity)
    {
        return ApiData.Mapper.MapperEntityOutput.Map<TEntity, TOutput>(entity);
    }

    public List<TOutput> FromEntityToOutput(List<TEntity> listEntity)
    {
        return ApiData.Mapper.MapperEntityOutput.Map<List<TEntity>, List<TOutput>>(listEntity);
    }

    public TEntity FromOutputToEntity(TOutput output)
    {
        return ApiData.Mapper.MapperEntityOutput.Map<TOutput, TEntity>(output);
    }

    public List<TEntity> FromInputCreateToEntity(List<TInputCreate> listInputCreate)
    {
        return ApiData.Mapper.MapperInputEntity.Map<List<TInputCreate>, List<TEntity>>(listInputCreate);
    }

    public TEntity FromInputUpdateToEntity(TInputUpdate inputUpdate)
    {
        return ApiData.Mapper.MapperInputEntity.Map<TInputUpdate, TEntity>(inputUpdate);
    }
    #endregion

}

#region All Parameters
public class BaseService_0() : BaseService<object, IBaseRepository_0, object, object, BaseInputIdentityUpdate_0, BaseEntity_0, object, object>(default, default) { }
#endregion

public class BaseService_1<TBaseRepository, TInputCreate, TInputUpdate, TInputIdentityUpdate, TEntity, TOutput, TInputIdentifier>(TBaseRepository? repository) : BaseService<object, TBaseRepository, TInputCreate, TInputUpdate, TInputIdentityUpdate, TEntity, TOutput, TInputIdentifier>(default, repository)
    where TBaseRepository : IBaseRepository<TEntity, TInputIdentifier>
    where TInputIdentityUpdate : BaseInputIdentityUpdate<TInputUpdate>
    where TEntity : BaseEntity<TEntity>
{ }