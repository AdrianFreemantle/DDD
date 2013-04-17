using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Core.Infrastructure
{
    public interface ILookupTable
    {
        int Id { get; set; }
        string Description { get; set; }
    }
}
