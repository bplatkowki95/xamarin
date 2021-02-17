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
using App3;

namespace App3
{
	public class Dialog_Przedmiot : DialogFragment
	{
		private Przedmiot przedmiot;

		TextView nazwa;
		TextView cena;
		TextView cenaZa;
		TextView opis;
		ImageView zdjecie;
		RatingBar ocena;

		public Dialog_Przedmiot(Przedmiot przedmiot1)
		{
			this.przedmiot = przedmiot1;
		}
		
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetStyle(DialogFragmentStyle.Normal, Android.Resource.Style.ThemeBlackNoTitleBarFullScreen);
		}



		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);

			var view = inflater.Inflate(Resource.Layout.Dialog_przedmiot, container, false);

			nazwa = view.FindViewById<TextView>(Resource.Id.textView1);
			nazwa.Text = przedmiot.nazwa;

			cena = view.FindViewById<TextView>(Resource.Id.textView2);
			cena.Text = przedmiot.cena.ToString();

			cenaZa = view.FindViewById<TextView>(Resource.Id.textView3);
			cenaZa.Text = przedmiot.cenaZa;

			opis = view.FindViewById<TextView>(Resource.Id.textView4);
			opis.Text = przedmiot.opis;

			ocena = view.FindViewById<RatingBar>(Resource.Id.ratingBar1);
			ocena.Rating = przedmiot.ocena;

			zdjecie = view.FindViewById<ImageView>(Resource.Id.imageView1);
			var sd = BitmapHelpers.LoadAndResizeBitmap(przedmiot.zdjecie, 600, 600);
			if (sd != null)
			{
				zdjecie.SetImageBitmap(sd);
			}


			return view;
		}
	}
}