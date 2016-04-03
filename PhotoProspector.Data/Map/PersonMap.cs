using FluentNHibernate.Mapping;
using PhotoProspector.Domain.Entity;

namespace PhotoProspector.Data.Map
{
    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Table("person");
            Id(x => x.Id).Column("person_id");
            Map(x => x.FirstName).Column("person_first_name");
            Map(x => x.LastName).Column("person_last_name");
            HasMany(x => x.Photos).LazyLoad().Cascade.All();
        }
    }
}
