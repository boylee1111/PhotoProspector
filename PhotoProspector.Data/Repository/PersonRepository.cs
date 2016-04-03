using PhotoProspector.Domain.Entity;
using PhotoProspector.Domain.Helper;
using PhotoProspector.Domain.Repository;

namespace PhotoProspector.Data.Repository
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
