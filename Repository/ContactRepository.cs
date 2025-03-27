using ContactDetails_Api.Data;
using ContactDetails_Api.Entity;
using ContactDetails_Api.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ContactDetails_Api.Repository
{
    public class ContactRepository : Icontact_Repository
    {
        private readonly AppDbContex contex;
        public ContactRepository(AppDbContex _contex)
        {
            this.contex = _contex;
        }
        public async Task Add_Details(Contact contact)
        {
            await contex.AddAsync(contact);
            await contex.SaveChangesAsync();
        }

        public async Task Delete_Details(int id)
        {
            var contact = await contex.Contacts.FindAsync(id);
            if (contact != null)
            { 
                contex.Contacts.Remove(contact);
                contex.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<Contact>> GetAll_Details()
        {
           var result = await contex.Contacts.ToListAsync();
            return result;
        }

        public async Task<Contact> GetById_Details(int id)
        {
          var result =  await contex.Contacts.FindAsync(id);
            return result;
        }

        public async Task Update_Details(Contact contact)
        {
            
            contex.Contacts.Update(contact);
             await contex.SaveChangesAsync();
            
        }

        public async Task<Contact?> Login(string firstname, string email)
        {
           var result =  await contex.Contacts.FirstOrDefaultAsync(c => c.FirstName == firstname && c.Email == email);
            return result;
        }
    }
}
