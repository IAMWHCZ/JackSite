using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JackSite.Identity.Server.Enums;

namespace JackSite.Identity.Server.Entities.Clients
{
    public class ClientBasic
    {
        public string ClientId { get; set; } = default!;

        public bool IsEnable { get; set; }

        public string? Description { get; set; }

        public ProtoType Proto { get; set; }

        public bool IsNeedClientSecret { get; set; }

        public bool IsNeedRequestObject { get; set; } 

        public bool IsNeedPkce { get; set; }

        public bool IsNeedTextPkce { get; set; }

        public bool IsOffline { get; set; }

        public bool IsBrowserTokenAccess { get; set; }
    }
}