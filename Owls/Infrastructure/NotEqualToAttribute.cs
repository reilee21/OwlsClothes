using System.ComponentModel.DataAnnotations;

namespace Owls.Infrastructure
{
	public class NotEqualToAttribute : ValidationAttribute
	{
		private readonly string _comparisonValue;

		public NotEqualToAttribute(string comparisonValue)
		{
			_comparisonValue = comparisonValue;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value != null && value.ToString().Equals(_comparisonValue, StringComparison.OrdinalIgnoreCase))
			{
				return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} cannot be '{_comparisonValue}'.");
			}

			return ValidationResult.Success;
		}
	}
}
