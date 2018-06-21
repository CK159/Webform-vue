using System;
using System.Web.UI.WebControls;

namespace WebformVue
{
    public partial class VuePage : System.Web.UI.Page
    {
        protected Label Label1;
        protected Button Button1;
        
        protected void Button1_Click(object sender, EventArgs e)
        {
            Label1.Text = "Clicked at " + DateTime.Now.ToString();
        }
    }
}