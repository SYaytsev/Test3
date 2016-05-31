using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp3
{
    /// <summary>
    /// User Gender
    /// </summary>
    enum Gender { unknown, male, female }

    /// <summary>
    /// User: LastName, FirstName, Birthdate, TimeAdded, City, Address, PhoneNumber, Gender, Email.
    /// </summary>
    class User
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime TimeAdded { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }

        public User()
        {
        }

        public User(string lastName, string firstName, DateTime birthDate,
            string city)
            : this(lastName, firstName, birthDate, city, "unknown", "unknown", Gender.unknown, "unknown")
        {

        }

        public User(string lastName, string firstName, DateTime birthDate,
            string city, string address, string phoneNumber, Gender gender, string email)
        {
            LastName = lastName;
            FirstName = firstName;
            Birthdate = birthDate;
            TimeAdded = DateTime.Now;
            City = city;
            Address = address;
            PhoneNumber = phoneNumber;
            Gender = gender;
            Email = email;
        }

        public override string ToString()
        {
            return String.Format("{0} {1}, from {2} ", LastName, FirstName, City);
        }
    }
}
