﻿using IntegracaoBrasilApi.Model;

namespace IntegracaoBrasilApi.Service.Interface;

public interface ICnpjService : IBaseService
{
    Task<CnpjModel?> Get(string cnpj);
}