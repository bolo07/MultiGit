using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;
using System.Reflection;

namespace Multi
{
   public class siec
    {

    
     public siec ja;		//wskaźnik na samego siebie
     public siec before;	//wskaźnik na wcześniejszą krawędź
     public siec next;		//wskaźnik na następną krawędź

     public int id;				//globalny identyfikator
     public int from;			//źródło
     public int to;			    //cel
     public double cost;		//koszt
     public double length;		//długość lini
     public double delay;			//opóżnienie
   
   


	public  siec(){
            ja = this;
            next = null;
            before = null;
    }
    public static T DeepCopy<T>(T obj)
    {
        if (obj == null)
            throw new ArgumentNullException("Object cannot be null");
        return (T)Process(obj, new Dictionary<object, object>() { });
    }
    static object Process(object obj, Dictionary<object, object> circular)
    {
        if (obj == null)
            return null;
        Type type = obj.GetType();
        if (type.IsValueType || type == typeof(string))
        {
            return obj;
        }
        if (circular.ContainsKey(obj))
            return circular[obj];
        if (type.IsArray)
        {
            string typeNoArray = type.FullName.Replace("[]", string.Empty);
            Type elementType = Type.GetType(string.Format("{0}, {1}", typeNoArray, type.Assembly.FullName));
            var array = obj as Array;
            Array copied = Array.CreateInstance(elementType, array.Length);
            circular[obj] = copied;
            for (int i = 0; i < array.Length; i++)
            {
                object element = array.GetValue(i);
                object copy = null;
                if (element != null && circular.ContainsKey(element))
                    copy = circular[element];
                else
                    copy = Process(element, circular);
                copied.SetValue(copy, i);
            }
            return Convert.ChangeType(copied, obj.GetType());
        }
        if (type.IsClass)
        {
            object toret = Activator.CreateInstance(obj.GetType());
            circular[obj] = toret;
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                object fieldValue = field.GetValue(obj);
                if (fieldValue == null)
                    continue;
                object copy = circular.ContainsKey(fieldValue) ? circular[fieldValue] : Process(fieldValue, circular);
                field.SetValue(toret, copy);
            }
            return toret;
        }
        else
            throw new ArgumentException("Unknown type");
    }
	public siec(siec obiekt)
    {
        this.ja=this;
        this.before = null;
	    this.next = obiekt.next;

	    this.id = obiekt.id;
	    this.from = obiekt.from;
	    this.to = obiekt.to;
	    this.cost = obiekt.cost;
	    this.length = obiekt.length;
	    this.delay = obiekt.delay;
    }
    public siec(double cost_c, double delay_c, int to_c, int from_c, int id_c, siec next_c)
    {
        this.ja = this;
        this.before = null;
        this.next = next_c;

        this.id = id_c;
        this.from = from_c;
        this.to = to_c;
        this.cost = cost_c;
        this.delay = delay_c;

    }
    public siec[] wczytaj(string plik)  //metoda wczytuje sieć z pliku
    {
        
            int counter = 0;
            string line;
            string[] tab; //tablica obiektów

            //Pobieramy bieżące ustawienia
            string currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            CultureInfo ci = new CultureInfo(currentCulture);
            //Ustawiamy nowy format separatora dziesiętnego
            ci.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            // odczytywanie ilosci lini
            System.IO.StreamReader file = new System.IO.StreamReader(@plik);
            int ile_node = Convert.ToInt32(file.ReadLine());

            //inicjacja tablicy sąsiedztwa
            siec[] graf = new siec[ile_node];
            for (int i = 0; i < ile_node; i++)
                graf[i] = null;

            while ((line = file.ReadLine()) != null)
            {  
                //odczyt lini danych i podział na części
                tab = line.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                //utworzenie nowej gałęzi
                
                siec temp = new siec();
                temp.id = Convert.ToInt16(tab[0]);
                temp.from = Convert.ToInt16(tab[1]);
                temp.to = Convert.ToInt16(tab[2]);
                temp.cost = Convert.ToDouble(tab[5]);
                temp.delay = Convert.ToDouble(tab[4]);
                temp.length = Convert.ToDouble(tab[3]);             
                temp.next = graf[temp.from];
                if (temp.next != null) temp.next.before = temp;
                graf[temp.from] = temp;
                

                siec temp1 = new siec();
                temp1.id = Convert.ToInt16(tab[0]);
                temp1.from = Convert.ToInt16(tab[2]);
                temp1.to = Convert.ToInt16(tab[1]);
                temp1.cost = Convert.ToDouble(tab[5]);
                temp1.delay = Convert.ToDouble(tab[4]);
                temp1.length = Convert.ToDouble(tab[3]);                
                temp1.next = graf[temp1.from];
                if (temp1.next != null) temp1.next.before = temp1;
                
                graf[temp1.from] = temp1;
                

                counter++;
            }


           // MessageBox.Show("Wczytano graf z " + Convert.ToString(ile_node) + " wierzchołków i " + counter + " krawędzi");
            file.Close();
          
        
        return graf;

       
    }
    public  siec sciezka(int start, int koniec, int n, siec[] graf, string metryka)
        {
            const int MAXINT = 2147483647;
            int i,j,u,x,v,hlen,parent,left,right,dmin,pmin,child;              
            v=start;
            siec pw,nw,poczatek;

             // Tworzymy tablice dynamiczne

            double[]d       = new double [n];			    // Tablica kosztów dojścia
            double[]op   = new double [n];			// Tablica opuznien
            int[]p       = new int [n];             // Tablica poprzedników
            bool[]QS     = new bool [n];            // Zbiory Q i S
            int[]S       = new int [n];             // Stos
            int[]h       = new int [n];             // Kopiec
            int[]hp      = new int [n];             // Pozycje w kopcu
            int sptr     = 0;                       // Wskaźnik stosu
  

             // Inicjujemy tablice dynamiczne

            for(i = 0; i < n; i++)
             {
                 d[i] = MAXINT;
           	     op[i]=MAXINT;
                 p[i] = -1;
                 QS[i] = false;
                 h[i] = hp[i] = i;
	
                }//for



            hlen = n;
            d[v] = 0;                       // Koszt dojścia v jest zerowy
            op[v]=0;
            x = h[0]; h[0] = h[v]; h[v] = x; // odtwarzamy własność kopca
            hp[v] = 0; hp[0] = v;

            // Wyznaczamy ścieżki

            for(i = 0; i < n; i++)
                {
                    u = h[0];                     // Korzeń kopca jest zawsze najmniejszy

                    // Usuwamy korzeń z kopca, odtwarzając własność kopca
                    h[0] = h[--hlen];             // W korzeniu umieszczamy ostatni element
                    hp[h[0]] = parent = 0;        // Zapamiętujemy pozycję elementu w kopcu
                    
                while(true)                   // W pętli idziemy w dół kopca, przywracając go
                    {
                        left  = parent + parent + 1; // Pozycja lewego potomka
                        right = left + 1;           // Pozycja prawego potomka
                        if(left >= hlen) break;     // Kończymy, jeśli lewy potomek poza kopcem
                        dmin = Convert.ToInt32( d[h[left]]);          // Wyznaczamy mniejszego potomka
                        pmin = left;
                        if((right < hlen) && (dmin > d[h[right]]))
                            {
                                dmin = Convert.ToInt32(d[h[right]]);
                                pmin = right;
                                }
                        if(d[h[parent]] <= dmin) break; // Jeśli własność kopca zachowana, kończymy
                        x = h[parent]; h[parent] = h[pmin]; h[pmin] = x; // Przywracamy własność kopca
                        hp[h[parent]] = parent; hp[h[pmin]] = pmin;      // na danym poziomie
                        parent = pmin;              // i przechodzimy na poziom niższy kopca
                     }

             // Znaleziony wierzchołek przenosimy do S

                    QS[u] = true;

             // Modyfikujemy odpowiednio wszystkich sąsiadów u, którzy są w Q
                    if (metryka == "cost")
                    {
                        for (pw = graf[u]; pw != null; pw = pw.next)
                            if (!QS[pw.to] && (d[pw.to] > d[u] + pw.cost))
                            {

                                d[pw.to] = d[u] + pw.cost;
                                op[pw.to] = op[u] + pw.delay;
                                p[pw.to] = u;

                                // Po zmianie d[v] odtwarzamy własność kopca, idąc w górę		
                                for (child = hp[pw.to]; child != null; child = parent)
                                {
                                    parent = child / 2;
                                    if (d[h[parent]] <= d[h[child]]) break;
                                    x = h[parent]; h[parent] = h[child]; h[child] = x;
                                    hp[h[parent]] = parent; hp[h[child]] = child;
                                }
                            }
                    }
                    if (metryka == "delay")
                    {
                        for (pw = graf[u]; pw != null; pw = pw.next)
                            if (!QS[pw.to] && (d[pw.to] > d[u] + pw.delay))
                            {

                                d[pw.to] = d[u] + pw.delay;
                                op[pw.to] = op[u] + pw.cost;
                                p[pw.to] = u;

                                // Po zmianie d[v] odtwarzamy własność kopca, idąc w górę		
                                for (child = hp[pw.to]; child != null; child = parent)
                                {
                                    parent = child / 2;
                                    if (d[h[parent]] <= d[h[child]]) break;
                                    x = h[parent]; h[parent] = h[child]; h[child] = x;
                                    hp[h[parent]] = parent; hp[h[child]] = child;
                                }
                            }
                    }
  }


  siec kopia;
  poczatek=null;

  for(j = koniec; j > -1; j = p[j]) 
    {
	  if(p[j]>-1){
		nw=graf[j];

		while(nw !=null){
	      if(nw.to == p[j] && nw.from ==j){
			  kopia = new siec(nw);
			  pw=kopia;
			  pw.next=poczatek;
              if (pw.next != null) pw.next.before = kopia;
		      poczatek=pw;

              
		  
		  }
		  nw=nw.next;
		}
	  }
  }

/*
    Debug.WriteLine("");
    i=koniec;
    Debug.Write(start+"->"+i+" ");
    

    // Ścieżkę przechodzimy od końca ku początkowi,
    // Zapisując na stosie kolejne wierzchołki
	
	for(j = i; j > -1; j = p[j]) S[sptr++] = j;

    // Wyświetlamy ścieżkę, pobierając wierzchołki ze stosu

    while(sptr >0) Debug.Write(S[--sptr]+" ");

    // Na końcu ścieżki wypisujemy jej koszt
    if (poczatek == null)
    {
        Debug.Write("Nie można odnaleść scieżki");
    }
    else
    {
        Debug.Write(" $" + d[i] + "," + op[i]);
    }
   
	*/


return (poczatek);
  
} //algorytm dijkstry 

   

    }
}
