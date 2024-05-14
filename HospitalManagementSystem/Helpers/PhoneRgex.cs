using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Helpers
{
    public class PhoneRgex
    {
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
          
            return Regex.IsMatch(phoneNumber, @"^\+998\d{9}$");
        }
    }
}
