﻿using IntegracaoBrasilApi.Arguments;
using IntegracaoBrasilApi.Domain.ApiManagement;
using IntegracaoBrasilApi.Domain.Interfaces.Service;
using IntegracaoBrasilApi.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IntegracaoBrasilApi.Api.Controllers.Address;

[Route("api/brasilapi/[controller]")]
public class AddressController(IApiDataService apiDataService, IAddressService service) : BaseController_1<IAddressService>(apiDataService, service)
{
    /// <summary>
    /// Consulta um endereço a partir de um Código Postal (CEP)
    /// </summary>
    [ProducesResponseType<OutputGetByCepAddress>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("GetByCep/{cep}")]
    public async Task<ActionResult<BaseResponseApi<OutputGetByCepAddress, ApiResponseException>>> GetByCep([Length(8,8)] string cep)
    {
        try
        {
            return await ResponseAsync(await _service!.GetByCep(cep));
        }
        catch (BaseResponseException ex)
        {
            return await BaseResponseExceptionAsync(ex);
        }
        catch (Exception ex)
        {
            return await ResponseExceptionAsync(ex);
        }
    }

    #region IgnoreApi
    [ApiExplorerSettings(IgnoreApi = true)]
    public override Task<ActionResult<BaseResponse<object>>> GetAll()
    {
        return base.GetAll();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public override Task<ActionResult<BaseResponse<object>>> Get(string id)
    {
        return base.Get(id);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public override Task<ActionResult<BaseResponse<object>>> GetListByListId(List<string> listId)
    {
        return base.GetListByListId(listId);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public override Task<ActionResult<BaseResponse<object>>> GetByIdentifier(object inputIdentifier)
    {
        return base.GetByIdentifier(inputIdentifier);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public override Task<ActionResult<BaseResponse<List<object>>>> GetListByListIdentifier([FromBody] List<object> listInputIdentifier)
    {
        return base.GetListByListIdentifier(listInputIdentifier);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public override Task<ActionResult<BaseResponse<string>>> Create(object inputCreate)
    {
        return base.Create(inputCreate);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public override Task<ActionResult<BaseResponse<List<string>>>> Create(List<object> listInputCreate)
    {
        return base.Create(listInputCreate);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public override Task<ActionResult<BaseResponse<string>>> Update(BaseInputIdentityUpdate_0 inputIdentityUpdate)
    {
        return base.Update(inputIdentityUpdate);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public override Task<ActionResult<BaseResponse<List<string>>>> Update(List<BaseInputIdentityUpdate_0> listInputIdentityUpdate)
    {
        return base.Update(listInputIdentityUpdate);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public override Task<ActionResult<BaseResponse<List<bool>>>> Delete(List<string> listId)
    {
        return base.Delete(listId);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public override Task<ActionResult<BaseResponse<bool>>> Delete(string id)
    {
        return base.Delete(id);
    }
    #endregion
}