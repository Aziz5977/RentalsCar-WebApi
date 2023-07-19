﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCar.DataAccess.Utils;
using RentCar.Service.Dtos.Cars;
using RentCar.Service.Interfaces.Cars;
using RentCar.Service.Validators.Dtos.Cars;

namespace RentCar.WebApi.Controllers;

[Route("api/car")]
[ApiController]
public class CarController : ControllerBase
{
    private ICarService _service;
    private readonly int maxPageSize = 30;

    public CarController(ICarService carService)
    {
        this._service = carService;
    }

    [HttpPost]
    public async Task<IActionResult> CraetAsync([FromForm] CarsCreateDto dto)
    {
        var createValidator = new CarCreateValidators();
        var validatorResult = createValidator.Validate(dto);
        if (validatorResult.IsValid) return Ok(_service.CreateAsync(dto));
        else return BadRequest(validatorResult.Errors);
    }

    [HttpGet("{carId}")]
    public async Task<IActionResult> GetByIdAsync(long carId)
        => Ok(await _service.GetByIdAsync(carId));

    [HttpDelete("{carId}")]
    public async Task<IActionResult> DeleteAsync(long carId)
        => Ok(await _service.DeleteAsync(carId));

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1)
        => Ok(await _service.GetAllAsync(new Paginationparams(page, maxPageSize)));

    [HttpPut("{carId}")]
    public async Task<IActionResult> UpdateAsync(long carId, [FromForm] CarsUpdatedto dto)
    {
        CarupdateValidators validationRules = new CarupdateValidators();
        var validatorResult = validationRules.Validate(dto);
        if (validatorResult.IsValid) return Ok(await _service.UpdateAsync(carId, dto));
        else return BadRequest(validatorResult.Errors);
    }

    [HttpGet("count")]
    public async Task<IActionResult> CountAsync()
        => Ok(await _service.CountAsync());
}
