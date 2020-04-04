using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace GtecIt.Util
{
    public abstract class IAuditoria
    {
        public IDictionary<String, String> Compare(Object voModified, String userName)
        {
            try
            {
                if (!(GetType() == voModified.GetType())) 
                    return null;

                var att_01 = SetRules(this);
                var att_02 = SetRules(voModified);

                IDictionary<String, String> _return = new Dictionary<string, string>();
                _return.Add("UsuarioDataHora", userName + " às " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                foreach (var element in att_01.Join
                                  (
                                      att_02,
                                      att01 => att01.Name,
                                      att02 => att02.Name,
                                      (att01, att02) => new
                                      {
                                          Att01 = att01,
                                          Att02 = att02
                                      }
                                  )
                             .Where(x => Convert.ToString(x.Att01.GetValue(this, null)) != Convert.ToString(x.Att02.GetValue(voModified, null)))
                             .ToDictionary(a => a.Att01.Name, x => string.Format(CultureInfo.InvariantCulture,
                                 "{0} de: '{1}' para: '{2}'",
                                 (x.Att01.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault() as DisplayNameAttribute).DisplayName,
                                 x.Att01.GetValue(this, null),
                                 x.Att02.GetValue(voModified, null))))
                {
                    _return.Add(element);
                }

                return _return;

            }
            catch
            {
                return null;
            }
        }

        private static IEnumerable<PropertyInfo> SetRules(object vo)
        {
            return vo.GetType().GetProperties().AsParallel()
                                           .Where(x => (x.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault() as DisplayNameAttribute) != null) // Ignora os elementos sem atributos
                                           .Where(x => (x.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault() as DisplayNameAttribute).DisplayName != "Ignore") // Ignorar os atributos com displayName = ignore
                                           .Select(x => x)
                                           .ToList(); ;
        }
    }
}

