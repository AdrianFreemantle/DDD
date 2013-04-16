using System;
using System.Linq;
using Domain.Core.Infrastructure;

namespace Infrastructure
{
    public static class LookupTables
    {
        public static void Register<TLookup, TEnum>(IRepository repository)
            where TLookup : class, ILookupTable
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            foreach (var enumValue in enumValues)
            {
                var wrapper = ActivatorHelper.CreateInstance<TLookup>();
                wrapper.Description = ((Enum)(dynamic)enumValue).GetDescription();
                wrapper.Id = ((int)(dynamic)enumValue);
                AddOrUpdate(wrapper, repository);
            }
        }

        private static void AddOrUpdate<TLookup>(TLookup wrapper, IRepository repository)
            where TLookup : class, ILookupTable
        {
            try
            {
                var savedLookup = repository.Get<TLookup>(wrapper.Id);
                savedLookup.Description = wrapper.Description;
            }
            catch (Exception) 
            {
                repository.Add(wrapper); //if the repository did not contain the lookup we add it.
            }
        }
    }
}