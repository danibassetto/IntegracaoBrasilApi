﻿using IntegracaoBrasilApi.Model;
using IntegracaoBrasilApi.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IntegracaoBrasilApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CepController(ICepService cepService) : ControllerBase
{
    private readonly ICepService _cepService = cepService;

    [HttpGet("{cep}")]
    public async Task<ActionResult<CepModel>> Get(string cep)
    {
        var response = await _cepService.Get(cep);

        if (response == null)
            return BadRequest("Cep não encontrado!");
        else
            return Ok(response);
    }
}