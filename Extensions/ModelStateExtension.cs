using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace Blog.Extensions
{
    public static class ModelStateExtension
    {
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {
            return (from item in modelState.Values
                    from error in item.Errors
                    select error.ErrorMessage).ToList();
        }
    }
}
