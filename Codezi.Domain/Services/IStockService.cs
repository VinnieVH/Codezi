using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codezi.Domain.Models;

namespace Codezi.Domain.Services
{
    public interface IStockService
    {
        Task<Company> GetCompanyBySymbolAsync(string symbol);
        Task<Company> GetIntradayByMinuteAsync(string symbol);
    }
}
