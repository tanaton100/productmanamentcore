using System.Collections.Generic;

namespace ProductmanagementCore.Models.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public IEnumerable<Products> Products { get; set; }

    }
}
