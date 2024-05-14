using System.Collections.Generic;
using CurrencyCalculator.Models.Entities;
using System.Xml;

namespace CurrencyCalculator.Extensions
{
    public static class CurrencyExtensions
    {
        public static List<Currency> GetCurrencyList(this XmlDocument xmlDocument)
        {
            List<Currency> currencies = new List<Currency>();
            XmlNodeList currencyChildren = xmlDocument.DocumentElement.ChildNodes;
            
            foreach(XmlNode currencyChild in currencyChildren)
            {
                Currency currency = new Currency
                {
                    Abbreviation = currencyChild.FirstChild.InnerText,
                    Name = currencyChild.LastChild.PreviousSibling.PreviousSibling.InnerText
                };
                currencies.Add(currency);
            }

            return currencies;
        }

        public static List<string> GetChildCurrencies(this XmlDocument xmlDocument)
        {
            List<string> abbreviations = new List<string>();
            XmlNodeList currencyChildren = xmlDocument.DocumentElement.ChildNodes;
            abbreviations.Add("EUR");

            foreach (XmlNode currencyChild in currencyChildren)
            {
                abbreviations.Add(currencyChild.LastChild.FirstChild.InnerText);
            }

            return abbreviations;
        }

        public static decimal GetFxRate(this XmlDocument xmlDocument, string currency)
        {
            decimal rate = 0;
            XmlNodeList currencyChildren = xmlDocument.DocumentElement.ChildNodes;

            foreach(XmlNode currencyChild in currencyChildren)
            {
                if (currencyChild.LastChild.FirstChild.InnerText == currency)
                {
                    rate = decimal.Parse(currencyChild.LastChild.LastChild.InnerText.Replace('.',','));
                    break;
                }
            }

            return rate;
        }
    }
}
