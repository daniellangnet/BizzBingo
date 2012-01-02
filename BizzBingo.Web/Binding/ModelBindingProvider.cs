using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzBingo.Web.Binding
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