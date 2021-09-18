using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Model.Services.Http;
using Shared.Dto;
using Shared.Enums;

namespace Model.Services
{
    public class CallServices : ICallServices
    {
        private readonly Bitsbi _bitsbi;
        private readonly MauiPContext _ctx;
        private readonly IMapper _mapper;

        public CallServices(Bitsbi bitsbi, MauiPContext ctx, IMapper mapper)
        {
            _bitsbi = bitsbi;
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<CallDto>> GetCallsFromUser(string id)
        {
            var user = await GetUserWithCalls(id);
            var calls = _mapper.Map<List<CallDto>>(user.Calls);

            return calls;
        }

        private async Task<User> GetUserWithCalls(string id) =>
            await _ctx.Users
                .Where(u => u.UserId == id)
                .Include(u => u.Calls)
                .FirstOrDefaultAsync();
    }
}