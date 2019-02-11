using System;
using Newtonsoft.Json;

namespace Medical_Examiner_API.Models
{
    public class User : Record
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
