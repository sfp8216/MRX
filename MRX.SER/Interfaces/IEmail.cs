using System.Threading.Tasks;
using MRX.SER.DTO;

namespace MRX.SER.Interfaces
{
    public interface IEmail
    {
        Task Send(string emailAddress, string body, EmailOptionsDTO emailOptionsDTO);
    }
}