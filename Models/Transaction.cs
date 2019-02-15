using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bank_session.Models
{
    public class Transaction
    {
        [Key]
        public int TransId { get; set; }

        public double Amount { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("user")]
        public int User_Id { get; set; }

        public User user { get; set; }
    }
}