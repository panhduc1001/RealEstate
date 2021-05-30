using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CAB201_Assignment
{
    // Password class handles all hashing and 
    class Password
    {
        private const int ITERATIONS = 1000;
        private const int LENGTH = 32;

        private byte[] salt;
        private byte[] pwd_hash;

        public Password(string userPassword)
        {
            Rfc2898DeriveBytes PBKDF2 = new Rfc2898DeriveBytes(userPassword, 8, ITERATIONS);    //Hash the password with a 8 byte salt
            byte[] hashedPassword = PBKDF2.GetBytes(LENGTH);
            byte[] _salt = PBKDF2.Salt;
            pwd_hash = hashedPassword;                                                          //Update the given customer's data
            salt = _salt;
        }

        // public method allowing customer to check a given password against the stored hash
        public bool checkPassword(string userInput)
        {
            Rfc2898DeriveBytes PBKDF2 = new Rfc2898DeriveBytes(userInput, salt, ITERATIONS);    //Hash the password with the users salt
            byte[] hashedInput = PBKDF2.GetBytes(LENGTH);                                       //Returns a hash - same length as hash returned in constructor           
            bool matching = hashEquals(hashedInput);                                            //Compares the password given with the given salt and hash
            return matching;
        }

        // simple method to check whether a given byte array matches the stored hash for the object
        private bool hashEquals(byte[] hash)
        {
            int length = pwd_hash.Length;
            if (length != hash.Length)
            {
                // if the array lengths aren't equal, the arrays aren't equal
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                // iterate through each corresponding elements of the arrays and test for equality
                if (pwd_hash[i] != hash[i]) return false;
            }
            return true;
        }
    }
}
