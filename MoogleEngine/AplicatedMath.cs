
// esta es una clase matematica que implementa varios metodos de calculo
namespace MoogleEngine{
    
    public class AplicatedMath{
        
        // calculo de tfidf
        //cantWordsDocuments es la cantidad de veces en los que aparece la palabra dentro del doc
        //cantWords es la cantidad de palabras de del documento
        //countDoc es la cantidad de documentos
        // countDocWord es el numero de docuemntos en los que aparece la palabra
        public double TFIDF(double cantWordsDocuments,double cantWords,double countDoc ,double countDocWord){

            double tf= (cantWordsDocuments/cantWords);// se refiere a la frecuencia de termino 
            
            double idf =(tf==0)?0: Math.Log10(countDoc/(countDocWord-0.5));
            
            return tf*idf;
            
        }
        // multiplicacion de un vector por otro vector mult(A,B)=Σ(Ai*Bi)
        private double VectorMult(Dictionary<string,double>Query,Dictionary<string,double>WordScore){
            
            double mult=0;
            foreach(var s in Query){
                mult+=s.Value*WordScore[s.Key];
            }
            return mult;
        }
        // calculo de la FunCos de dos vectores 
        // responde a la formula s(A,B)= Σ(Ai*Bi)/(√(Σ Ai^2)*√(Σ Bi^2)+1) para dos vectores A y B y sus indices sus elementos
        public double FunCos(Dictionary<string,double>Query,Dictionary<string,double>WordScore){
            double moduloVectorQuery= new double();
            double moduloVectorScore=new double();
            NormVector(Query,ref moduloVectorQuery);
            NormVector(WordScore,ref moduloVectorScore);
            double sim=VectorMult(Query,WordScore )/(Math.Sqrt(moduloVectorQuery*moduloVectorScore)+1);
            return sim;
        }
        
        // normalizacion de un vector numerico es la operacio mediante la cual el vector es convertido a la longitud 1
        // responde a la formula √(Σ Xi^2) con Xi los elementos del vector
        // declaro moduloVector por referencia xq hace falta para calcular la similitud
        public void NormVector(Dictionary<string,double>vector, ref double moduloVector){
            
            double sumSquare=0;
            foreach(var s in vector){
                 sumSquare+= Math.Pow(s.Value,2);
            }
            moduloVector=Math.Sqrt(sumSquare);
            foreach(var s in vector){
                vector[s.Key]=s.Value/moduloVector;
            }
                
        }
        
        // metodo de la sugerencia 
        public string Suggest(List<string> WordsTotal, string word)
    {
        string suggest = word;        // Guarda el resultado de la búsqueda.
        int error = word.Length + 5;  // Guarda la distancia entre la palabra escogida y **suggest**.
        
        int start = Search(WordsTotal, word.Length - 1);  // Marca el inicio del intervalo de la búsqueda.
        int end = Search(WordsTotal, word.Length + 2);    // Marca el final del intervalo de la búsqueda.

        for (int i = start; i < end; i++)
        {
            int dist = LevinshteinDistance(WordsTotal[i], word);  // Guarda la distancia entre **suggest** y la palabra que se está procesando actualmente.

            
            if(dist < error)
            {
                error = dist;
                suggest = WordsTotal[i];
            }
        }

        return suggest;
    }

    // Search es un metodo que ayuda a buscar la  palabra de menor longitud mas grande que x
    // usando el algoritmo de Busqueda Binaria donde con dos punteros i y j 
   
        private int Search(List<string> arr, int x)
    {
        int i = 0, j = arr.Count - 1;   // Ambas variables marcan el inicio y el fin del intervalo en los que se realiza la búsqueda.

        while(i < j)
        {
            int mi = (i + j) / 2;

            if(arr[mi].Length >= x)
                j = mi;
            else
                i = mi + 1;
        }

        return j;
    }

    // metodo conocido como la Distancia de Levenstein recursivo
    // Esta distancia se calcula contando el número mínimo de operaciones necesarias para transformar una cadena en otra.
    // Las operaciones permitidas son la inserción, eliminación o sustitución de un carácter.
    
    private int LevinshteinDistance(string s1, string s2)
    {
        // se toma dos palabras y sus respectivas distancias 
        static int dist(string s1, int m, string s2,int n){
            // si alguna string llega a ser 0 pues se agregan la cantidad de caracteres 
            // que contiene la otra
            if(m==0 || n==0){return (m==0)?n:m;}
            
            // variable de comparacion que visualiza si los caracteres en las posiciones m y n son
            // el mismo o distinto insercion
            int total=(s1[m-1]==s2[n-1])?0:1;

            // dist(s1,m1-1,s2,n) es cuando hacemos la eliminacion de un caracter de s1 
            // dist(s1,m,s2,n-1) es cuando hacemos la elminacin de un caracter de s2
            //dist(s1,m-1,s2,n-1) es cuando hacemos la sustitucion de caracteres de ambas palabras
            return Math.Min(Math.Min(dist(s1,m-1,s2,n)+1,dist(s1,m,s2,n-1)+1),dist(s1,m-1,s2,n-1)+total);
        }

        return dist(s1,s1.Length,s2,s2.Length);
    }
    }
}