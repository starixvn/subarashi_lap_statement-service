using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.Core.Models.DTO
{
    public class ApplicationOptions
    {
        public IList<string> WhiteList { get; set; }
        public string AesKey { get; set; }
        public string AesIV { get; set; }
    }
}
