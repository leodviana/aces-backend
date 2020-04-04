using System.Text.RegularExpressions;

namespace GtecIt.Util
{
    public static class FiltroDeCaracteres
    {
        public static string RemoverCaracteresEspeciais(string strValue)
        {
            return strValue != null ? Regex.Replace(strValue, @"[^0-9a-zA-Z]+", "") : strValue;
        }
    }
}