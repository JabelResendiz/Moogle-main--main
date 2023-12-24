
namespace MoogleEngine{

    public class Principal{
        private CreateVocabulary CreateVocabulary = new CreateVocabulary();
        private QueryWork QueryWork = new QueryWork();
        private AplicatedMath AplicatedMath = new AplicatedMath();
        private Normalize Normalizate = new Normalize();
        

        Dictionary<string, double> WordsDocument;// cantidad de doc en los que aparece la palabra
        HashSet<string> hashglobal;                     // Contiene todas las palabras que aparecen en todos los documentos (sin repetir)
        List<string>bagOfWords;                             // [Facil y rapida verificacion de aparicion o no de la palabra]
        List<Files> Document;  // los objetos de documentos 

       // constructor de la clase 
        public Principal(){
            WordsDocument = new Dictionary<string, double>();
            hashglobal = new HashSet<string>();
            bagOfWords= new List<string>();
            Document = this.CreateVocabulary.VectorFiles(ref WordsDocument , ref hashglobal,ref bagOfWords);
            CalculateTFIDF(ref Document);
            bagOfWords.Sort(OrderLength);
        }

        //////// metodo encargado de darle un valor tf*idf a cada palabra de los documents
        private void CalculateTFIDF(ref List<Files>Document){
            
            for(int i=0;i<Document.Count;i++){
                
                foreach (string word in Document[i].repeat.Keys){
                    double value = this.AplicatedMath.TFIDF(Document[i].repeat[word], Document[i].Length, Document.Count, WordsDocument[word]);

                    Document[i].WordScore.Add(word, value);
                
                }
                double s= new double();
                AplicatedMath.NormVector(Document[i].WordScore,ref s);// ya los vectores relacionados estan normalizados
                
            }
        }



        // metodo para asignarle a crear los objetos SearchItem y guardarlos en un array organizados por su score
         public SearchItem[] Process(string text){
        // Crea el vector correspondiente a la query almacenando el valor del TF-IDF de las palabras que contiene.
            Dictionary<string, double> Query = this.QueryWork.VectorQuery(text, Document, hashglobal, WordsDocument);
            
            if(Query.Count == 0){return new SearchItem[0];}
                

            // Verifica si se usó algun operador y realiza las acciones correspondientes.
            double[] IncrementScoreDocument  = this.QueryWork.Operators( text, ref Query, ref hashglobal,Document.Count ,ref Document);
            
            CalculateVectorialModel(Query, IncrementScoreDocument);

            List<SearchItem> query = CallSearch(Query.Keys.ToArray());
            
            query.Sort(OrderScore);

            return query.ToArray();
        }
        // metodo de ordenacion de dos objetos segun su campo score
         int OrderScore(SearchItem a,SearchItem b){
            return (a.Score>=b.Score)?-1:1;
        }

// ordenar las palabras de bagOfWords por su tamano
        int OrderLength(string a, string b){
        if(a.Length < b.Length)
            return -1;
        return 1;
    }
        // este metodo analizara el proceso de calculo vectorial
        //VectorQuery es el vector del query 
        //IncrementScoreDocument es el valor que ya se le va asignando a cada docuemnto entre 0 y 1 de acuerdo al operador cercania
        //Prioridades para un documento 
        /*
        1.Presencia del operador ! que advierte de que el docuemnto con esa palabra hay que desecharlo
        2.Presencia del operador ^ que advierte que el documento que no la tengo hay que desecharlo
        3.Presencia de la mayor cantidad de palabras exactas con el query
        4.Casi al mismo tiempo la presencia del operador de * dando tanta relevancia a la palabra como asteriscos tenga
        */
        /*
        1.EL VectorQuery despues de normalizado se le agregaron valores a sus palabras debido a la presencia de operadores
        2.La idea es hallar la similitud del vectorquery y el de cada documento y al final darle un incremento al score del documento
        */
        private void CalculateVectorialModel(Dictionary<string, double> VectorQuery, double[] IncrementScoreDocument){
            for(int i = 0; i < Document.Count; i++){
                double sim = 0, sumScore = 0, sumScoreQuery = 0;
                int words_founds = 0;   // Cantidad de palabras de la query que aparecen en el documento actual.
                
                foreach (var query in VectorQuery){// para cada palabra del query

                    double value = query.Value;
                    double priority = 1;
                    if(!Document[i].WordScore.ContainsKey(query.Key)&& value>100){
                        // si la palabra no esta en el vocabulario del doc actual y value>100 (presencia de ! o *)

                        if(value>1000){continue;}// si la palabra presenta asteriscos saltar a la otra palabra
                        words_founds=0;break;// si la palabra tiene ^ exige que debe aparecer puesto que no esta en el doc actual desecharlo
                    }
                    if(!Document[i].WordScore.ContainsKey(query.Key)){// si la palabra no aparece en el docs actual pero su valor es menor o igual a 100 ( presencia de cercania o de *)
                        // en realidad no importa pues si la palabra no esta y no exige presencia pues salir a buscar la otra palabra
                        continue;// saltar a la otra palabra
                    }
                    
                    /// la palabra del query esta en el documento actual 

                    /// Operator [!]
                    if(value == -1){// si el valor que le corresponde es -1 es xq la palabra no puede aparecer 
                        
                        words_founds = 0;//si la palabra aparece en el vocabulario entonces evitar ese documento
                            // words not foudn guardara todas las palabras del vocbualrio query
                        break;// saltar al proximo docuemnto
                       
                    }
                    words_founds++;

                    /// Operator [*]
                    if(value > 1000){// operador prioridad 
                        priority = (int)(value / 1000);
                        value -= (1000 * priority);
                        value *= priority;// es multiplicar la cantidad de * astericos por el valor normalizado 
                        value=(value>100)?99:value;
                        // esto esta pensado para evitar confusion con la linea de abajo(aproxiamdamente a partir de 250 * son ignorados)
                    }

                /// Operator [^]
                    if(value > 100 ){// debe devolverse este doc 
                      // se le devuelve la palabra que tenia
                        value -= 100;
                    }
                    
                    // sim sera la suma de los valores de cada palabra 
                    sim += (Document[i].WordScore[query.Key] * value);
                    sumScore += (Document[i].WordScore[query.Key] * Document[i].WordScore[query.Key]);
                    sumScoreQuery += (value * value);
                }

                // Si el documento no contiene ninguna palabra de la query lo marco para no retornarlo en la respuesta.
                if(words_founds == 0){
                    Document[i].Score = -1;
                }
                else{
                    double a1 = (double)Math.Sqrt(sumScore);
                    double a2 = (double)Math.Sqrt(sumScoreQuery);
                    //Console.WriteLine(Document[i].Title+"  :"+ sim / (a1 * a2 +1));
                    Document[i].Score = (double)(sim / (a1 * a2 +1)) +words_founds+IncrementScoreDocument[i];
                }
            }
        }      

