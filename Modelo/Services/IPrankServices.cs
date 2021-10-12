using Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Services
{
    public interface IPrankServices
    {
        Task<IEnumerable<PrankDto>> GetTrendingPranks();
        Task<IEnumerable<PrankDto>> GetLatestPranks();
    }
}
