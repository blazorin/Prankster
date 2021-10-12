using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class HomePranksDto
    {
        public IEnumerable<PrankDto> Trending { get; set; }
        public IEnumerable<PrankDto> Latest { get; set; }
    }
}
