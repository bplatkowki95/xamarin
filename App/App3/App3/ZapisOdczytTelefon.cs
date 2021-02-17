using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace App3
{
	public static class ZapisOdczytTelefon
	{
		static string nazwaPliku = "ListaPrzedmiotow.json";
        static string nazwaPlikuKategorie = "kategorie.json";

        public static void ZapiszDoPliku(List<Przedmiot> lista)
		{
			string sciezka = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			string sciezkaDoPliku = Path.Combine(sciezka, nazwaPliku);
			if (File.Exists(sciezkaDoPliku))
			{
				File.Delete(sciezkaDoPliku);
				return;
			}

			string json = JsonConvert.SerializeObject(lista);

			using (var plik = File.Open(sciezkaDoPliku, FileMode.OpenOrCreate, FileAccess.Write))
			using (var strumien = new StreamWriter(plik))
			{
				strumien.Write(json);
			}
		}
		public static List<Przedmiot> odczytajZpliku()
		{
			List<Przedmiot> lista = new List<Przedmiot>();
			string json;
			string sciezka = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			string sciezkaDoPliku = Path.Combine(sciezka, nazwaPliku);
			if (!File.Exists(sciezkaDoPliku)) return lista;
			using (var plik = File.Open(sciezkaDoPliku, FileMode.Open, FileAccess.Read))
			using (var strumien = new StreamReader(plik))
			{
				json = strumien.ReadToEnd();
			}
			lista = JsonConvert.DeserializeObject<List<Przedmiot>>(json);
			return lista;

		}
        public static void StworzKategorie()
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            var list = new List<string>();
            //kat. chemia
            list.Add("Pasta");
            list.Add("Szampon");
            list.Add("Płyn do naczyn");
            list.Add("Cos TAm");
            dictionary.Add("Chemia", list);
            //kolejna kategoria
            var list2 = new List<string>();
            list2.Add("asd");
            list2.Add("wad");
            list2.Add("Płynsdsd");
            list2.Add("12 TAm");
            list2.Add("23");
            list2.Add("ad2dasd");
            list2.Add("123 do naczyn");
            list2.Add("dw TAm");
            dictionary.Add("Losowo", list2);
            //i nastepna

            string sciezka = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string sciezkaDoPliku = Path.Combine(sciezka, nazwaPlikuKategorie);

            string json = JsonConvert.SerializeObject(dictionary);
            using (var plik = File.Open(sciezkaDoPliku, FileMode.OpenOrCreate, FileAccess.Write))
            using (var strumien = new StreamWriter(plik))
            {
                strumien.Write(json);
            }

        }

        public static Dictionary<string, List<string>> OdczytajKategorie()
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();

            string json;
            string sciezka = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string sciezkaDoPliku = Path.Combine(sciezka, nazwaPlikuKategorie);
            if (!File.Exists(sciezkaDoPliku)) return dictionary;
            using (var plik = File.Open(sciezkaDoPliku, FileMode.Open, FileAccess.Read))
            using (var strumien = new StreamReader(plik))
            {
                json = strumien.ReadToEnd();
            }
            dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
            return dictionary;
        }

        public static void ZapiszKategorie(Dictionary<string,List<string>> dictionary)
        {
            string sciezka = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string sciezkaDoPliku = Path.Combine(sciezka, nazwaPlikuKategorie);
            if (File.Exists(sciezkaDoPliku))
            {
                File.Delete(sciezkaDoPliku);
            }

            string json = JsonConvert.SerializeObject(dictionary);

            using (var plik = File.Open(sciezkaDoPliku, FileMode.OpenOrCreate, FileAccess.Write))
            using (var strumien = new StreamWriter(plik))
            {
                strumien.Write(json);
            }
        }
    }
}