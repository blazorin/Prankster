using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Services
{
    public class PrankServices : IPrankServices
    {
        private readonly MauiPContext _ctx;
        private readonly IMapper _mapper;

        public PrankServices(MauiPContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PrankDto>> GetLatestPranks()
        {
            var result = await _ctx.Pranks
                .Where(p => p.Enabled)
                .OrderByDescending(r => r.DateAdded)
                .ProjectTo<PrankDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<PrankDto>> GetTrendingPranks()
        {
            var result = await _ctx.Pranks
                .Where(p => p.Enabled)
                .OrderByDescending(r => r.Popularity)
                .ProjectTo<PrankDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return result;
        }

        public async Task<PrankDto> GetPrankById(int prankId)
        {
            var result = await _ctx.Pranks
                .Where(p => p.PrankId == prankId.ToString() && p.Enabled)
                .ProjectTo<PrankDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return result;
        }
    }
}
