using Microsoft.Practices.Unity;
using System;

namespace Checkout.Infrastructure.Extensions
{
    public static class ContainerRegistrationExtensions
    {
        public static string GetMappingAsString(this ContainerRegistration registration)
        {
            string regName, regType, mapTo, lifetime;

            var r = registration.RegisteredType;
            regType = r.Name + GetGenericArgumentsList(r);

            var m = registration.MappedToType;
            mapTo = m.Name + GetGenericArgumentsList(m);

            regName = registration.Name ?? "[default]";

            lifetime = registration.LifetimeManagerType.Name;

            if (mapTo != regType)
                mapTo = $" -> {mapTo}";
            else
                mapTo = string.Empty;

            lifetime = lifetime.Substring(0, lifetime.Length - "LifeTimeManager".Length);

            return $"+ {regType}{mapTo} '{regName}' {lifetime}";
        }

        private static string GetGenericArgumentsList(Type type)
        {
            if (type.GetGenericArguments().Length == 0)
                return string.Empty;

            string argList = string.Empty;

            bool first = true;

            foreach (Type t in type.GetGenericArguments())
            {
                argList += first ? t.Name : $",{t.Name}";

                first = false;

                if (t.GetGenericArguments().Length > 0)
                    argList += GetGenericArgumentsList(t);
            }

            return $"<{argList}>";
        }
    }
}
