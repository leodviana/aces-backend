using System;
using System.Globalization;
using System.Web.Mvc;

namespace GtecIt.Binder
{
    public class DateTimeBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            var date = value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);

            return date;
        }
    }
    public class NullableDateTimeBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            
            if (value != null)
            {
                try
                {
                    var date = value.ConvertTo(typeof (DateTime), CultureInfo.CurrentCulture);

                    return date;
                }
                catch (InvalidOperationException ex)
                {
                    var date = value.AttemptedValue.Substring(0, 10).Split(new char[]{'/'});

                    return new DateTime(int.Parse(date[2]), int.Parse(date[0]), int.Parse(date[1]));
                }
                catch (Exception e)
                {
                    throw new Exception();
                }
                
            }

            return null;
        }
    }
}