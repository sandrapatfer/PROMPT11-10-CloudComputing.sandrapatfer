using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace ConsoleApp.Entities
{
    class Contact : TableServiceEntity
    {
        public Contact()
        { }
        public Contact(string country, string lastName)
            : base(country, lastName)
        { }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
    }
}
