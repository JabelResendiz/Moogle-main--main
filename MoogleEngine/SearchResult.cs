namespace MoogleEngine;
//El tipo `SearchResult` recibe en su constructor dos argumentos: `items` y `suggestion`. El parámetro `items` es un array de objetos 
// de tipo `SearchItem`. Cada uno de estos 
//objetos representa un posible documento que coincide al menos parcialmente con la consulta en `query`.
public class SearchResult
{
    private CreateVocabulary CreateVocabulary = new CreateVocabulary();
    private SearchItem[] items;
    
    public SearchResult(SearchItem[] items, string suggestion="")
    {
        // 3º paso cuando se ejecuta el boton de buscar
        // este es un constructor de la clasee SearchResult
        // toma como  argumento un arrya de objetos tipo Search Items y un strings
        if (items == null) {
            throw new ArgumentNullException("items");
        }

        this.items = items;
        this.Suggestion = suggestion;
    }

    public SearchResult() : this(new SearchItem[0]) {
        // esto es otro constructor que toma 
         
    }

    public string Suggestion { get; private set; } 
// almacena un string a taves de get  y que solo puede modificar la clase a traves de set 
    
 
    public IEnumerable<SearchItem> Items() {
        return this.items;
        // un metodo que devuelve una lista de resultados de la busqueda que ha sido almacenados en la variable items
        // este metodo es lo que se imprime en la pantalla 
    }
    

    public int Count { get { return this.items.Length; } }
// una propiedad que devuelve el tamaño deel array items
// que como sabemos es un array de objetos de la clase SearchItems    
}
