using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OpenMB.Forms.Model
{
	public class GraphicConfigure : Configure
	{
		private string renderSystemName;
		private BindingList<string> renderSystems;
		private BindingList<string> renderParams;
		private BindingList<string> possibleValues;
		private string currentPossibleValue;
		public string RenderSystem
		{
			get
			{
				return renderSystemName;
			}
			set
			{
				renderSystemName = value;
				OnPropertyChanged("RenderSystem");
			}
		}
		public BindingList<string> RenderSystemNames
		{
			get
			{
				return renderSystems;
			}
			set
			{
				renderSystems = value;
			}
		}
		public BindingList<string> RenderParams
		{
			get
			{
				return renderParams;
			}
			set
			{
				renderParams = value;
				OnPropertyChanged("RenderParams");
			}
		}
		public BindingList<string> PossibleValues
		{
			get
			{
				return possibleValues;
			}
			set
			{
				possibleValues = value;
				OnPropertyChanged("CurrentPossibleValues");
			}
		}
		public string CurrentPossibleValue
		{
			get
			{
				return currentPossibleValue;
			}
			set
			{
				currentPossibleValue = value;
				OnPropertyChanged("CurrentPossibleValue");
			}
		}
		public GraphicConfigure()
		{
			renderSystems = new BindingList<string>();
			renderParams = new BindingList<string>();
			possibleValues = new BindingList<string>();
		}
	}
}
