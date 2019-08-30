using System;
using System.Collections.Generic;
using System.Text;
using Fanex.Athena.ExtService.Shared.DTO;
using Fanex.Athena.ExtService.Shared.Model.DTO;

namespace Fanex.Athena.ExtService.Shared.Services
{
    public interface ICommonService
    {
        LicenseeInfo GetLicenseeInfo(string siteName, int custId = 0);
        BalanceUp GetBalanceUp();

    }
}
