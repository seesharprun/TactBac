using System.Collections.Generic;
using System.Threading.Tasks;
using TactBac.Mobile.Models;

namespace TactBac.Mobile.Services
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetContactNamesAsync();
    }
}