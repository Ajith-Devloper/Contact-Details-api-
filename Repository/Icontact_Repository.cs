using ContactDetails_Api.Entity;

namespace ContactDetails_Api.Model
{
    public interface Icontact_Repository
    {
        Task<IEnumerable<Contact>> GetAll_Details();
        Task<Contact> GetById_Details(int id);
        Task Add_Details(Contact contact);
        Task Update_Details(Contact contact);
        Task Delete_Details(int id);
        Task<Contact?> Login(string firstname, string email);
    }
}
