
using System;
using System.Text.RegularExpressions;

namespace MoogleEngine{
    public class Normalize{

        string charInvalid=" @!*^#$_&-+()/¿?¡;:',.~`|•√π÷×¶∆}{=°¢$€£%©®™℅[]><\"\\\n\t\r";
        // una cadena de char invalidos que deben ser removidos

        Dictionary<char,string> charSimilar= new Dictionary<char, string>();
        // a cada char se le asigna una cadena de char que deben ser reemplazados

        //1. este metodo private se usa para reemplazar caracteres de una cadena por elementos alfanumericos
        private string Replaced(string texto){
            charSimilar['a'] = @"áàäâæāªãåą";
            charSimilar['e'] = @"ėêęēèéë";
            charSimilar['i'] = @"ìïíįîī";
            charSimilar['o'] = @"ºōœøõôöòó";
            charSimilar['u'] = @"ūùûüú";
            charSimilar['c'] = @"ćçč";
            charSimilar['n'] = @"ńñ";

            charSimilar['$'] = @"ß€$¢£₹₱¥";
            charSimilar[' '] = @"†‡★—_–·";
            charSimilar['\"'] = @"„“”«»‚‘’‹›";
        

            string normalized= texto.ToLower();
            foreach(char s in charSimilar.Keys){
                foreach(char k in charSimilar[s]){
                    normalized= normalized.Replace(k,s);
                }
            }
            return normalized;
        } 

        // este metodo devuelve una array de string con los elementos de charInvalid removidos
        public string[] Filter(string texto){
            
            texto=Replaced(texto);

            return texto.Split(charInvalid.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            // tomara cada palabra del string que se encuentre separada por alguno de esos signos
        }

        // Expresión regular que solo admite caracteres no alfanumericos y los remplaza por espacios
        public string DeleteInvalidCharacter(string texto){
    
            texto = Replaced(texto);
            string expression = @"[^\w0-9]";  

            return Regex.Replace(texto, expression, " ");
        }


        // este metodo es llamado solamente para modificar el nombre de los archivos txt,
        public string NormalizateResultsTitles(string query){
            query = query.Replace('_', ' ');
            query = query.Replace(".txt", "");
            query = query.ToUpper();

            return query;
    }
    }
}