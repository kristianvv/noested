﻿using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace Noested.Models.ViewModels
{

    public class UserListViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
    }
    public class UserViewModel
    {



        [DisplayName("BrukerID")]
        public string? UserId { get; set; }

        [DisplayName("Epost")]
        public string? Email { get; set; }


        [DisplayName("Brukernavn")]
        public string? UserName { get; set; }


        [DisplayName("Bekreftet Epost")]
        public bool? EmailConfirmed { get; set; }

        public List<string>? UserRoles { get; set; }

        [DisplayName("Telefonnummer")]
        public string? PhoneNumber { get; set; }

    }
}