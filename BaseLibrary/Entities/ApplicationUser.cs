﻿
namespace BaseLibrary.Entities
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
