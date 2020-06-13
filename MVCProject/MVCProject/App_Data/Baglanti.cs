using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;

/// <summary>
/// Summary description for BaglantiBilgileri
/// </summary>
public class BaglantiBilgileri
{
    private SqlConnection _Baglanti;

    public SqlConnection Baglanti
    {
        get { return _Baglanti; }
    }

    public BaglantiBilgileri()
    {
        this._Baglanti = new SqlConnection("Data Source=DESKTOP-2UGGNUM;Initial Catalog=StajProje;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
       
    }
}