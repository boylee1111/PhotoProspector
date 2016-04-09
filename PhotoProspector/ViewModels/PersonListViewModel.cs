using PhotoProspector.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoProspector.ViewModels
{
    public class PersonListViewModel
    {
        [Required]
        public List<Person> Persons { get; set; }

        [Required]
        public string ImageURL { get; set; }

        public PersonListViewModel()
        {
            Persons = new List<Person>();
        }
    }

}
