using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_GPNTB.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Genre { get; set; }

        public string Description { get; set; }

        public DateTime? DateIssued { get; set; }

        public DateTime? DateReturn { get; set; }

        public BookStatus Status { get; set; }

        public User Reader { get; set; }
    }

    public enum BookStatus
    {
        Available,
        Issued,
        Maintenance
    }
}
