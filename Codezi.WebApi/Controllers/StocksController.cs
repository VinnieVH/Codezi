using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codezi.Domain.Models;
using Codezi.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Codezi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly ILogger _logger;

        public StocksController(IStockService stockService, ILogger<StocksController> logger)
        {
            _stockService = stockService;
            _logger = logger;
        }

        // GET: api/stocks/company/{symbol}
        [HttpGet("company/{symbol}")]
        public async Task<ActionResult<Company>> GetCompanyBySymbol(string symbol)
        {
            _logger.LogInformation($"{DateTime.Now} received get request for company");
            var company = await _stockService.GetCompanyBySymbolAsync(symbol);
            return Ok(company);
        }
    }
}