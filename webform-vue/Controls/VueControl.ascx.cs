using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebformVue
{
	public partial class VueControl : UserControl
	{
		protected Label Label1;
		protected Button Button1;

		private int postbacks
		{
			get => (int)(ViewState["postbacks"] ?? 0);
			set => ViewState["postbacks"] = value;
		}

		private void Page_Load(object sender, EventArgs e)
		{
			if (Page.IsPostBack)
			{
				postbacks++;
			}

			if (postbacks > 0)
			{
				Label1.Text = "Postbacks: " + postbacks;
				Label1.Visible = true;
			}
		}
	}
}