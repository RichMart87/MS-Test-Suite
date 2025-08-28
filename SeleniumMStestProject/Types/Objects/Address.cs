using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumMStestProject.Types.Objects
{
    public class Address
    {
        private string email;
        private string firstName;
        private string lastName;
        private string fullname;
        private string homephone;
        private string mobilephone;
        private string workphone;
        private string address1;
        private string address2;
        private string city;
        private string state;
        private string zip;
        private string country;
        //Setup PhoneType in Enums
        //private Dictionary<PhoneType,string> phoneNumbers;

        public Address(string email, string firstName, string lastName, string fullname, string homephone, string mobilephone, string workphone, string address1, string address2, string city, string state, string zip, string country)
        {
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
            this.fullname = fullname;
            this.homephone = homephone;
            this.mobilephone = mobilephone;
            this.workphone = workphone;
            this.address1 = address1;
            this.address2 = address2;
            this.city = city;
            this.state = state;
            this.zip = zip;
            this.country = country;
        }

        public Address(TestCategoryAttribute testCategoryAttribute)
        {
            //Setup Country in TestCategoryAttribute when initializing testmethod
            //this.country = testCategoryAttribute.Country;
            InitializeAddress(this.country, true);
        }

        private void InitializeAddress(string country, bool v)
        {
            throw new NotImplementedException();
        }
    }
}