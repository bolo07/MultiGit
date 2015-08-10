using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi
{
    class heurystyki
    {

        
        public void KPP(int nadawca, int[] odbiorcy, int ile_PC, siec []graf, int n, int delta)  //metoda - wywołanie algorytmu KPP
        {
            int licz = (ile_PC * (ile_PC - 1)) / 2, sc = 0;
            siec[] lista_sciezek = new siec[licz];
            siec pomoc = new siec();

            //////////////// STEP 1 - konstrukcja spujnego nieskierowanego grafu N' /////////////////////////////////////////
            //skladajacego sie z wszystkich odbiorcow i nadawcowi sciezek o najnizszym koszczie 
            for(int i=0; i<ile_PC; i++)				
		        {										
			        for(int j=i+1; j<ile_PC; j++)		
			            {					
				         lista_sciezek[sc]=pomoc.sciezka (odbiorcy[i], odbiorcy[j], n, graf );		//obliczam sciezki 
				         sc++;
			            }
		        }
       
        
    
        }


        
        public void CSPT() //metoda - wywołanie algorytmu CSPT
        {

        }
    }
}
