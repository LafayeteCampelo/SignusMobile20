using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SignusMobile20.ExpandableListView
{
    public class ListViewPageModel
    {
        public ObservableCollection<Person__> PersonsList { get; set; }

        private Person__ _selectedPerson { get; set; }
        public Person__ SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                if (_selectedPerson != value)
                {
                    _selectedPerson = value;
                    ExpandOrCollapseSelectedItem();
                }
            }
        }

        private void ExpandOrCollapseSelectedItem()
        {
            PersonsList.Where(t => t.Id == SelectedPerson.Id).FirstOrDefault().isVisible =
                !SelectedPerson.isVisible;
        }

        public ListViewPageModel()
        {
            PersonsList = new ObservableCollection<Person__>
            {
                new Person__() { Id = 1, FirstName = "John", LastName = "Smith", City = "Los Angeles", MobileNo = "532213" },
                new Person__() { Id = 2, FirstName = "Alex", LastName = "Smith", City = "Los Angeles", MobileNo = "1232213" },
                new Person__() { Id = 3, FirstName = "Helen", LastName = "Smith", City = "Los Angeles", MobileNo = "983929" },
                new Person__() { Id = 4, FirstName = "Grace", LastName = "Smith", City = "Los Angeles", MobileNo = "9504393" }
            };
        }
    }

    public class Person__ : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string MobileNo { get; set; }
        private bool _isVisible { get; set; }
        public bool isVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
