using System.Threading.Tasks;
using CrmApi.DataTransferObjects;

namespace CrmApi.Contracts
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(UserDto userAuthDto);
        Task<string> CreateToken();
    }
}