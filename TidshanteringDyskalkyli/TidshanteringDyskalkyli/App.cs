using TidshanteringDyskalkyli.Pages;
using Xamarin.Forms;

namespace TidshanteringDyskalkyli
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application

            
            var tabbedPage = new TabbedPageExtenstion
            {
                Children =
                    {
                        new ScrollTranslatePage(),
                        new TimeTranslatePage(),
                        new TimeEstimatorPage()

                    },
                Title = "Tidshantering",
            };
            var navigationPage = new NavigationPage(tabbedPage);
            MainPage = navigationPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
