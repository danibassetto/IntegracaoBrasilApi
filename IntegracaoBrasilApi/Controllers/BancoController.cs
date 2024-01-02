﻿using IntegracaoBrasilApi.Controllers.Base;
using IntegracaoBrasilApi.Model;
using IntegracaoBrasilApi.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IntegracaoBrasilApi.Controllers;

public class BancoController(IBancoService service) : BaseController<IBancoService>(service)
{
    [HttpGet]
    public async Task<ActionResult<List<BancoModel>>> GetAll()
    {
        var response = await _service.GetAll();

        if (response == null)
            return BadRequest("Nenhum banco encontrado!");
        else
            return Ok(response);
    }
}