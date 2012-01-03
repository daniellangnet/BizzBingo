using System;
using System.Web.Mvc;

namespace BizzBingo.Web.Infrastructure.Binder
{
    public class ModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            var genericType = typeof(IModelBinder<>).MakeGenericType(modelType);
            return (IModelBinder)DependencyResolver.Current.GetService(genericType);
        }
    }
}