using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bank_session.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get ; set; }
        public string LastName { get ; set; }
        public string Email { get ; set; }

        [DataType(DataType.Password)]
        public string Password { get ; set; }

        public double Balance { get; set; }

        public List<Transaction> Transactions { get; set; }

        public User()
        {
            Transactions = new List<Transaction>();
        }
    }
}