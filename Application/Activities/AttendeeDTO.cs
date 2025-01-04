using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class AttendeeDTO
    {
        public required string UserName { get; set; }
        public required string DisplayName { get; set; }
        public required string Bio { get; set; }
        public required string Image { get; set; }
        public bool Following { get; set; }
        public int FollowingCount { get; set; }
        public int FollowersCount { get; set; }
    }
}