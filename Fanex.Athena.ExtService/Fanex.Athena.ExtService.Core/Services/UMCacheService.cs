using System;
using System.Collections.Generic;
using System.Text;
using ONEbook.UM.Helpers;

namespace Fanex.Athena.ExtService.Core.Services
{
    public class UMCacheService : IUMCacheService
    {
        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, T value, TimeSpan absoluteExpiration)
        {
            throw new NotImplementedException();
        }
    }
}
