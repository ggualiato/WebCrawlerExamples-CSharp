using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrawlerExemple.Models;

using System.Net;

using HtmlAgilityPack;
using System.Collections.Specialized;

namespace CrawlerExemple
{
    class Program
    {
        static void Main(string[] args)
        {
            FinanceOne();
            //Cinema();
            //VivaReal();
            //Google();

        }

        private static void FinanceOne()
        {
            string url = "https://financeone.com.br/moedas/cotacoes-do-dolar";

            using (var client = new WebClient())
            {
                var values = new NameValueCollection();

                values.Add("dia", "23");
                values.Add("mes", "03");
                values.Add("ano", "2018");
                values.Add("cotacao", "data");

                if (client.Proxy != null)
                {
                    client.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
                }

                var response = client.UploadValues(url, values);
                var res = Encoding.Default.GetString(response);
                var doc = new HtmlDocument();
                doc.LoadHtml(res);
                var docNode = doc.DocumentNode;

                var node = docNode.SelectSingleNode("//div[@id = 'box-cotacoes']").Descendants("td").ToList();

                var data = node[0].InnerText.Trim();
                var compra = node[1].InnerText.Trim();
                var venda = node[2].InnerText.Trim();

                //var dados = node.Descendants("td").ToList();

                Console.WriteLine("Data: {0} \nCompra: {1} \nVenda: {2} \n", data, compra, venda);

                Console.ReadLine();
            }
        }

        private static void Cinema()
        {
            Filmes filme = new Filmes();
            List<Filmes> filmes = new List<Filmes>();

            string url = "http://www.caixabelasartes.com.br/programacao-regular/";
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(url);
            var docNode = doc.DocumentNode;
            var node = docNode.SelectNodes("//div[@class = 'c-movie-card-grid ']");

            foreach (var nNode in node)
            {
                var infos = nNode.SelectSingleNode("//p[@class = 'c-movie-card__info-comp']").InnerText.ToString().Split('|');

                filme.Titulo = nNode.Descendants("h3").FirstOrDefault().InnerText;
                filme.Sinopse = nNode.SelectSingleNode("//p[@class = 'c-movie-card__info-synopsis']").InnerText;
                filme.Genero = infos[0];
                filme.Diretor = infos[4];

                filmes.Add(filme);

                Console.WriteLine("Titulo: {0} \nSinopse: {1} \nGenero: {2} \nDiretor: {3} \n\n", filme.Titulo.ToUpper(), filme.Sinopse, filme.Genero, filme.Diretor);
            }
            Console.ReadLine();

        }

        private static void VivaReal()
        {
            List<Imovel> imoveis = new List<Imovel>();
            Imovel imovel = new Imovel();

            string classTitulo = "property-card__title js-cardLink js-card-title";
            string classPreco = "property-card__price js-property-card-prices js-property-card__price-small";

            var url = "http://www.vivareal.com.br/venda/sp/sao-paulo/zona-sul/morumbi/apartamento_residencial/";
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(url);

            var docNode = doc.DocumentNode;

            var node = docNode.SelectNodes("//div[@class = 'js-card-selector']");

            foreach (var nNode in node)
            {
                //imovel.Titulo = nNode.SelectSingleNode("//a[@class = 'property-card__title js-cardLink js-card-title']").InnerText.Replace("\n", "").Trim();
                //imovel.Titulo = nNode.SelectSingleNode("//article/h2/a").InnerText.Replace("\n", "").Trim(); 
                //imovel.Titulo = nNode.Descendants("a").Single(n => n.Attributes["class"].Value == classTitulo).InnerText.Replace("\n", "").Trim();
                imovel.Titulo = nNode.Descendants("a").Where(n => n.Attributes["class"].Value == classTitulo).Single().InnerText.Replace("\n", "").Trim();
                imovel.Endereco = nNode.Descendants("span").FirstOrDefault().InnerText.Replace("\n", "").Trim();
                imovel.Preco = nNode.Descendants("div").Where(n => n.Attributes["class"].Value == classPreco).FirstOrDefault().InnerText.Replace("\n", "").Trim();

                imoveis.Add(imovel);

                Console.WriteLine("Titulo: {0} \nEndereço: {1} \nPreço: {2} \n\n", imovel.Titulo, imovel.Endereco, imovel.Preco);
            }

            Console.ReadLine();
        }

        private static void Google()
        {
            string url = "http://www.google.com.br/search?q=resource+it";
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(url);

            var resultado = doc.GetElementbyId("resultStats").InnerText;

            var docNode = doc.DocumentNode;

            var node = docNode.SelectNodes("//div[@id = 'search']");

            foreach (var nNode in node.Descendants("a"))
            {
                Console.WriteLine("\n" + nNode.GetAttributeValue("href", ""));
            }

            Console.WriteLine(resultado);
            Console.ReadLine();
        }
    }
}
