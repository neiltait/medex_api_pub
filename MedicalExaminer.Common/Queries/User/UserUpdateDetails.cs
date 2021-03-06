﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MedicalExaminer.Common.Queries.User
{
    public class UserUpdateDetails : IUserUpdate
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
