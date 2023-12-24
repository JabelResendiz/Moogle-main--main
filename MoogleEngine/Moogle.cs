﻿


namespace MoogleEngine;
// metodo que es llamado cuando el usuario carga el boton buscar
public static class Moogle
{   
    
    
    static Principal Manager = new Principal();
    
    public static SearchResult Query(string query) {

        SearchItem[] items = Manager.Process(query);   // Guarda la información de los documentos correspondientes al resultado la búsqueda.
       string suggest = Manager.GetSuggestion(query);

        return new SearchResult(items, suggest);
    }
    
}



