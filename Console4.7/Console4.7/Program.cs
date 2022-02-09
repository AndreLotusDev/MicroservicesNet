using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Console4._7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string urlEndpoint = "http://webservices2.twwwireless.com.br/reluzcap/wsreluzcap.asmx";

            var httpbasicBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            httpbasicBinding.TextEncoding = Encoding.UTF8;

            var serv = new Servhttps.ReluzCapWebServiceSoapClient(httpbasicBinding, new EndpointAddress(urlEndpoint));

            serv.EnviaSMSAlt("CAMED", "Camed@2020", "1", "5541988664960", "doutor estou dodoi");
            serv.EnviaSMS("CAMED", "Camed@2020", "1", "5541988664960", "doutor estou dodoi");
        }
    }
}
