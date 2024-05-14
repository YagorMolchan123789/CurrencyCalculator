using CurrencyCalculator.Extensions;
using CurrencyCalculator.Models.Entities;
using CurrencyCalculator.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Xml;

namespace CurrencyCalculator.Infrastructure.Binders
{
    public class CalculatorViewModelBinder : IModelBinder
    {
        private readonly ILogger<CalculatorViewModelBinder> _logger;
        private readonly IConfiguration _config;

        public CalculatorViewModelBinder(ILogger<CalculatorViewModelBinder> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProvider = bindingContext.ValueProvider;

            decimal amount = 0;
            string errorMessage = null;

            try
            {
                amount = (valueProvider.GetValue("Amount") != ValueProviderResult.None) ?
                    decimal.Parse(valueProvider.GetValue("Amount").FirstValue.Replace('.', ',')) : 0;
            }
            catch(FormatException ex)
            {
                errorMessage = _config.GetSection("Errors:Message").Value;
                _logger.LogError(string.Format("{0}. Value = '{1}'", ex.Message, valueProvider.GetValue("Amount").FirstValue));
            }
            
            string fromAbbreviation = (valueProvider.GetValue("FromAbbreviation")!=ValueProviderResult.None) ? 
                valueProvider.GetValue("FromAbbreviation").FirstValue : "EUR";

            string toAbbreviation = (valueProvider.GetValue("ToAbbreviation") != ValueProviderResult.None) ?
                valueProvider.GetValue("ToAbbreviation").FirstValue : "EUR";

            DateTime rateDate = (valueProvider.GetValue("RateDate") != ValueProviderResult.None) ? 
                DateTime.Parse(valueProvider.GetValue("RateDate").FirstValue) : DateTime.Now;


            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(_config.GetSection("Currency:List").Value);

            List<Currency> currencies = xmlDocument.GetCurrencyList();
            xmlDocument.Load(_config.GetSection("Currency:FxRate").Value + rateDate.ToString("yyyy-MM-dd"));

            List<string> childCurrencies = xmlDocument.GetChildCurrencies();

            currencies = currencies.Join(childCurrencies,
                    curr => curr.Abbreviation,
                    child => child,
                    (curr, child) => new Currency { Abbreviation = child, Name = curr.Name }
                ).ToList();            


            var fromRate = (fromAbbreviation == "EUR") ? 1 : xmlDocument.GetFxRate(fromAbbreviation);
            var toRate = (toAbbreviation == "EUR") ? 1 : xmlDocument.GetFxRate(toAbbreviation);
            var rate = toRate / fromRate;

            CalculatorViewModel model = new CalculatorViewModel
            {
                Amount = amount,
                RateDate = rateDate,
                Rate = rate,
                FromAbbreviation = fromAbbreviation,
                FromCurrencies = new SelectList(currencies, "Abbreviation", "Name"),
                ToAbbreviation = toAbbreviation,
                ToCurrencies = new SelectList(currencies, "Abbreviation", "Name"),
                ErrorMessage = errorMessage
            };

            _logger.LogInformation(string.Format("Rate date = {0} From = {1} To = {2} Amount = {3} Calculated amount = {4} Rate = {5}",
                model.RateDate.ToString("yyyy-MM-dd"), model.FromAbbreviation, model.ToAbbreviation, Math.Round(model.Amount,2), Math.Round(model.CalculatedAmount,2),
                Math.Round(model.Rate,2)));

            bindingContext.Result = ModelBindingResult.Success(model);

            return Task.CompletedTask;
        }
    }
}
