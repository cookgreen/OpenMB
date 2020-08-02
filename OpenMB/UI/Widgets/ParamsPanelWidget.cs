using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Widgets
{

	/// <summary>
	/// Basic parameters panel widget
	/// </summary>
	public class ParamsPanelWidget : Widget
	{
		protected TextAreaOverlayElement namesAreaElement;
		protected TextAreaOverlayElement valuesAreaElement;
		protected StringVector names = new StringVector();
		protected StringVector values = new StringVector();

		// Do not instantiate any widgets directly. Use SdkTrayManager.
		public ParamsPanelWidget(string name, float width, uint lines)
		{
			element = OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/ParamsPanel", "BorderPanel", name);
			OverlayContainer c = (OverlayContainer)element;
			namesAreaElement = (TextAreaOverlayElement)c.GetChild(Name + "/ParamsPanelNames");
			valuesAreaElement = (TextAreaOverlayElement)c.GetChild(Name + "/ParamsPanelValues");

			element.Width = (width);
			element.Height = (namesAreaElement.Top * 2f + lines * namesAreaElement.CharHeight);
		}

		public void SetAllParamNames(StringVector paramNames)
		{
			names = paramNames;
			values.Clear();
			values.Resize(names.Count, "");
			element.Height = (namesAreaElement.Top * 2 + names.Count * namesAreaElement.CharHeight);
			UpdateText();
		}

		public StringVector GetAllParamNames()
		{
			return names;
		}

		public void SetAllParamValues(StringVector paramValues)
		{
			values = paramValues;
			values.Resize(names.Count, "");
			UpdateText();
		}

		public void SetParamValue(string paramName, string paramValue)
		{
			for (int i = 0; i < names.Count; i++)
			{
				if (names[i] == DisplayStringToString(paramName))
				{
					values[i] = DisplayStringToString(paramValue);

					UpdateText();
					return;
				}
			}

			string desc = "ParamsPanel \"" + Name + "\" has no parameter \"" + DisplayStringToString(paramName) + "\".";
			OGRE_EXCEPT("Ogre::Exception::ERR_ITEM_NOT_FOUND", desc, "ParamsPanel::setParamValue");
		}

		public void SetParamValue(uint index, string paramValue)
		{
			if (index >= names.Count)
			{
				string desc = "ParamsPanel \"" + Name + "\" has no parameter at position " + (index).ToString() + ".";
				OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", desc, "ParamsPanel::setParamValue");
			}

			values[(int)index] = DisplayStringToString(paramValue);
			UpdateText();
		}

		public string GetParamValue(string paramName)
		{
			for (int i = 0; i < names.Count; i++)
			{
				if (names[i] == DisplayStringToString(paramName)) return values[i];
			}

			string desc = "ParamsPanel \"" + Name + "\" has no parameter \"" + DisplayStringToString(paramName) + "\".";
			OGRE_EXCEPT("Ogre::Exception::ERR_ITEM_NOT_FOUND", desc, "ParamsPanel::getParamValue");
			return "";
		}

		public string GetParamValue(uint index)
		{
			if (index >= names.Count)
			{
				string desc = "ParamsPanel \"" + Name + "\" has no parameter at position " + (index).ToString() + ".";
				OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", desc, "ParamsPanel::getParamValue");
			}

			return values[(int)index];
		}

		public StringVector GetAllParamValues()
		{
			return values;
		}


		//        -----------------------------------------------------------------------------
		//		| Internal method - updates text areas based on name and value lists.
		//		-----------------------------------------------------------------------------
		protected void UpdateText()
		{
			string namesDS = "";
			string valuesDS = "";

			for (int i = 0; i < names.Count; i++)
			{
				namesDS += (names[i] + ":\n");
				valuesDS += (values[i] + "\n");
			}

			namesAreaElement.Caption = (namesDS);
			valuesAreaElement.Caption = (valuesDS);
		}
	}
}
