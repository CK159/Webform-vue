using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace WebformVue
{
    public partial class VuePage : System.Web.UI.Page
    {
        protected Label Label1;
        protected Button Button1;
	    protected PlaceHolder ControlContainer;
	    protected Label WarningLabel;
	    
	    private Dictionary<string, string> pages = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
	    {
		    {"control", "VueControl.ascx"},
		    {"pd", "VuePreviewDetail.ascx"},
		    {"select", "VueAsyncDropdown.ascx"}
	    };
	    
	    protected override void OnInit(EventArgs e)
	    {
		    string pk = Request.QueryString["c"];
		    
		    if (pages.ContainsKey(pk))
			    ControlContainer.Controls.Add(LoadControl("../Controls/" + pages[pk]));
		    else
			    WarningLabel.Visible = true;
	    }
        
        protected void Button1_Click(object sender, EventArgs e)
        {
            Label1.Text = "Clicked at " + DateTime.Now.ToString();
        }
    }
}