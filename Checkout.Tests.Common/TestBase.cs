using Checkout.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Checkout.Tests.Common
{
    public abstract class TestBase
    {
        protected void AssertProperties(PropertyInfo ap, PropertyInfo ep, object apv, object epv, bool enableNullSubstitutionRules = false)
        {
            if (enableNullSubstitutionRules && (apv == null && (ap.PropertyType.Equals(typeof(bool?)) && ep.PropertyType.Equals(typeof(bool)))))
            {
                Assert.AreEqual(
                epv,
                default(bool),
                message: string.Format(
                            "Actual Property {0} Value: {1} - Expected Property {2} Value: {3}",
                            ap.Name,
                            default(bool),
                            ep.Name,
                            epv));
            }
            else if (enableNullSubstitutionRules && (apv == null && (ap.PropertyType.Equals(typeof(DateTime?)) && ep.PropertyType.Equals(typeof(DateTime)))))
            {
                Assert.AreEqual(
                epv,
                default(DateTime),
                message: string.Format(
                            "Actual Property {0} Value: {1} - Expected Property {2} Value: {3}",
                            ap.Name,
                            default(DateTime),
                            ep.Name,
                            epv));
            }
            else if (
                        enableNullSubstitutionRules &&
                        (apv == null &&
                        (ap.PropertyType.Equals(typeof(int?)) && ep.PropertyType.Equals(typeof(int))) ||
                        (ap.PropertyType.Equals(typeof(decimal?)) && ep.PropertyType.Equals(typeof(decimal))) ||
                        (ap.PropertyType.Equals(typeof(long?)) && ep.PropertyType.Equals(typeof(long))))
                    )
            {
                Assert.AreEqual(
                epv,
                0,
                message: string.Format(
                            "Actual Property {0} Value: {1} - Expected Property {2} Value: {3}",
                            ap.Name,
                            0,
                            ep.Name,
                            epv));
            }
            else
            {
                Assert.AreEqual(
                    epv,
                    apv,
                    message: string.Format(
                                "Actual Property {0} Value: {1} - Expected Property {2} Value: {3}",
                                ap.Name,
                                apv,
                                ep.Name,
                                epv));
            }
        }

        /// <summary>
        /// Metodo usado na comparação de propriedades de objetos complexos dentro dos testes unitários
        /// </summary>
        /// <param name="actual">Objeto base</param>
        /// <param name="expected">Objeto à ser validado</param>
        /// <param name="enableNullSubstitutionRules">Habilitar regras de substituição de nulos padrão do framework de mapeamento</param>
        public void CompareProperties(object actual, object expected, bool enableNullSubstitutionRules = false)
        {
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.Flush();

            Debug.Indent();

            var bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            foreach (var propActual in actual.GetType().GetProperties(bindingFlags).ToList())
            {
                var propExp = expected.GetType().GetProperty(propActual.Name, bindingFlags);

                Debug.WriteLineIf(propExp == null, string.Format("Property -> {0} doesn't exist on expected object.", propActual.Name));

                if (propExp != null)
                {
                    var propActualValue = propActual.GetValue(actual);
                    var propExpValue = propExp.GetValue(expected);

                    if (!propActual.PropertyType.IsComplex())
                    {
                        this.AssertProperties(propActual, propExp, propActualValue, propExpValue, enableNullSubstitutionRules);
                    }
                    else
                    {
                        if (propActualValue != null)
                        {
                            this.CompareProperties(propActualValue, propExpValue, enableNullSubstitutionRules);
                        }
                        else
                        {
                            this.AssertProperties(propActual, propExp, propActualValue, propExpValue, enableNullSubstitutionRules);
                        }
                    }
                }
            }

            Debug.Unindent();
        }
    }
}
