
namespace MoogleEngine{
    public class QueryWork{
        
        // todo lo relacionado con el query

        private CreateVocabulary vocabulary= new CreateVocabulary();
        private Normalize normalize= new Normalize();
        private AplicatedMath AplicatedMath= new AplicatedMath();
      
        // query es el texto
        //Documents es la lista de objetos 
        //hashglobal es el vocabuario
        // WordsDocumetns en la cantidad de docmeuntso donde aparece la palabra
        public  Dictionary<string,double> VectorQuery(string query,List<Files>Document, HashSet<string>hashglobal,Dictionary<string,double>WordsDocuments){
            Dictionary<string,double>Query= new Dictionary<string, double>();
            string []words=this.normalize.Filter(query);
            ProcessQuery(words,hashglobal,ref Query);// la cantidad de veces que aparece una palabra dentro del query

            foreach (var index in Query){
            double scoreQuery = this.AplicatedMath.TFIDF(index.Value, words.Length, Document.Count, WordsDocuments[index.Key]);
            
            Query[index.Key] = scoreQuery;
        }
        
            double ñ= new double();
            AplicatedMath.NormVector(Query,ref ñ);// el vector query normalizado
            
            return Query;
           
        }
    /// Guarda en Query la cantidad de veces que se repiten las palbras en la query siempre que aparezcan en algún documento.
    ///<param name="words">
    /// Palabras que aparecen en la query 
    ///<param name="HashGlobal">
    /// Todas las palabras que aparecen en todos los documentos en CreateVocabulary
    ///<param name="Query">
    /// Vector correspondiente a la query.
    
        private void ProcessQuery(string[] words, HashSet<string> HashGlobal ,ref Dictionary<string, double> VectorQuery){
            for(int i = 0; i < words.Length; i++){
                if(!HashGlobal.Contains(words[i])){continue;}
                    
                if(!VectorQuery.ContainsKey(words[i])){VectorQuery.Add(words[i], 0);}
                    
                VectorQuery[words[i]]++;
            }
        }
        
        //// metodo encargado de encontrar la presencia de operadores 
        //// Algoritmo
        ////1. Asignamos un valor a cada docuemnto de acuerdo a la cercania de dos palabras del query
        ////2.Revisamos la presenica de operadores dentro del query
        ////3. Pero esta vez le asignamos un variamos el valor al VectorQuery
        public double [] Operators(string query, ref Dictionary<string,double>VectorQuery,ref HashSet<string>HashGlobal,int CountFils,ref List<Files>Document){

            string [] words= query.Split(' ',StringSplitOptions.RemoveEmptyEntries);
            
            double[] ScoreAdd= new double[CountFils];// crea un array de double para cada documento solo para el operador de cercania 

            int index_near=0;
            if (Near(words,ref index_near)){
                //existe un operador de cercania en el query luego hay que darle a cada documento un valor 
                for (int i = 0; i < CountFils; i++){
                    // llamada al metodo OperatorNear
                    double value = OperatorNear(Document[i].txtnorm, query, Document[i].WordScore, index_near);

                    if (value != -1)
                        ScoreAdd[i] = value;
                    else
                        ScoreAdd[i] = 0;
                    Console.WriteLine(Document[i].Title+"   :"+ ScoreAdd[i]);
                }
            }
            
            // si no hay ningun operador de cercania no incrementa nada a los documentos 
            foreach(string item in words){
                string[]text=this.normalize.Filter(item);
                if(text.Length==0 || !HashGlobal.Contains(text[0])){continue;}

                VectorQuery[text[0]]+=Mark(item);

                if(item[0]=='!'){
                    VectorQuery[text[0]] = -1;// los dobles rondan valores entre 0 y 1 luego un -1 indica que ese documento nso se debe tomar
                }

                if(item[0]=='^'){
                    VectorQuery[text[0]]+=100;
                }
            }     
            return ScoreAdd;
        }

        // advierte de la presencia de un operador de cercania 
        private bool Near(string[] words,ref int index){
            
            for(int  i=1;i<words.Length-1;i++){
                index++;
                if(words[i]=="~"){
                    return true;
                }
            }
            return false;
        }
             
