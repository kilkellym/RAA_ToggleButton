#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RAA_ToggleButton.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows.Controls;
using System.Windows.Markup;
using static System.Net.Mime.MediaTypeNames;

#endregion

namespace RAA_ToggleButton
{
	internal class App : IExternalApplication
	{
		public static string paletteState = "show";
		public static App _app = null;
		public PushButton pbPalette = null;

		public static App Instance
		{
			get { return _app; }
		}

		public Result OnStartup(UIControlledApplication app)
		{
			// 0. create instance of the application for use with show/hide
			_app = this;

			// 1. Create ribbon tab
			string tabName = "Ribbon Test";
			try
			{
				app.CreateRibbonTab(tabName);
			}
			catch (Exception)
			{
				Debug.Print("Tab already exists.");
			}

			// 2. Create ribbon panel 
			RibbonPanel panel = Utils.CreateRibbonPanel(app, tabName, "Revit Tools");

			// 3. Create button data instances
			PushButtonData btnData1 = new PushButtonData("Toggle", "Hide/Show" + "\r\n" + "Palette",
				Assembly.GetExecutingAssembly().Location,
				"RAA_ToggleButton.Command1");

			// 4. Set button image based on palette state
			try
			{
				if (paletteState == "show")
				{
					btnData1.LargeImage = ButtonDataClass.BitmapToImageSource(Resources.Blue_32);
				}
				else
				{
					btnData1.LargeImage = ButtonDataClass.BitmapToImageSource(Resources.Red_32);
				}
			}
			catch (Exception ex)
			{
				Debug.Print("Could not find images. " + ex.Message);
			}

			// 5. Create buttons
			pbPalette = panel.AddItem(btnData1) as PushButton;

			return Result.Succeeded;
		}
		public void toggle()
		{
			if (paletteState == "show")
			{
				pbPalette.LargeImage = ButtonDataClass.BitmapToImageSource(Resources.Red_32);
				pbPalette.ToolTip = "Hide Palette";
				paletteState = "hide";
			}
			else
			{
				pbPalette.LargeImage = ButtonDataClass.BitmapToImageSource(Resources.Blue_32);
				pbPalette.ToolTip = "Show Palette";
				paletteState = "show";
			}
		}
		public Result OnShutdown(UIControlledApplication a)
		{
			return Result.Succeeded;
		}
	}
}
