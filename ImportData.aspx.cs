using ImportDataFromExternalSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SitefinityWebApp
{
    public partial class ImportData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var requestBaseUrl = this.Request.Url.GetComponents(UriComponents.Scheme | UriComponents.HostAndPort, UriFormat.Unescaped) + "/api/default/";
            new DataImporter().Import(requestBaseUrl);
        }
    }
}