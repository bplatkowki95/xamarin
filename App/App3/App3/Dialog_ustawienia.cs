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
    public class Dialog_ustawienia : DialogFragment
    {
        Button dodajKategorie;
        Button usunKategorie;
        Button dodajPodkategorie;
        Button usunpodkategorie;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(DialogFragmentStyle.Normal, Android.Resource.Style.ThemeBlackNoTitleBarFullScreen);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Dialog_ustawienia, container, false);

            dodajKategorie = view.FindViewById<Button>(Resource.Id.dodajKategorie);
            usunKategorie = view.FindViewById<Button>(Resource.Id.usuńKategorie);
            dodajPodkategorie = view.FindViewById<Button>(Resource.Id.dodajPodkategorie);
            usunpodkategorie = view.FindViewById<Button>(Resource.Id.usuńPodkategorie);

            dodajKategorie.Click += DodajKategorie_Click;
            usunKategorie.Click += UsunKategorie_Click;
            dodajPodkategorie.Click += DodajPodkategorie_Click;
            usunpodkategorie.Click += Usunpodkategorie_Click;

            return view;
        }

        private void Usunpodkategorie_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            Dialog_dodajPodkategorie dialog = new Dialog_dodajPodkategorie(false);
            dialog.Show(transaction, "dialog");
        }

        private void DodajPodkategorie_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            Dialog_dodajPodkategorie dialog = new Dialog_dodajPodkategorie();
            dialog.Show(transaction, "dialog");
        }

        private void UsunKategorie_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            Dialog_dodajKategorie dialog = new Dialog_dodajKategorie(false);
            dialog.Show(transaction, "dialog");
        }

        private void DodajKategorie_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            Dialog_dodajKategorie dialog = new Dialog_dodajKategorie();
            dialog.Show(transaction, "dialog");
        }
    }
}