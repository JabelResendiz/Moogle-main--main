 

namespace MoogleEngine;
// este clase es llamada para imprimir los documentos y el pedazo de texto donde aparece la frase(s)
public class SearchItem
{
    public SearchItem(string title, string snippet, float score)
    {
        Normalize normalizate = new Normalize();

        this.Title = normalizate.NormalizateResultsTitles(title);
        this.Snippet = snippet;
        this.Score = score;
        // constructor de la clase 
        
    }

     
   
    public string Title { get; private set; } 

    public string Snippet { get; private set; }

    public float Score { get; private set; }
}

//Cada `SearchItem` recibe 3 argumentos en su constructor: `title`, `snippet` y `score`. El parámetro `title` debe ser
// el título del documento (el nombre del archivo de texto correspondiente). El parámetro `snippet` debe contener una porción del 
//documento donde se encontró el contenido del `query`. El parámetro `score` tendrá un valor de tipo `float` que será más alto
// mientras más relevante sea este item.