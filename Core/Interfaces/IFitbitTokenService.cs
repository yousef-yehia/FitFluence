using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IFitbitTokenService
    {
        public Task StoreTokenAsync(string token);
        public Task<string> RetrieveTokenAsync();

    }
}
