namespace Gaku
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MenuPrincipal());
        }
    }
}
