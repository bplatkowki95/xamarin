using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Provider;
using System;
using Android;
using Android.Content.PM;
using System.IO;

namespace App3
{
	[Activity(Label = "App2", MainLauncher = true)]
	public class MainActivity : Activity
	{
		Button dodaj;
		Button wyswietl;
        Button ustawienia;
		List<Przedmiot> lPrzedmioty;

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{

			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Main);

			lPrzedmioty = new List<Przedmiot>();

			dodaj = FindViewById<Button>(Resource.Id.button1);
			wyswietl = FindViewById<Button>(Resource.Id.button2);
            ustawienia = FindViewById<Button>(Resource.Id.ustawienia);
			dodaj.Click += Dodaj_Click;
			wyswietl.Click += Wyswietl_Click;
            ustawienia.Click += Ustawienia_Click;
			lPrzedmioty = ZapisOdczytTelefon.odczytajZpliku();
		}

        private void Ustawienia_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            Dialog_ustawienia dialog = new Dialog_ustawienia();
            dialog.Show(transaction, "dialog");
        }

        private void Wyswietl_Click(object sender, EventArgs e)
		{
			FragmentTransaction transaction = FragmentManager.BeginTransaction();
			Dialog_Wyswietl dialog = new Dialog_Wyswietl(lPrzedmioty);
			dialog.Show(transaction, "dialog");
		}

		private void Dodaj_Click(object sender, EventArgs e)
		{
			FragmentTransaction transaction = FragmentManager.BeginTransaction();
			Dialog_Dodaj dialog = new Dialog_Dodaj();
			dialog.Show(transaction, "dialog");
			dialog.poDodaniuPrzedmiotu += Dialog_poDodaniuPrzedmiotu;
		}

		private void Dialog_poDodaniuPrzedmiotu(object sender, Dialog_Dodaj.PoDodaniuPrzedmiotuEvent e)
		{
			lPrzedmioty.Add(e.przedmiot);
			ZapisOdczytTelefon.ZapiszDoPliku(lPrzedmioty);
		}


		protected override void OnStart()
		{
			base.OnStart();
			CheckAppPermissions();
            if (PierwszeUruchomienie()) ZapisOdczytTelefon.StworzKategorie();
		}

		private void CheckAppPermissions()
		{
			if ((int)Build.VERSION.SdkInt < 23)
			{
				return;
			}
			else
			{
				if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
					&& PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted)
				{
					var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
					RequestPermissions(permissions, 1);
				}
			}
		}

        private bool PierwszeUruchomienie()
        {
            string nazwaPliku = "kategorie.json";
            string sciezka = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string sciezkaDoPliku = Path.Combine(sciezka, nazwaPliku);
            if (!File.Exists(sciezkaDoPliku)) return true;
            return false;
        }


	}
}

