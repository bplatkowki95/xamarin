using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Java.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Provider;
using Android.Content.PM;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using Android;

namespace App3
{
    public static class App
    {
        public static Java.IO.File _file;
        public static Java.IO.File _dir;
        public static Bitmap bitmap;
    }

    class Dialog_Dodaj : DialogFragment
    {
        private Spinner kategorie;
        private Spinner podkategorie;
        private EditText editText1;
        private EditText editText2;
        private Spinner cena;
        private RatingBar ocena;
        private EditText editText3;
        private Button zdjecie;
        private Button dodaj;
        private ImageView _imageView;
        private Przedmiot przedmiot;
        private Dictionary<string, List<string>> dictionary;

        public Dialog_Dodaj()
        { }

        public Dialog_Dodaj(Przedmiot Przedmiot)
        {
            przedmiot = Przedmiot;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(DialogFragmentStyle.Normal, Android.Resource.Style.ThemeBlackNoTitleBarFullScreen);

        }

        private void StworzFolderNaZdjecia()
        {
            App._dir = new Java.IO.File(
                Environment.GetExternalStoragePublicDirectory(
                    Environment.DirectoryPictures), "CameraAppDemo");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }

        public event EventHandler<PoDodaniuPrzedmiotuEvent> poDodaniuPrzedmiotu;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Dialog_dodaj, container, false);
            StworzFolderNaZdjecia();
            dictionary = ZapisOdczytTelefon.OdczytajKategorie();
            List<string> lPodkategorie = new List<string>();
            List<string> lKategorie = new List<string>();
            lKategorie = dictionary.Keys.ToList();
            
            List<string> lCena = new List<string>(new string[] { "kilogram", "litr", "sztuke" });

            kategorie = view.FindViewById<Spinner>(Resource.Id.kategorie);
            podkategorie = view.FindViewById<Spinner>(Resource.Id.podkategorie);
            editText1 = view.FindViewById<EditText>(Resource.Id.editText1);
            editText2 = view.FindViewById<EditText>(Resource.Id.editText2);
            cena = view.FindViewById<Spinner>(Resource.Id.cena);
            ocena = view.FindViewById<RatingBar>(Resource.Id.ratingBar1);
            editText3 = view.FindViewById<EditText>(Resource.Id.editText3);
            zdjecie = view.FindViewById<Button>(Resource.Id.zdjecie);
            dodaj = view.FindViewById<Button>(Resource.Id.dodaj);
            _imageView = view.FindViewById<ImageView>(Resource.Id.imageView1);

            dodaj.Click += Dodaj_Click;
            zdjecie.Click += Zdjecie_Click;

            var adapter2 = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleSpinnerItem, lCena);

            adapter2.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cena.Adapter = adapter2;

            var adapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleSpinnerItem, lKategorie);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            kategorie.Adapter = adapter;
            if (przedmiot != null)
            {
                int i = adapter.GetPosition(przedmiot.kategorie);
                kategorie.SetSelection(i);
                int k = adapter2.GetPosition(przedmiot.cenaZa);
                cena.SetSelection(k);
            }

            int initialSpinnerPosition = kategorie.SelectedItemPosition;
            kategorie.ItemSelected += (sender, args) =>
            {
                lPodkategorie = null;
                lPodkategorie = new List<string>();
                {
                    lPodkategorie = dictionary[kategorie.SelectedItem.ToString()];
                    if(lPodkategorie == null || lPodkategorie.Count==0)
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
                    if (przedmiot != null)
                    {
                        int j = adapter1.GetPosition(przedmiot.podkategorie);
                        podkategorie.SetSelection(j);
                    }
                }
            };

            if (przedmiot != null)
            {
                UzupelnijPola();
            }

            return view;

        }

        private void UzupelnijPola()
        {
            var path = "";
            if (App._file != null)
                path = App._file.Path;
            editText1.Text = przedmiot.nazwa;
            editText2.Text = przedmiot.cena.ToString();
            editText3.Text = przedmiot.opis;
            ocena.Rating = przedmiot.ocena;
            path = przedmiot.zdjecie;

            //int height = Resources.DisplayMetrics.HeightPixels;
            //int width = 200;
            //App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
            //if (App.bitmap != null)
            //{
            //	_imageView.SetImageBitmap(App.bitmap);
            //	App.bitmap = null;
            //}

            //// Dispose of the Java side bitmap.
            //GC.Collect();

        }



        private void Zdjecie_Click(object sender, EventArgs e)
        {
            Intent cameraIntent = new Intent(MediaStore.ActionImageCapture);

            App._file = new Java.IO.File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));

            cameraIntent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));

            StartActivityForResult(cameraIntent, 1888);
        }

        public override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Make it available in the gallery

            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Uri contentUri = Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            //Activity.SendBroadcast(mediaScanIntent);

            // Display in ImageView. We will resize the bitmap to fit the display
            // Loading the full sized image will consume to much memory 
            // and cause the application to crash.

            //int height = Resources.DisplayMetrics.HeightPixels;
            //int width = _imageView.Height;
            //App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
            //if (App.bitmap != null)
            //{
            //	_imageView.SetImageBitmap(App.bitmap);
            //	App.bitmap = null;
            //}

            //// Dispose of the Java side bitmap.
            //GC.Collect();
        }

        private void Dodaj_Click(object sender, EventArgs e)
        {
            if (PustePola())
            {
                var path = "";
                if (App._file != null)
                    path = App._file.Path;


                Przedmiot dodajPrzedmiot = new Przedmiot
                {
                    nazwa = editText1.Text,
                    cena = Convert.ToDouble(editText2.Text.Replace(".", ",")),
                    opis = editText3.Text,
                    kategorie = kategorie.SelectedItem.ToString(),
                    podkategorie = podkategorie.SelectedItem.ToString(),
                    cenaZa = cena.SelectedItem.ToString(),
                    ocena = ocena.Rating,
                    zdjecie = path

                };
                poDodaniuPrzedmiotu.Invoke(this, new PoDodaniuPrzedmiotuEvent(dodajPrzedmiot));
                this.Dismiss();
            }
        }

        private bool PustePola()
        {
            if (editText1.Text.Length == 0)
            {
                editText1.Error = "To pole nie może być puste!";
                return false;
            }
            if (editText2.Text.Length == 0)
            {
                editText2.Error = "To pole nie może być puste!";
                return false;
            }
            if (editText3.Text.Length == 0)
            {
                editText3.Error = "To pole nie może być puste!";
                return false;
            }
            return true;
        }

        public class PoDodaniuPrzedmiotuEvent : EventArgs
        {
            public Przedmiot przedmiot;

            public PoDodaniuPrzedmiotuEvent(Przedmiot przedmiot)
            {
                this.przedmiot = przedmiot;
            }

        }


    }

}