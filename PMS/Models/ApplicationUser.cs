﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;

namespace PMS.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            CreatedOn = DateTime.Now;
            UpdatedOn = DateTime.Now;
            AnnouncementUsers = new Collection<AnnouncementUser>();
        }

        public string FullName { get; set; }

        public string Avatar { get; set; }

        public string Major { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<AnnouncementUser> AnnouncementUsers { get; set; }
    }
}
