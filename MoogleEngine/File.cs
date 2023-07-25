
namespace MoogleEngine;
// clase encargada de manipular la informacion del vector documento

public class Files{
    public double number;// el numero del archivo
    public string Title;// el nombre del documento
    public string text;//el cuerpo del texto
    public string[]txtnorm;//el texto normalizado
    public Dictionary<string,double>repeat;// las palabras y la cantidad de veces que se repite dentro del doc
    
   
    public double Length; // tamaño del texto nomalizado
    public string snippet{get;set;} // el snippet
    public Dictionary<string, double> WordScore; // donde a cada palabra le voy hacer corresponder su tf*idf
    public double Score { get; set; } // su score

// el constructor de la clase tomara 5 argumentos principales
// int number es indice del documento
// string title es el titulo del mismo
// Dictionary que contiene toda la informacion referida a las palabras del texto y la cantidad de veees que se repite

    public Files(double number,string Title,Dictionary<string,double>repeat,string[]txtnorm,string text){

        WordScore = new Dictionary<string, double>();
        this.number=number;
        this.Title=Title;
        this.repeat=repeat;
        this.txtnorm= txtnorm;
        this.text=text;
        this.Length=txtnorm.Length;
        this.snippet="";
    }
    
}