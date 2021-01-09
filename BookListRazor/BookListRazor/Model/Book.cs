using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookListRazor.Model
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Author { get; set; }
        //minute 1:05:24 

        public string ISBN { get; set; }

    }
}
