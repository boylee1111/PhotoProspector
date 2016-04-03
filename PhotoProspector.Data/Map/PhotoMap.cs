using FluentNHibernate.Mapping;
using PhotoProspector.Domain.Entity;

namespace PhotoProspector.Data.Map
{
    public class PhotoMap : ClassMap<Photo>
    {
        public PhotoMap()
        {
            Table("photo");
            Id(x => x.Id).Column("photo_id");
            Map(x => x.Date).Column("photo_date");
            References(x => x.Person).Column("photo_person_id");
        }
    }
}
