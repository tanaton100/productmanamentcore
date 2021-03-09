using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductmanagementCore.Models.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public string ProductName { get; set; }
    }
}
