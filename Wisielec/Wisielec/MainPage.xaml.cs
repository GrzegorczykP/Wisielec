using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace Wisielec
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
        }

		async public void NowaGra(object sender, EventArgs args) {
			await Navigation.PushAsync(new Wisielec.Gra());
		}

		public void Wyjscie(object sender, EventArgs args) {
            System.Environment.Exit(0);
        }
	}
}
