using Mogre;
using Mogre_Procedural.MogreBites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Widgets
{

	/// <summary>
	/// Basic progress bar widget
	/// </summary>
	public class ProgressBarWidget : Widget
	{
		protected TextAreaOverlayElement textAreaElement;
		protected TextAreaOverlayElement commentTextAreaElement;
		protected OverlayElement meterElement;
		protected OverlayElement fillElement;
		protected float progress = 0f;

		public ProgressBarWidget(string name, string caption, float width, float commentBoxWidth)
		{
			progress = 0.0f;
			element = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/ProgressBar", "BorderPanel", name);
			element.Width = (width);
			Mogre.OverlayContainer c = (Mogre.OverlayContainer)element;
			textAreaElement = (Mogre.TextAreaOverlayElement)c.GetChild(Name + "/ProgressCaption");
			Mogre.OverlayContainer commentBox = (Mogre.OverlayContainer)c.GetChild(Name + "/ProgressCommentBox");
			commentBox.Width = (commentBoxWidth);
			commentBox.Left = (-(commentBoxWidth + 5f));
			commentTextAreaElement = (Mogre.TextAreaOverlayElement)commentBox.GetChild(commentBox.Name + "/ProgressCommentText");
			meterElement = c.GetChild(Name + "/ProgressMeter");
			meterElement.Width = (width - 10f);
			fillElement = ((Mogre.OverlayContainer)meterElement).GetChild(meterElement.Name + "/ProgressFill");
			setCaption(caption);
		}

		/// <summary>
		/// Sets the progress as a percentage.
		/// </summary>
		/// <param name="progress"></param>
		public void setProgress(float progress)
		{
			this.progress = SdkTrayMathHelper.clamp<float>(progress, 0f, 1f);
			fillElement.Width = (System.Math.Max((int)fillElement.Height, (int)(this.progress * (meterElement.Width - 2 * fillElement.Left))));
		}

		/// <summary>
		/// Gets the progress as a percentage.
		/// </summary>
		/// <returns></returns>
		public float getProgress()
		{
			return progress;
		}

		public string getCaption()
		{
			return textAreaElement.Caption;
		}

		public void setCaption(string caption)
		{
			textAreaElement.Caption = (caption);
		}

		public string getComment()
		{
			return commentTextAreaElement.Caption;
		}

		public void setComment(string comment)
		{
			commentTextAreaElement.Caption = (comment);
		}
	}
}