    //// Realiza el proceso correspondiente al operador de cercnía.
    
    ////name="hashglobal"
    //// Todas las palabras que aparecen en todos los documentos.
  
    ////name="query"
    /// int index_near es la posicon del signo ~
    ////text.norm es el normalizado del documento actual
    /// Algoritmo 
    /*
    Se itera sobre cada palabra del texto del documento y se fijan dos punteros,inicialmente en la posicion 0 , uno de ellos se movera 
    en busca de encontrar otra palabra si la palabra coincide con la que esta en el puntero mas retrasado se mantiene la distancia. Si en cambio la palabra es distinta en cada puntero(por supuesto pero es
    una de las que quiero buscar) pues se calcula esa distancia; si esa distancia es menor que la guardada pues se actualiza sino se mantiene la 
    distancia. Despues de alguna de estas operaciones se mueve el puntero mas atrasado hacia la posicion del puntero mas adelantada.
    */
    private double OperatorNear(string [] txtnorm, string query, Dictionary<string,double>WordsScore,int index_near)
    {
        string [] term=this.normalize.Filter(query);
        if(!WordsScore.ContainsKey(term[index_near-1]) || !WordsScore.ContainsKey(term[index_near])){return -1;}

        // el texto actual contiene las palabras
        Console.WriteLine(String.Join(" ",txtnorm));
        int word1=0;
        int word2=0;
        double dist=double.MaxValue;
        
        int k=0;
        for(int i=0;i<txtnorm.Length;i++){
            if(txtnorm[i]==term[index_near-1] || txtnorm[i]==term[index_near]){
                k++;
                if(k==1){word1=i;word2=i-1;continue;}
                if(txtnorm[i]==txtnorm[Math.Max(word1,word2)]){
                    word2=i;
                }
                else{
                    int dist2= i-Math.Max(word1,word2);
                    if(dist>dist2){
                        dist=dist2;
                    }
            }
        }
        }
        return 0.5/dist;
        
    }

    
        private double Mark(string item){

            if(!item.StartsWith('*')){
                return 0;
            }
            return ((item.LastIndexOf('*')+2)*1000);
        }

         /// el snippet 
        // un objeto  Files y el queery
        // se observa oracion por oracion hasta 
        public void Snippet(ref Files file, string[] query){
            string text = file.text + ".";   // Texto sin procesar del documento.
            int len_max = 0;    // Cantidad de palabras del fragmento que coinciden con las de la query.

            int start = 0;  // Marca el inicio del fragmento.
            int p = text.IndexOf('.', start);   // Marca el final del fragmento desde start
        
            while(p != -1){// es -1 cuando el documento llegue al final
                string fragment = text.Substring(start, p - start);// texto que empieza por start y longitud p-start
                string[] words = this.normalize.Filter(fragment);// se filtra
                int len = 0;

                // se va contar la cantidad de palabras que coinciden con el query
                foreach (string word in words){
                    foreach (string q in query){
                        if(word == q){
                            len++;
                            break;
                        }
                    }
                }
                // se buscara la oracion con la mayor cantidad de palabras que coincidan con el query
                if(len > len_max){
                    len_max = len;
                    file.snippet = fragment;// se le asigna al campo snippet del objeto lo hallado
                }
                //movemos el marcador start una unidad mas que p 
                start = p + 1;
                // si no existen mas 100 palabras a partir del marcador start el marcador p se movera hasta el punto final seguido,buscando la proxima oracion
                // si existen mas de 100 palabras el marcador p se movera hasta el proximo punto final a partir de las 100 palabras
                p=(start + 100 < text.Length)?text.IndexOf('.', 100 + start): text.IndexOf('.', start);
                
            }
        }    

        // al snippet le modificamos agregando algunas cosas para que se imprima entre los emogis
        public string FlagSnippet( string snippet,string[]query){
            
            string snipet= this.normalize.DeleteInvalidCharacter(snippet);
            foreach(string a in query){
                if(snipet.Contains(a)){
                    snipet=snipet.Replace(a," 🏁 "+a+" 🏁 ");
                }
            }
            return snipet;
        }
        
    }
}