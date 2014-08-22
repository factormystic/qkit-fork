using QKitSampleApp.Common;
using QKitSampleApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QKitSampleApp.ListPlaceholderSample
{
    public class SearchViewModel : BaseModel
    {
        private string query = "";
        private List<ContactModel> contacts;
        private IList results;

        public SearchViewModel()
        {
            UpdateSearchResults();
        }

        public List<ContactModel> Contacts
        {
            get
            {
                if (contacts == null)
                    contacts = ContactModel.CreateSampleData().OrderBy(x => x.Name).ToList();
                return contacts;
            }
        }

        public IList Results
        {
            get { return results; }
            set
            {
                results = value;
                NotifyPropertyChanged();
            }
        }

        public string Query
        {
            get { return query; }
            set
            {
                query = value;
                UpdateSearchResults();
                NotifyPropertyChanged();
            }
        }

        private void UpdateSearchResults()
        {
            var searchString = Query.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(searchString))
                Results = Contacts;
            else
                Results = Contacts.Where(x => x.Name.ToLower().Contains(searchString)).ToList();
        }
    }
}
