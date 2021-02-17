using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace App3
{
    public class Dialog_dodajPodkategorie : DialogFragment
    {
        bool dodawanie = true;
        TextView textView;
        Spinner kategorie;
        Spinner podkategorie;
        EditText edittext;
        Button zapisz;
        Dictionary<string, List<string>> dictionary;
        public Dialog_dodajPodkategorie() { }
        public Dialog_dodajPodkategorie(bool Dodawanie)
        {
            if (!Dodawanie) dodawanie = false;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            dictionary = ZapisOdczytTelefon.OdczytajKategorie();
            SetStyle(DialogFragmentStyle.Normal, Android.Resource.Style.ThemeDeviceDefaultDialogNoActionBar);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Dialog_dodajPodkategorie, container, false);
            textView = view.FindViewById<TextView>(Resource.Id.textView1);
            kategorie = view.FindViewById<Spinner>(Resource.Id.spinner1);
            podkategorie = view.FindViewById<Spinner>(Resource.Id.spinner2);

            edittext = view.FindViewById<EditText>(Resource.Id.editText1);
            zapisz = view.FindViewById<Button>(Resource.Id.zapiszKategorie);

            var lista = dictionary.Keys.ToList();
            var adapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleSpinnerItem, lista);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            kategorie.Adapter = adapter;

            if (dodawanie)
            {
                podkategorie.Visibility = ViewStates.Gone;
            }
            else
            {
               
                textView.Text = "Usuń podkategorię";
                zapisz.Text = "Usuń";
                edittext.Visibility = ViewStates.Gone;

                int initialSpinnerPosition = kategorie.SelectedItemPosition;
                kategorie.ItemSelected += (sender, args) =>
                {
                    var lPodkategorie = new List<string>();
                    {
                        lPodkategorie = dictionary[kategorie.SelectedItem.ToString()];
                        if (lPodkategorie == null || lPodkategorie.Count == 0)
                        {
                            lPodkategorie = new List<string>(); lPodkategorie.Add("Brak podkategorii");
                            podkategorie.Enabled = false;
                        }
                        else
                        {
                            podkategorie.Enabled = true;
                        }
                        var adapter1 = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleSpinnerItem, lPodkategorie);
                        adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                        podkategorie.Adapter = adapter1;
                        
                    }
                };
            }
            zapisz.Click += Zapisz_Click;
            return view;
        }

        private void Zapisz_Click(object sender, EventArgs e)
        {
            if (dodawanie)
            {
                var selected = kategorie.SelectedItem.ToString();
                List<string> lista;
                var asd = dictionary[selected];
                if (asd != null)
                {
                    lista = asd.ToList();
                }
                else
                {
                    lista = new List<string>();
                }
                lista.Add(edittext.Text);
                dictionary[selected] = lista;
            }
            else
            {
                var selected = kategorie.SelectedItem.ToString();
                var lista = dictionary[selected].ToList();
                lista.Remove(podkategorie.SelectedItem.ToString());
                dictionary[selected] = lista;
            }
            ZapisOdczytTelefon.ZapiszKategorie(dictionary);
            this.Dismiss();

        }
    }
}