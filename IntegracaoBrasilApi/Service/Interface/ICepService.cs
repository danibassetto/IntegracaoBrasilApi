﻿using IntegracaoBrasilApi.Model;

namespace IntegracaoBrasilApi.Service.Interface;

public interface ICepService
{
    Task<CepModel?> Get(string cep);
}