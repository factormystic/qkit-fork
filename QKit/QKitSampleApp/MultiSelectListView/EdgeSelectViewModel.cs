using QKit.JumpList;
using QKitSampleApp.Models;
using System.Collections;
using Windows.UI.Xaml.Data;

namespace QKitSampleApp.MultiSelectListView
{
    public class EdgeSelectViewModel
    {
        private IList data;

        public IList Data
        {
            get
            {
                if (data == null)
                {
                    var items = MovieModel.CreateSampleData();
                    data = items;
                }
                return data;
            }
        }
    }
}
