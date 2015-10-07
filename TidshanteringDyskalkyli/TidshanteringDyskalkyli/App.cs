using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TidshanteringDyskalkyli.Pages;
using Xamarin.Forms;

namespace TidshanteringDyskalkyli
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            MainPage = new TabbedPage()
            {
                Children =
                {
                    new TimeTranslatePage(),
                    new TimeEstimatorPage()
                }
            };
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
