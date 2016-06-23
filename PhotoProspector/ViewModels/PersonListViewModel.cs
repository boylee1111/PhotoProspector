using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhotoProspector.Models;

namespace PhotoProspector.ViewModels
{
    public class PersonListViewModel
    {
        [Required]
        public List<Person> Persons { get; set; }

        [Required]
        public List<Person> Customers { get; set; }

        [Required]
        public string ImageURL { get; set; }

        public PersonListViewModel()
        {
            Persons = new List<Person>();
            Customers = new List<Person>();
        }
    }

}
