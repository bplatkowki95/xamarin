using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App3
{
	class PrzedmiotAdapter : BaseAdapter<Przedmiot>
	{
		private Context mContext;
		private int WygladWiersza;
		private List<Przedmiot> lPrzedmiot;

		public PrzedmiotAdapter(Context mContext, int wygladWiersza, List<Przedmiot> lPrzedmiot)
		{
			this.mContext = mContext;
			WygladWiersza = wygladWiersza;
			this.lPrzedmiot = lPrzedmiot;
		}

		public override int Count
		{
			get { return lPrzedmiot.Count; }
		}

		public override Przedmiot this[int position]
		{
			get { return lPrzedmiot[position]; }
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = lPrzedmiot[position];
			View wiersz = convertView;
			if (wiersz == null)
			{
				wiersz = LayoutInflater.From(mContext).Inflate(WygladWiersza, parent, false);
			}

			TextView nazwa = wiersz.FindViewById<TextView>(Resource.Id.editText1);
			nazwa.Text = lPrzedmiot[position].nazwa;

			TextView cena = wiersz.FindViewById<TextView>(Resource.Id.editText2);
			cena.Text = lPrzedmiot[position].cena.ToString();

			ImageView zdjecie = wiersz.FindViewById<ImageView>(Resource.Id.Image);
			var sd = BitmapHelpers.LoadAndResizeBitmap(lPrzedmiot[position].zdjecie, 200, 200);
			if (sd != null)
			{
				zdjecie.SetImageBitmap(sd);
			}


			//wiersz.FindViewById<TextView>(Resource.Id.editText1).Text = item.Heading;
			//wiersz.FindViewById<TextView>(Resource.Id.editText2).Text = item.SubHeading;
			//wiersz.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(item.ImageResourceId);

			return wiersz;

		}
	}
}