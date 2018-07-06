using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wisielec
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Gra : ContentPage {
		char[] litery = { 'A', 'Ą', 'B', 'C', 'Ć', 'D', 'E', 'Ę', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'Ł', 'M', 'N', 'Ń', 'O', 'Ó', 'P', 'Q', 'R', 'S', 'Ś', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'Ż', 'Ź' };

		double skala;

		string haslo, hasloUkryte;
		int iloscBledow;

        Wymiary podstawoweWymiary;
        
        public Gra() {
			InitializeComponent();

            podstawoweWymiary = GetWymiary();
            SizeChanged += (sender, e) => Skaluj();

            Skaluj();
			RozpocznijGre();
		}

        public void KlikniecieLitery(object sender, EventArgs args) {
			var litera = sender as Button;

			Sprawdz(litera);
		}

		void RysujKlawiture() {
			var klawiatura = new Grid();
			for (int i = 0; i < 5; i++) klawiatura.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			for (int i = 0; i < 7; i++) klawiatura.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			for (int i = 0; i < litery.Length; i++) {
                Button button = new Button {
                    Text = litery[i].ToString(),
                    BackgroundColor = Color.Gray
				};
				button.FontSize *= skala;
				button.Clicked += KlikniecieLitery;

				klawiatura.Children.Add(button, i % 7, i / 7);
			}
			KlawiaturaWynikStcLay.Children.Clear();
			KlawiaturaWynikStcLay.Children.Add(klawiatura);
		}

		void RozpocznijGre() {
			iloscBledow = 0;
            Haslo hasloWylosowane = Hasla.LosujHaslo();

            haslo = hasloWylosowane.haslo.ToUpper();
			hasloUkryte = Regex.Replace(haslo, "[A-ZĄĆĘŁŃÓŚŻŹ]", "□");
			HasloLbl.Text = hasloUkryte;

            KategoriaLbl.Text = "Kategoria: " + hasloWylosowane.kategoria.ToString("F");

			RysujKlawiture();
			SzubienicaImg.Source = "s0.jpg";
		}

		string ZamienZnak(string wCzym, char naCo, int gdzie) {
			if (gdzie > wCzym.Length - 1) return wCzym;
			else return wCzym.Substring(0, gdzie) + naCo + wCzym.Substring(gdzie + 1);
		}

		void Sprawdz(Button litera) {
			int nrLitery = Array.IndexOf(litery, litera.Text[0]);
			bool jestTakaLitera = false;

			for (int i = 0; i < haslo.Length; i++) {
				if (haslo[i] == litery[nrLitery]) {
					hasloUkryte = ZamienZnak(hasloUkryte, litery[nrLitery], i);
					jestTakaLitera = true;
				}
			}

			if (jestTakaLitera) {
				litera.IsEnabled = false;
				litera.BackgroundColor = Color.Green;

				HasloLbl.Text = hasloUkryte;
			} else {
				litera.IsEnabled = false;
				litera.BackgroundColor = Color.Red;

				SzubienicaImg.Source = "s" + ++iloscBledow + ".jpg";
			}

			if (haslo == hasloUkryte) PiszWynik(true);
			if (iloscBledow >= 9) { HasloLbl.Text = haslo; PiszWynik(false); }
		}

		void PiszWynik(bool czyWygrana) {
			KlawiaturaWynikStcLay.Children.Clear();

			KlawiaturaWynikStcLay.Children.Add(new Label {
				Text = czyWygrana ? "Wygrałeś!" : "Przegarłeś",
				TextColor = Color.Gray,
				FontSize = 48 * skala,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.Center,
				Margin = 15 * skala
			});

			Button button = new Button {
				Text = "Od nowa?",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.Center,
				BackgroundColor = Color.Gray,
				TextColor = Color.Black,
				FontSize = 24 * skala,
				WidthRequest = 200 * skala
			};
			button.Clicked += RestartGry;

			KlawiaturaWynikStcLay.Children.Add(button);

			Button button2 = new Button {
				Text = "Powrót do menu",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.Center,
				BackgroundColor = Color.Gray,
				TextColor = Color.Black,
				FontSize = 24 * skala,
				WidthRequest = 200 * skala
			};
			button2.Clicked += PowrotDoMenu;

			KlawiaturaWynikStcLay.Children.Add(button2);
		}

		void RestartGry(object sender, EventArgs args) {
			RozpocznijGre();
		}

		async void PowrotDoMenu(object sender, EventArgs args) {
			await this.Navigation.PopAsync();
		}

		void Skaluj() {
			double skala = Application.Current.MainPage.Height / 750;
			HasloLbl.FontSize = podstawoweWymiary.hasloFont * skala;
            KategoriaLbl.FontSize =  podstawoweWymiary.kategoriaFont * skala;
            SzubienicaImg.WidthRequest = podstawoweWymiary.szubienicaSzerokosc * skala;
			SzubienicaImg.HeightRequest = podstawoweWymiary.szubienicaWysokosc * skala;
			SzubienicaImg.Margin = new Thickness(podstawoweWymiary.szubienicaMargines * skala);
			Zawartosc.Padding = new Thickness(podstawoweWymiary.zawartoscPadding * skala);

            this.skala = skala;
		}

        Wymiary GetWymiary() {
            Wymiary wymiary;

            wymiary.hasloFont = (int)HasloLbl.FontSize;
            wymiary.kategoriaFont = (int)KategoriaLbl.FontSize;
            wymiary.szubienicaSzerokosc = (int)SzubienicaImg.WidthRequest;
            wymiary.szubienicaWysokosc = (int)SzubienicaImg.HeightRequest;
            wymiary.szubienicaMargines = (int)SzubienicaImg.Margin.Top;
            wymiary.zawartoscPadding = (int)Zawartosc.Padding.Top;

            return wymiary;
        }
	}

    struct Wymiary {
        public int hasloFont;
        public int kategoriaFont;
        public int szubienicaSzerokosc;
        public int szubienicaWysokosc;
        public int szubienicaMargines;
        public int zawartoscPadding;
    }

    enum Kategorie{
        Tytuł,
        Przedmiot,
        Powiedzenie,
        Czynność,
        Miejsce,
        Postać
    }

    class Haslo {
        public string haslo;
        public Kategorie kategoria;
    }

	class Hasla {
		static Haslo[] hasla = {
			new Haslo {
                haslo = "Zmierzch",
                kategoria = Kategorie.Tytuł
            },
            new Haslo {
                haslo = "Harry Potter",
                kategoria = Kategorie.Tytuł
            },
            new Haslo {
                haslo = "300 Spartan",
                kategoria = Kategorie.Tytuł
            },
            new Haslo {
                haslo = "Bałwan",
                kategoria = Kategorie.Przedmiot
            },
            new Haslo {
                haslo = "Szampan",
                kategoria = Kategorie.Przedmiot
            },
            new Haslo {
                haslo = "Fajrwerki",
                kategoria = Kategorie.Przedmiot
            },
            new Haslo {
                haslo = "Burza mózgów",
                kategoria = Kategorie.Powiedzenie
            },
            new Haslo {
                haslo = "Przechodzić samego siebie",
                kategoria = Kategorie.Powiedzenie
            },
            new Haslo {
                haslo = "Tępy jak osioł",
                kategoria = Kategorie.Powiedzenie
            },
            new Haslo {
                haslo = "Raz kozie śmierć",
                kategoria = Kategorie.Powiedzenie
            },
            new Haslo {
                haslo = "Gdy dwóch sie bije, trzeci korzysta",
                kategoria = Kategorie.Powiedzenie
            },
            new Haslo {
                haslo = "Panna z Mokrą Głową",
                kategoria = Kategorie.Tytuł
            },
            new Haslo {
                haslo = "Nie wywołuj wilka z lasu",
                kategoria = Kategorie.Powiedzenie
            },
            new Haslo {
                haslo = "Głowa Pusta jak kapusta",
                kategoria = Kategorie.Powiedzenie
            },
            new Haslo {
                haslo = "Drapieżny jak lew",
                kategoria = Kategorie.Powiedzenie
            },
            new Haslo {
                haslo = "M jak miłość",
                kategoria = Kategorie.Tytuł
            },
            new Haslo {
                haslo = "Syrenka",
                kategoria = Kategorie.Postać
            },
            new Haslo {
                haslo = "Kluski Śląskie",
                kategoria = Kategorie.Przedmiot
            },
            new Haslo {
                haslo = "Taniec Brzucha",
                kategoria = Kategorie.Czynność
            },
            new Haslo {
                haslo = "Casino Royal",
                kategoria = Kategorie.Tytuł
            },
            new Haslo {
                haslo = "Hawaje",
                kategoria = Kategorie.Miejsce
            },
            new Haslo {
                haslo = "Cylinder",
                kategoria = Kategorie.Przedmiot
            },
            new Haslo {
                haslo = "Z deszczu pod rynne",
                kategoria = Kategorie.Powiedzenie
            }
		};

		public static Haslo LosujHaslo() {
			Random r = new Random();
			return hasla[r.Next(hasla.Length)];
		}
	}
}