using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.Common.DTO;
using Fanex.Athena.ExtService.Common.Model.DTO;

namespace Fanex.Athena.ExtService.Common.Services
{
    public interface ICommonService
    {
        Task<LicenseeInfo> GetLicenseeInfoAsync(string siteName, int custId = 0);
        Task<BalanceUp> GetBalanceUpAsync();

    }
}
