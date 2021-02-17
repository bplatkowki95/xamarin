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
    public class Dialog_dodajKategorie : DialogFragment
    {
        bool dodawanie = true;
        TextView textView;
        Spinner kategorie;
        EditText edittext;
        Button zapisz;
        Dictionary<string, List<string>> dictionary;
        public Dialog_dodajKategorie() { }
        public Dialog_dodajKategorie(bool Dodawanie)
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

            var view = inflater.Inflate(Resource.Layout.Dialog_dodajKategorie, container, false);
            textView = view.FindViewById<TextView>(Resource.Id.textView1);
            kategorie = view.FindViewById<Spinner>(Resource.Id.spinner1);
            edittext = view.FindViewById<EditText>(Resource.Id.editText1);
            zapisz = view.FindViewById<Button>(Resource.Id.zapiszKategorie);
            if (dodawanie)
            {
                kategorie.Visibility = ViewStates.Gone;
            }
            else
            {
                var lista = dictionary.Keys.ToList();
                var adapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleSpinnerItem, lista);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                kategorie.Adapter = adapter;
                textView.Text = "Usuń kategorię";
                zapisz.Text = "Usuń";
                edittext.Visibility = ViewStates.Gone;

            }
            zapisz.Click += Zapisz_Click;
            return view;
        }

        private void Zapisz_Click(object sender, EventArgs e)
        {
            if (dodawanie)
            {
                dictionary.Add(edittext.Text, null);
            }
            else
            {
                dictionary.Remove(kategorie.SelectedItem.ToString());
            }
            ZapisOdczytTelefon.ZapiszKategorie(dictionary);
            this.Dismiss();

        }
    }
}