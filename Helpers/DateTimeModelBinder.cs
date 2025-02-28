using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MercerStore.Helpers
{
    public class DateTimeModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ArgumentNullException.ThrowIfNull(bindingContext);

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrWhiteSpace(value))
            {
              
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            if (DateTime.TryParse(value, out var parsedDate))
            {
                parsedDate = DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
                bindingContext.Result = ModelBindingResult.Success(parsedDate);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Неверный формат даты.");
            }

            return Task.CompletedTask;
        }
    }
}