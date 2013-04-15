using System;
using System.Linq;
using Domain.Core.Infrastructure;

namespace PersistenceModel.Reporting
{
    public static class LookupTables
    {
        public static void Initialize<TEnum>(IRepository repository) where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            foreach (TEnum value in enumValues)
            {
                var lookup = new AccountStatusLookup
                {
                    Id = (int)(dynamic)value,
                    Description = ((Enum)(dynamic)value).GetDescription()
                };

                repository.Add(lookup);
            }
        }
    }
}