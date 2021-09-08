using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductmanagementCore.Models.ModelInput
{
    public class CriteriaRequest
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public string SortColumn { get; set; }
        public bool? SortDesc { get; set; }
        public string Filter { get; set; }
    }
}
