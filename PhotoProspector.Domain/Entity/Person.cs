using System.Collections.Generic;

namespace PhotoProspector.Domain.Entity
{
    public class Person : Entity
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual IList<Photo> Photos { get; set; }

        public Person()
        {
            Photos = new List<Photo>();
        }

        public virtual Person AddPhoto(Photo photo)
        {
            photo.Person = this;
            Photos.Add(photo);
            return this;
        }
    }
}
