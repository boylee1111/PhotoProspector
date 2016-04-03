using System;

namespace PhotoProspector.Domain.Entity
{
    public class Photo : Entity
    {
        public virtual DateTime Date { get; set; }
        public virtual Person Person { get; set; }
    }
}
