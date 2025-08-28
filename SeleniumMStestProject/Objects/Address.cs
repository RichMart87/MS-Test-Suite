using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumMStestProject.Objects
{
    public interface IAddress
    {
        string Street { get; set; }
        string City { get; set; }
        string State { get; set; }
        string ZipCode { get; set; }
        string Country { get; set; }
        string FullAddress { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string PhoneNumber { get; set; }
        string Email { get; set; }

        string AddressLine1 { get; set; }
        string AddressLine2 { get; set; }
        string AddressLine3 { get; set; }

        //Set up in Enums
        //CountryType Country { get; set; }
        string GetFullAddress(string street, string city, string state, string zipCode, string country);

        string GetFullAddress(string street, string city, string state, string zipCode);

        string GetFullAddress(string street, string city, string state);

        void FillAddress(string street, string city, string state, string zipCode, string country);

        void ClearAddress();
    }
}