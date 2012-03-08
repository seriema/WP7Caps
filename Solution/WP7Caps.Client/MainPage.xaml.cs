using Microsoft.Phone.Controls;
using WinPhoneCaps.Client.ViewModels;

namespace WinPhoneCaps.Client
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();			
        }

        private void Email_Click(object sender, System.EventArgs e)
        {
			var vm = DataContext as MainPageViewModel;
			if(vm != null)
				vm.EmailData();
        }

        private void PhoneApplicationPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = LayoutRoot.DataContext as MainPageViewModel;
            if (vm != null)
                vm.Load(Dispatcher);
        }
    }
}