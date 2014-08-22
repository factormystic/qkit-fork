using QKit.JumpList;
using QKitSampleApp.Models;
using System.Collections;
using Windows.UI.Xaml.Data;

namespace QKitSampleApp.JumpListSample
{
    public class MoviesViewModel
    {
        private IList data;
        private CollectionViewSource collection;

        public IList Data
        {
            get
            {
                if (data == null)
                {
                    var items = MovieModel.CreateSampleData();
                    data = items.ToGroups(x => x.Name, x => x.Category);
                }
                return data;
            }
        }

        public CollectionViewSource Collection
        {
            get
            {
                if (collection == null)
                {
                    collection = new CollectionViewSource();
                    collection.Source = Data;
                    collection.IsSourceGrouped = true;
                }
                return collection;
            }
        }
    }
}
