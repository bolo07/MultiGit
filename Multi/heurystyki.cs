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
            //////////////////////obliczenie minimalnego drzewa spinajacego z grafu kpp 
            //////////////////////metryka jest delay

            bool[] vis = new bool [n] ;	     //wierzcholek odwiedzony							
            double[] waga = new double[n];  //delay polaczenia									
            double[] waga_k = new double[n];      //koszt 

            Dictionary<siec, siec> kopiec = new Dictionary<siec,siec>(n);

            for(int i=0; i<n; i++)
                {
                 waga_k[i]=0;
                 waga[i]= 0;				//inicjowanie tablic
                 vis[i]= false;
                }

            int v = 0, waga_mst = 0;

            siec[] mst = new siec[n];

            for (int i = 0; i < n; i++)
                 mst[i] = null;


            vis[0] = true;				//wierzcholek startowy (nadawca)
            waga[odbiorcy[0]] = 0;
            waga_k[odbiorcy[0]] = 0;

            kopiec.Clear();
    
            if(!vis[pomoc.to] )
		        {	
			if( waga[pomoc.to]+ pomoc.delay < delta  )
			{
				vis[pomoc.to]=true;				//dodaje nowa krawedz do drzewa
				waga[pomoc.to]= pomoc.delay;
				waga_k[pomoc.to]= waga_k[pomoc.to]+pomoc.cost;

				//nowy2 = new wezel(pomoc->koszt, pomoc->delay, pomoc->pi, pomoc->v, pomoc->id, mst[pomoc->v] );
				//mst[pomoc->v]=nowy2;
				siec nowy2 = new siec(pomoc.cost, pomoc.delay, pomoc.to, pomoc.to, pomoc.id, mst[pomoc.from] );
				mst[pomoc.from]=nowy2;
				v=pomoc.to;
			}//else 
				//{
				//kopiec.erase(kopiec.begin()); //usowam 1 element kopca
			   // warunek = false;
				//}
		}
		//if(warunek){
            kopiec.Remove()
			kopiec.erase(kopiec.begin()); //usowam 1 element kopca
			for(pomoc = graf_kpp[v]; pomoc; pomoc = pomoc->next)	//dodaje nie odwiedzonych sasiadow do kopca
				if(!vis[pomoc->v])	
				{
					nowy2 = new wezel(*pomoc);
					kopiec.insert(*nowy2);
				}
		//}
		//warunek=true;
	}
           
       


         
        }

        
        public void CSPT() //metoda - wywołanie algorytmu CSPT
        {

        }
    }
}
