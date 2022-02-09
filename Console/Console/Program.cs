using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string urlEndpoint = "http://webservices2.twwwireless.com.br/reluzcap/wsreluzcap.asmx/EnviaSMS";
            var serv = new Serv.ReluzCapWebServiceSoapClient(new BasicHttpBinding(BasicHttpSecurityMode.None),
                        new EndpointAddress(urlEndpoint));
            serv.Endpoint.IsSystemEndpoint = true;
            var utf8 = Encoding.UTF8.GetBytes("Teste");
            var mensagem = Encoding.Convert(Encoding.UTF8, Encoding.ASCII, utf8);
            var str = Encoding.ASCII.GetString(mensagem);
            serv.EnviaSMS("CAMED", "Camed@2020", "1", "5566992373203", str);
        }
    }
}
