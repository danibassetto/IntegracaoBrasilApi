﻿using Harmonit.Microservice.Base.Library.BaseService;
using Harmonit.Microservice.Base.Library.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace Harmonit.Microservice.Base.Library.BaseController;

[Authorize]
[ApiController]
public class BaseController<TIService, TValidate, TTypeCurrentOperation>(TIService service) : Controller
    where TIService : IBaseService
{
    public Guid _guidApiDataRequest;
    public TIService? _service = service;

    //[NonAction]
    //public async Task<ActionResult> ResponseAsync<TTypeResult, TTypeException>(BaseResponseApiContent<TTypeResult, TTypeException> result)
    //{

    //}

    [NonAction]
    public void SetData()
    {
        Guid guidApiDataRequest = ApiData.CreateApiDataRequest();
        SetGuid(guidApiDataRequest);
    }

    [NonAction]
    public void SetGuid(Guid guidApiDataRequest)
    {
        _guidApiDataRequest = guidApiDataRequest;
        SetGuidApiDataRequest(this, guidApiDataRequest);
    }

    [NonAction]
    public override async void OnActionExecuting(ActionExecutingContext context)
    {
        try
        {
            SetData();
        }
        catch (Exception ex)
        {
            
        }
    }

    public static void SetGuidApiDataRequest<TClass>(TClass @class, Guid guidApiDataRequest)
    {
        List<FieldInfo> listRuntimeFields = (from i in @class?.GetType().GetRuntimeFields() where i.FieldType.IsInterface select i).ToList();

        _ = (from i in listRuntimeFields
             let setGuidMethod = i.FieldType.GetMethod("SetGuid")
             let listInterface = setGuidMethod == null ? [.. i.FieldType.GetInterfaces()] : new List<Type>()
             where (from j in listInterface where j != null select j).Any() | setGuidMethod != null
             let interfaceToInvoke = setGuidMethod == null ? (from j in listInterface
                                                              where j != null
                                                              let setGuidMethod = j.GetMethod("SetGuid")
                                                              where setGuidMethod != null
                                                              select new { Interface = i, SetGuidMethod = setGuidMethod }).FirstOrDefault() : new { Interface = i, SetGuidMethod = setGuidMethod }
             where interfaceToInvoke != null
             select InvokeInterfaceSetGuid(@class, guidApiDataRequest, (FieldInfo: interfaceToInvoke.Interface, MethodInfo: interfaceToInvoke.SetGuidMethod))).ToList();
    }

    public static bool InvokeInterfaceSetGuid<TClass>(TClass @class, Guid guidBeesApiDataRequest, (FieldInfo FieldInfo, MethodInfo MethodInfo) interfaceToInvoke)
    {
        try
        { interfaceToInvoke.MethodInfo.Invoke(interfaceToInvoke.FieldInfo.GetValue(@class), new object[] { guidBeesApiDataRequest }); }
        catch { }
        return true;
    }
}