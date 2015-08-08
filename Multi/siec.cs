using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;

namespace Multi
{
   public class siec
    {

    
     public siec ja;		//wskaźnik na samego siebie
     public siec before;	//wskaźnik na wcześniejszą krawędź
     public siec next;		//wskaźnik na następną krawędź

     public int id;				//globalny identyfikator
     public int from;			//źródło
     public int to;			//cel
     public double length;		//długość krawędzi
     public double bandwidth;		//pasmo
     public double delay;			//opóżnienie
   
   


	public  siec(){
            ja = this;
            next = null;
    }
        
	public siec(siec obiekt)
    {
        this.ja=this;
        this.before = null;
	    this.next = obiekt.next;

	    this.id = obiekt.id;
	    this.from = obiekt.from;
	    this.to = obiekt.to;
	    this.length = obiekt.length;
	    this.bandwidth = obiekt.bandwidth;
	    this.delay = obiekt.delay;
    }
	
     public  siec(int id_c, int from_c, int to_c, float length_c, float bandwidth_c, float delay_c, siec next_c)
    {
        this.ja = this;
        this.before = null;
        this.next = next_c;

        this.id = id_c;
        this.from = from_c;
        this.to = to_c;
        this.length = length_c;
        this.bandwidth =bandwidth_c;
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
                temp.length = Convert.ToDouble(tab[3]);
                temp.delay = Convert.ToDouble(tab[4]);
                temp.bandwidth = Convert.ToDouble(tab[5]);
                temp.next = graf[temp.from];
                graf[temp.from] = temp;

                siec temp1 = new siec();
                temp1.id = Convert.ToInt16(tab[0]);
                temp1.from = Convert.ToInt16(tab[2]);
                temp1.to = Convert.ToInt16(tab[1]);
                temp1.length = Convert.ToDouble(tab[3]);
                temp1.delay = Convert.ToDouble(tab[4]);
                temp1.bandwidth = Convert.ToDouble(tab[5]);
                temp1.next = graf[temp1.from];
                graf[temp1.from] = temp1;
                
                counter++;
            }


            MessageBox.Show("Wczytano graf z " + Convert.ToString(ile_node) + " wierzchołków i " + counter + " krawędzi");
            file.Close();
        /*
            siec temp3;
            for (int i = 0; i < ile_node; i++) //wyswietla Lsonsiadow
            {
                Debug.Write( "graf[" +i+ "] =");
                temp3 = graf[i];
                while (temp3!=null)
                {
                   Debug.Write( temp3.to + " ");
                   temp3=temp3.next;
                }
                Debug.WriteLine("");
            }*/
     
        
        return graf;

       
    }


	 ~siec() {}

    }
}
