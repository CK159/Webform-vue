using System;
using System.Web.UI.WebControls;

namespace WebformVue
{
    public partial class VuePage : System.Web.UI.Page
    {
        protected Label Label1;
        protected Button Button1;
	    protected PlaceHolder ControlContainer;
	    protected Label WarningLabel;
	    
	    protected override void OnInit(EventArgs e)
	    {
		    string name = null;

		    if (Request.QueryString["c"].ToLowerInvariant() == "control")
		    {
			    name = "VueControl.ascx";
		    }
		    else if (Request.QueryString["c"].ToLowerInvariant() == "pd")
		    {
			    name = "VuePreviewDetail.ascx";
		    }

		    if (name == null)
		    {
			    WarningLabel.Visible = true;
		    }
		    else
		    {
			    ControlContainer.Controls.Add(LoadControl("../Controls/" + name));
		    }
	    }
        
        protected void Button1_Click(object sender, EventArgs e)
        {
            Label1.Text = "Clicked at " + DateTime.Now.ToString();
        }
    }
}