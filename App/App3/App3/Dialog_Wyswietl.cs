using System;
using System.Collections.Generic;
using System.IO;
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
	public class Dialog_Wyswietl : DialogFragment, IMenuItemOnMenuItemClickListener
	{
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetStyle(DialogFragmentStyle.Normal, Android.Resource.Style.ThemeBlackNoTitleBarFullScreen);
		}

		private ListView lista;
		private List<Przedmiot> listaPrzedmiotow;

		public Dialog_Wyswietl(List<Przedmiot> listaPrzedmiotow)
		{
			this.listaPrzedmiotow = listaPrzedmiotow;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			var view = inflater.Inflate(Resource.Layout.Dialog_wyswietl, container, false);
			lista = view.FindViewById<ListView>(Resource.Id.listView1);

			var adapter = new PrzedmiotAdapter(Context, Resource.Layout.Przedmiot, listaPrzedmiotow);
			lista.Adapter = adapter;

			lista.ItemClick += Lista_ItemClick;
			RegisterForContextMenu(lista);
			lista.SetOnCreateContextMenuListener(this);



			return view;
		}

		private void Lista_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			FragmentTransaction transaction = FragmentManager.BeginTransaction();
			Dialog_Przedmiot dialog = new Dialog_Przedmiot(listaPrzedmiotow[e.Position]);
			dialog.Show(transaction, "dialog");
		}


		public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
		{


			if (v.Id == Resource.Id.listView1)
			{
				var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
				var menuItems = Resources.GetStringArray(Resource.Array.context_menu_items);
				for (var i = 0; i < 2; i++)
				{
					menu.Add(Menu.None, i, i, menuItems[i]);
					menu.FindItem(i).SetOnMenuItemClickListener(this);
				}
			}
		}

		public bool obsluzZaznaczonyPrzedmiot(IMenuItem item)
		{
			var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
			var menuItemIndex = item.ItemId;
			var menuItems = Resources.GetStringArray(Resource.Array.context_menu_items);
			var menuItemName = menuItems[menuItemIndex];
			HandleContentItemSelect(menuItemName, info.Position);
			return true;
		}



		private void HandleContentItemSelect(string menuItemName, int position)
		{
			switch (menuItemName)
			{
				case "Usuń":
					usunPrzedmiot(position);
					break;
				case "Edytuj":
					edytujPrzedmiot(position);
					break;
				default:

					break;
			}
		}
		int pozycja;
		private void edytujPrzedmiot(int position)
		{
			
			FragmentTransaction transaction = FragmentManager.BeginTransaction();
			Dialog_Dodaj dialog = new Dialog_Dodaj(listaPrzedmiotow[position]);
			dialog.Show(transaction, "dialog");
			pozycja = position;
			dialog.poDodaniuPrzedmiotu += Dialog_poDodaniuPrzedmiotu;
		}

		private void Dialog_poDodaniuPrzedmiotu(object sender, Dialog_Dodaj.PoDodaniuPrzedmiotuEvent e)
		{
			if(!(listaPrzedmiotow[pozycja].zdjecie==e.przedmiot.zdjecie))
			{
				if (File.Exists(listaPrzedmiotow[pozycja].zdjecie))
				{
					File.Delete(listaPrzedmiotow[pozycja].zdjecie);
				}
			}
			listaPrzedmiotow.Add(e.przedmiot);
			ZapisOdczytTelefon.ZapiszDoPliku(listaPrzedmiotow);
			var adapter = new PrzedmiotAdapter(Context, Resource.Layout.Przedmiot, listaPrzedmiotow);
			lista.Adapter = adapter;
			listaPrzedmiotow.RemoveAt(pozycja);
		}

		private void usunPrzedmiot(int index)
		{
			if(File.Exists(listaPrzedmiotow[index].zdjecie))
			{
				File.Delete(listaPrzedmiotow[index].zdjecie);
			}
			listaPrzedmiotow.RemoveAt(index);
			var adapter = new PrzedmiotAdapter(Context, Resource.Layout.Przedmiot, listaPrzedmiotow);
			lista.Adapter = adapter;
			ZapisOdczytTelefon.ZapiszDoPliku(listaPrzedmiotow);
		}

		public bool OnMenuItemClick(IMenuItem item)
		{
			obsluzZaznaczonyPrzedmiot(item);
			return true;
		}
	}
}
