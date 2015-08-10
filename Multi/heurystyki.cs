using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;
namespace Multi
{
    class heurystyki
    {


        public void KPP(int nadawca, int[] odbiorcy, int ile_PC, siec[] graf, int n, int delta)  //metoda - wywołanie algorytmu KPP
        {
            int licz = (ile_PC * (ile_PC - 1)) / 2, sc = 0;
            double koszt, opuzn;
            siec[] lista_sciezek = new siec[licz];
            siec pomoc = new siec();
            odbiorcy[0] = nadawca;

            //////////////////////////////////////// Krok 1///////////////////////////////////////////
            /////////////////konstrukcja spujnego nieskierowanego grafu N' //////////////////////////
            ///skladajacego sie z wszystkich odbiorcow i nadawcowi sciezek o najnizszym koszczie////

            for (int i = 0; i < ile_PC; i++)
            {
                for (int j = i + 1; j < ile_PC; j++)
                {
                    lista_sciezek[sc] = pomoc.sciezka(odbiorcy[i], odbiorcy[j], n, graf);		//obliczam sciezki 
                    sc++;
                }
            }

            /////////////////////////rozwijam graf do grafu pełnego nieskierowanego//////////////////
            int b = 0, c = 0;
            siec[] graf_kpp = new siec[n];
            for (int i = 0; i < n; i++)
                graf_kpp[i] = null;

            for (int i = 0; i < licz; i++)
            {
                pomoc = lista_sciezek[i];

                b = pomoc.to;
                koszt = 0;
                opuzn = 0;

                while (pomoc != null)
                {
                    koszt = koszt + pomoc.cost;
                    opuzn = opuzn + pomoc.delay;

                    if (pomoc.next == null) c = pomoc.from;
                    pomoc = pomoc.next;

                }

                siec nowy1 = new siec(koszt, opuzn, c, b, i, graf_kpp[b]);
                graf_kpp[b] = nowy1;
            }
        ///////////////////////////////////////////KROK 1 koniec/////////////////////////////////////////


        ///////////////////////////////////////////KROK 2///////////////////////////////////////////////
            
            List<bool> vis = new List<bool>();	//wierzcholek odwiedzony							//obliczenie minimalnego drzewa spinajacego z grafu kpp
            List<int> waga = new List<int>();  //delay polaczenia									//metryka jest delay
            List<int> waga_k = new List<int>();//koszt 


            HashSet<siec> kopiec = new HashSet<siec>();
          
           
       


         
        }

        
        public void CSPT() //metoda - wywołanie algorytmu CSPT
        {

        }
    }
}