        // metodo donde se crean los objetos SearchItem guardando el titulo,su score y su snippet pero sin organizar
        private List<SearchItem> CallSearch( string[]query ){
            List<SearchItem> Query = new List<SearchItem>();

            for (int i = 0; i < Document.Count; i++){
            // Si el documento no contiene ninguna palabra de la query no lo agregamos a la respuesta.
                if(Document[i].Score == -1){ continue;}
                   

                Files doc = Document[i]; //doc sera el elemento es el que estoy
                this.QueryWork.Snippet(ref doc, query);

                string snippet =this.QueryWork.FlagSnippet(doc.snippet, query);// marca el snippet 

                Query.Add(new SearchItem(doc.Title, snippet, (float)doc.Score));// crea el objeto SearchItem
            }

            return Query;// return la lista de objetos 
        }


// metodo de la sugerencia para cadenas donde no hay una coincidencia fuerte
        // dentro de los textos de la base de dato
        // Explicacion:
    /*
    1. Busca en el vocabulario de palabras del corpus si la palabra esta: en caso de estarlo pues
    agregar la palabra al texto de la sugerencia. Si no llama al metodo Suggest
    2. En Suggest se encarga de buscar el rango de palabras cuya longitud oscile entre uno hasta 2 caracteres
    3.Esta busqueda la hace a traves del metodo Search con algoritmo de Busqueda Binaria
    4. LevinshteinDistance es el metodo que mide las palabras dentro de ese rango y devuelve la diferencia de
    caracteres entre cada palabra del rango y la actual tomada
    5. Suggest termina viendo la palabra con menor valor de entre estas y guardando para la sugerencia
    */
        public string GetSuggestion(string text)
    {
        List<string> suggest = new List<string>();
        string[] words = this.Normalizate.Filter(text);

        /// Corrige cada palabra en caso de que esté mal escrita.
        foreach (string word in words)
        {
            if(!hashglobal.Contains(word))
                // si no esta en el vocabulario pues llama el metodo sugerencia
                suggest.Add(this.AplicatedMath.Suggest(bagOfWords, word));
            else
                //en otro caso la agrega a la lista sugerencia
                suggest.Add(word);
        }

        string sugestText = String.Join(" ", suggest);

        return sugestText;
    }
    
    }
}