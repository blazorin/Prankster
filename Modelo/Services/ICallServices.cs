using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Dto;

namespace Model.Services
{
    public interface ICallServices
    {
        Task<List<CallDto>> GetCallsFromUser(string id);
    }
}