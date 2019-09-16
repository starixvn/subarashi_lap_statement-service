using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.Core.Models.DTO
{
    public class ApplicationOptions
    {
        public IList<string> WhiteList { get; set; }
        public RedisConfiguration Redis { get; set; }
        public string LogCategory { get; set; }
        public string RabbitMQ { get; set; }

    }
}
