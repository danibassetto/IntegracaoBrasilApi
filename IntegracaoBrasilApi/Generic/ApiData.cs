﻿namespace Harmonit.Microservice.Base.Library.Generic;

public static class ApiData
{
    public static List<ApiDataRequest>? ListApiDataRequest { get; private set; }

    public static Guid CreateApiDataRequest()
    {
        var newApiDataRequest = new ApiDataRequest();
        ListApiDataRequest ??= [];
        ListApiDataRequest.Add(newApiDataRequest);
        return newApiDataRequest.GuidApiDataRequest;
    }

    public static ApiDataRequest GetApiDataRequest(Guid guidApiDataRequest)
    {
        ApiDataRequest? apiDataRequest = (from i in ListApiDataRequest where i.GuidApiDataRequest == guidApiDataRequest select i).FirstOrDefault();
        return apiDataRequest ?? throw new Exception("Invalid ApiDataRequest");
    }

    public static void RemoveApiDataRequest(Guid guidApiDataRequest)
    {
        ApiDataRequest? apiDataRequest = (from i in ListApiDataRequest ?? [] where i.GuidApiDataRequest == guidApiDataRequest select i).FirstOrDefault();
        if (apiDataRequest != null)
            ListApiDataRequest?.Remove(apiDataRequest);
    }
}

public class ApiDataRequest
{
    public ApiDataRequest()
    {
        GuidApiDataRequest = Guid.NewGuid();
    }

    public Guid GuidApiDataRequest { get; private set; }
}