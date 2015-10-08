using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TidshanteringDyskalkyli.Pages
{
    class TabbedPageExtenstion : TabbedPage
    {

        public TabbedPageExtenstion()
        {
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
