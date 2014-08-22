using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QKitSampleApp.Models
{
    public class ContactModel
    {
        public static List<ContactModel> CreateSampleData()
        {
            var data = new List<ContactModel>();

            data.Add(new ContactModel("John"));
            data.Add(new ContactModel("Beth"));
            data.Add(new ContactModel("Courtney"));
            data.Add(new ContactModel("Ahab"));
            data.Add(new ContactModel("Margie"));
            data.Add(new ContactModel("Enrique"));
            data.Add(new ContactModel("Quetin"));
            data.Add(new ContactModel("Alex"));
            data.Add(new ContactModel("Eric"));
            data.Add(new ContactModel("Mark"));
            data.Add(new ContactModel("Alfonzo"));
            data.Add(new ContactModel("Milo"));
            data.Add(new ContactModel("Pia"));
            data.Add(new ContactModel("Edgar"));
            data.Add(new ContactModel("Phillip"));
            data.Add(new ContactModel("Lillian"));
            data.Add(new ContactModel("Chucky"));
            data.Add(new ContactModel("Rolf"));
            data.Add(new ContactModel("Gregory"));
            data.Add(new ContactModel("Jimmy-bean"));
            data.Add(new ContactModel("Tommy"));
            data.Add(new ContactModel("Pierre"));
            data.Add(new ContactModel("Tigris"));
            data.Add(new ContactModel("Zorro"));
            data.Add(new ContactModel("リザードン"));

            return data;
        }

        public ContactModel(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
