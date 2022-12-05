using PX.Data;

namespace HH_APICustomization.Utils
{
    public class GraphUtil
    {
        public static void SetRequired<Field>(PXCache cache, object item, bool required) where Field : IBqlField
        {
            PXPersistingCheck type = required ? PXPersistingCheck.NullOrBlank : PXPersistingCheck.Nothing;
            PXUIFieldAttribute.SetRequired<Field>(cache, required);
            PXDefaultAttribute.SetPersistingCheck<Field>(cache, item, type);
        }

        public static void SetError<Field>(PXCache cache, object row, object newValue, string errorMsg, PXErrorLevel errorLevel = PXErrorLevel.Error) where Field : PX.Data.IBqlField
        {
            cache.RaiseExceptionHandling<Field>(row, newValue,
                  new PXSetPropertyException(errorMsg, errorLevel));
        }
    }
}
