using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Profiles
{
    public class Profile
    {
        public required string UserName { get; set; }
        public required string DisplayName { get; set; }
        public required string Bio { get; set; }
        public required string Image { get; set; }

        public required ICollection<Photo> Photos { get; set; }

        public bool Following { get; set; }
        public int FollowingCount { get; set; }
        public int FollowersCount { get; set; }
    }
}