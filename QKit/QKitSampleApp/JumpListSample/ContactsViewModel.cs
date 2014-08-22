using QKit.JumpList;
using QKitSampleApp.Models;
using System.Collections;
using Windows.UI.Xaml.Data;

namespace QKitSampleApp.JumpListSample
{
    public class ContactsViewModel
    {
        private IList data;

        public IList Data
        {
            get
            {
                if (data == null)
                {
                    var items = ContactModel.CreateSampleData();
                    data = items.ToAlphaGroups(x => x.Name);
                }
                return data;
            }
        }
    }
}
