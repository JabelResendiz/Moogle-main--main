
using System;
using System.IO;

namespace MoogleEngine{
// clase encargada de trabajar con lo documentos desde su lectura hasta convertirlos en objetos de la clase Files
    public class CreateVocabulary{
        
        private Normalize normalized= new Normalize();
        
        // metodo encargado de cargar los archivos de la carpeta content
        private string[]archivos;
        private static string[]ScanTxt(ref string[]archivos){
           
           
            string rutaActual = Directory.GetCurrentDirectory()+"/content/";
           
            archivos = Directory.GetFiles(rutaActual,"*.txt").Select(Path.GetFileName).ToArray()!;
        
            return archivos;

        }

        /// metodo encargado de la lectura de los archivos
        public string ReadDocument(string title){
    
            return File.ReadAllText( Directory.GetCurrentDirectory()+"/content/"+title);
        }

        // metodo que inspecciona los documentos y trabaja sobre ellos
        // WordsDocument es la cantidad de documentos en los que aparece una palabra del vocabulario
        // hashGlobal es el vocabulario de palabras 
       // por cada documentos voy a necesitar sacar mucha informacion valiosa, vamos a crear la clase File
        public List<Files> VectorFiles(ref Dictionary<string, double > WordsDocument,ref HashSet<string>hashglobal,ref List<string>bagOfWords){

            List<Files>Document= new List<Files>();
            double number=0;

            foreach(string s in ScanTxt(ref archivos) ){
                string plane= ReadDocument(s);// llamando al metodo de lectura
                Dictionary<string,double>WordsCount= new Dictionary<string, double>();//las palabras y  la cantidad de veces que aparece las palabras en un doc
                string[]text=this.normalized.Filter(plane);// llamando al metodo Filter de la clase Normalized
                
                for( int i=0;i<text.Length;i++){
                    
                    if(!WordsCount.ContainsKey(text[i])){
                        WordsCount.Add(text[i],0);
                    }
                    WordsCount[text[i]]++;

                    //si la palabra no esta en mi vocabulario agregarla
                    if(!hashglobal.Contains(text[i])){
                        
                        hashglobal.Add(text[i]);
                        bagOfWords.Add(text[i]);
                    }

                    // si es primera vez que la palabra es mencionada en el doc ver si ya esta en wordsDocument
                    // si no esta agregarla y si esta sumarle uno
                    // si la palabra ya fue mencionada en el documento actual ignorar xq ya existio una primera vez que mencionada
                    if(WordsCount[text[i]]==1){
                        if(!WordsDocument.ContainsKey(text[i])){
                            WordsDocument.Add(text[i],0);
                        }
                        WordsDocument[text[i]]++;
                    }
                }
            
                Files vectorDoc= new Files(number,s,WordsCount,text,plane);// la creacion del objeto que contiene la info de un doc
                Document.Add(vectorDoc);
                number++;
                

            }
            
            return Document;
            
        }

       

        

    }
}
