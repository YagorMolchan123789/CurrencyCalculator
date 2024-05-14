using CurrencyCalculator.Infrastructure.Binders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CurrencyCalculator.Models.ViewModels
{
    [ModelBinder(BinderType = typeof(CalculatorViewModelBinder))]
    public class CalculatorViewModel
    {
        [Display(Name = "Amount")]
        [RegularExpression("^\\d+(?:[,.]\\d+)?$")]
        public decimal Amount { get; set; }

        [Display(Name = "From")]
        public string FromAbbreviation { get; set; }

        public SelectList FromCurrencies { get; set; }

        [Display(Name = "To")]
        public string ToAbbreviation { get; set; }

        public SelectList ToCurrencies { get; set; }

        [Display(Name =  "Rate date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RateDate { get; set; }

        [Display(Name = "Rate")]
        public decimal Rate { get; set; }


        [Display(Name = "Calculated amount")]
        public decimal CalculatedAmount { get { return this.Amount * this.Rate; } }

        public string ErrorMessage { get; set; }

        //    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //    {
        //        List<ValidationResult> errors = new List<ValidationResult>();

        //        if (string.IsNullOrEmpty(this.Amount.ToString()))
        //        {
        //            errors.Add(new ValidationResult("Amount is required!", new List<string> { "Amount" }));
        //        }

        //        return errors;
        //    }
    }

    //public class RateViewModelMetaData
    //{
    //    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public DateTime RateDate { get; set; }
    //}



}
