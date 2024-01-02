﻿using AutoMapper;
using IntegracaoBrasilApi.Model;
using IntegracaoBrasilApi.Refit;
using IntegracaoBrasilApi.Service.Interface;

namespace IntegracaoBrasilApi.Service;

public class CepService(IMapper mapper, ICepRefit refit) : BaseService<ICepRefit>(mapper, refit), ICepService
{
    public async Task<CepModel?> Get(string cep)
    {
        var response = await _refit.Get(cep);
        if (response != null && response.IsSuccessStatusCode)
            return _mapper.Map<CepModel>(response.Content);
        else
            return null;
    }
}