using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Core.Dtos
{
    public class GameDto
    {
        public string Title { get; set; } = null!;
        public DateTime StartDate { get; set; }
    }
}
