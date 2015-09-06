using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

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
            siec nowy2;
            odbiorcy[0] = nadawca;

            ///////////////////////////////////////// Krok 1 ////////////////////////////////////////////////
            #region
            /////////////////konstrukcja spujnego nieskierowanego grafu N' //////////////////////////
            ///skladajacego sie z wszystkich odbiorcow i nadawcowi sciezek o najnizszym koszczie////

            for (int i = 0; i < ile_PC; i++)
            {
                for (int j = i + 1; j < ile_PC; j++)
                {
                    lista_sciezek[sc] = pomoc.sciezka(odbiorcy[i], odbiorcy[j], n, graf,"cost");		//obliczam sciezki 
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
            #endregion

            ////////////////////////////////////////// KROK 2 //////////////////////////////////////////////
            #region
            //////////////////////obliczenie minimalnego drzewa spinajacego z grafu kpp 
            //////////////////////metryka jest delay

            bool[] vis = new bool [n] ;	     //wierzcholek odwiedzony							
            double[] waga = new double[n];  //delay polaczenia									
            double[] waga_k = new double[n];      //koszt 
           
            HashSet<siec>kopiec = new HashSet<siec>();
            //Dictionary<siec, siec> kopiec = new Dictionary<siec,siec>(n);

            for(int i=0; i<n; i++)
                {
                 waga_k[i]=0;
                 waga[i]= 0;				//inicjowanie tablic
                 vis[i]= false;
                }

            int v = 0;
            double waga_mst = 0;

            siec[] mst = new siec[n];

            for (int i = 0; i < n; i++)
                 mst[i] = null;


            vis[0] = true;				//wierzcholek startowy (nadawca)
            waga[odbiorcy[0]] = 0;
            waga_k[odbiorcy[0]] = 0;

            kopiec.Clear();

            for (pomoc = graf_kpp[odbiorcy[0]]; pomoc != null; pomoc = pomoc.next) //dodanie nie odwiedzonych sasiadow do kopca
            {
                nowy2 = new siec(pomoc);
                kopiec.Add(nowy2);
            }

            while (kopiec.Count >0) //glowna petla mst
            {

                pomoc = kopiec.FirstOrDefault().ja;
                                
                if (vis[pomoc.to]==false)
                {
                    if (waga[pomoc.to] + pomoc.delay < delta)
                    {
                        vis[pomoc.to] = true;				//dodaje nowa krawedz do drzewa
                        waga[pomoc.to] = pomoc.delay;
                        waga_k[pomoc.to] = waga_k[pomoc.to] + pomoc.cost;

                        //nowy2 = new wezel(pomoc->koszt, pomoc->delay, pomoc->pi, pomoc->v, pomoc->id, mst[pomoc->v] );
                        //mst[pomoc->v]=nowy2;
                        nowy2 = new siec(pomoc.cost, pomoc.delay, pomoc.to, pomoc.from, pomoc.id, mst[pomoc.from]);
                        mst[pomoc.from] = nowy2;
                        v = pomoc.to;
                    }//else 
                    //{
                    //kopiec.erase(kopiec.begin()); //usowam 1 element kopca
                    // warunek = false;
                    //}
                }


                //if(warunek){

                kopiec.Remove(kopiec.FirstOrDefault());
                //kopiec.erase(kopiec.begin()); //usowam 1 element kopca
                for (pomoc = graf_kpp[v]; pomoc != null; pomoc = pomoc.next)	//dodaje nie odwiedzonych sasiadow do kopca
                {
                    if (vis[pomoc.to]==false)
                    {
                        nowy2 = new siec(pomoc);

                        kopiec.Add(nowy2);
                    }
                }
                //}
                //warunek=true;
            }
            ///////////////////////////////////////////KROK 2 KONIEC///////////////////////////////////////////////////////
#endregion

            ////////////////////////////////////////// KROK 3 //////////////////////////////////////////////
            #region
            ///////////////Zastąp krawędzie powstałego drzewa oryginalnymi z grafu z modelem sieci/////////////////////////
            bool [][] kontrolka = new bool[n][];
            for (int i = 0; i < n; i++)
            {
                kontrolka[i] = new bool[n];
            }

            //zerowanie kontrolki
            for(int f=0; f<n; f++)
                {	
			        for(int h=0; h<n; h++)
                    {
				        kontrolka[f][h]=false;
			        }   
		        }
	
            siec pomoc2=new siec();
            siec[] mst2 = new siec[n];
            int iterator;
	        
            for(int i=0; i<n; i++)
            {
		        mst2[i]= null;
            }

            for (int i = 0; i < n; i++)	//odtwarzam pierwotne mst
            {
                for (pomoc = mst[i]; pomoc != null; pomoc = pomoc.next)	//dla kazdego graf_mst
                {
                    iterator = 0;
                    b = pomoc.from;
                    c = -1;

                    while (lista_sciezek[iterator].to != b || pomoc.to != c)
                    {
                        if (lista_sciezek[iterator].to != b)
                        {
                            iterator++;
                        }
                        else
                        {
                            for (pomoc2 = lista_sciezek[iterator]; pomoc2 != null; pomoc2 = pomoc2.next)	//wyszukiwanie krawedzi w liscie sciezek
                            {
                                if (pomoc2.next == null && pomoc2.from == pomoc.to) c = pomoc2.from;

                            }
                            if (c == -1)
                            {
                                iterator++;
                            }
                        }
                    }

                    for (pomoc2 = lista_sciezek[iterator]; pomoc2 != null; pomoc2 = pomoc2.next)
                    {
                        if (kontrolka[pomoc2.from][pomoc2.to] != true && kontrolka[pomoc2.to][pomoc2.from] != true)
                        {

                            nowy2 = new siec(pomoc2.cost, pomoc2.delay, pomoc2.from, pomoc2.to, pomoc2.id, mst2[pomoc2.to]); //tworzenie nowej krawedzi
                            mst2[pomoc2.to] = nowy2;
                            nowy2 = new siec(pomoc2.cost, pomoc2.delay, pomoc2.to, pomoc2.from, pomoc2.id, mst2[pomoc2.from]);
                            mst2[pomoc2.from] = nowy2;
                            kontrolka[pomoc2.to][pomoc2.from] = true;
                            kontrolka[pomoc2.from][pomoc2.to] = true;

                        }
                    }

                }
            }
            //////////////////////////////////////////////////KROK 3 KONIEC//////////////////////////////////////////////////////////////
            #endregion

            ///////////////////////////////////Zapis wyniku do pliku////////////////////////////////////////
            #region
            double delay_mst = 0;
            System.IO.StreamWriter plik = new System.IO.StreamWriter(@"KPP.txt");

	        for (int f = 0; f < n; f++)
	            { //zerowanie kontrolki
				    for (int h = 0; h < n; h++)
				        {
					        kontrolka[f][h] = false;
				        }
            	}

	        for (int i = 0; i < n; i++) //wyswietla Lsonsiadow graf_kpp
	            {
		            pomoc = mst2[i];
		            if (pomoc!=null)
		                {
		                while (pomoc !=null)
		                    {
			                    if (kontrolka[pomoc.from][pomoc.to] != true && kontrolka[pomoc.to][pomoc.from] != true)
			                        {
			                        waga_mst = waga_mst + pomoc.cost;
                                    delay_mst = delay_mst + pomoc.delay;
			                        kontrolka[pomoc.to][pomoc.from] = true;
			                        kontrolka[pomoc.from][pomoc.to] = true;
			                        }
		                       pomoc = pomoc.next;
		                    }
                    }
	            }

	        Debug.Write("\n");
	        Debug.Write("Koszt drzewa multicast wynosi = ");
	        Debug.Write(waga_mst);
	        Debug.Write("\n");
	        Debug.Write("\n");

	        Debug.Write("\n");
	        Debug.Write("DRZEWO MULTICAST ");
	        Debug.Write("\n");
	        Debug.Write("\n");

            plik.WriteLine( "\n" + "Koszt drzewa multicast wynosi = "+waga_mst + "\n");
	        plik.WriteLine("\n" + "DRZEWO MULTICAST " +"\n");


	

		    for (int i = 0; i < n; i++) //wyswietla Lsonsiadow graf_kpp
		        {
		            pomoc = mst2[i];
		            if (pomoc!=null)
		                {
		                Debug.Write("graf_mst2[");
		                Debug.Write(i);
		                Debug.Write("] =");

                        plik.Write("graf_mst2[" + i + "] =");
		
		                while (pomoc!=null)
		                    {
			                    Debug.Write(" "+ pomoc.to+" ");
                                plik.Write(" "+pomoc.to+" ");
			
		                        pomoc = pomoc.next;
		                    }
		
                        Debug.Write( "\n");
		                plik.WriteLine("");
                        }
		        }

	        Debug.Write("\n");

	        plik.Close();
            #endregion
            MessageBox.Show("Koszt drzewa transmisji grupowej wynosi:  " + Convert.ToString(waga_mst) + "  \n " + Convert.ToString(delay_mst));


        }//kpp koniec


        public void CSPT(int nadawca, int[] odbiorcy, int ile_PC, siec[] graf, int n, int delta) //metoda - wywołanie algorytmu CSPT
        {
            int sc = 0;
            odbiorcy[0] = nadawca;

            bool[] w_dodane = new bool[ile_PC]; //węzły które spełniały wymogi delta, dodane do grafu w pierwszym kroku

            siec[] lista_sciezek = new siec[ile_PC - 1];  //lista krok 1
            siec[] lista_sciezek2 = new siec[ile_PC - 1]; //lista krok 2
            siec[] lista_sciezek3 = new siec[ile_PC - 1]; //lista krok 3
            siec pomoc = new siec();
            siec pomoc2 = new siec();

            for (int i = 0; i < ile_PC - 1; i++)
            {
                lista_sciezek[i] = null;
                lista_sciezek2[i] = null;
                lista_sciezek3[i] = null;
            }
            //////////////////////////////// KROK 1 ////////////////////////////////////////////
            // Obliczanie mst zawierającego ściezki do odbiorców, które spełniaja założone delta
            //Metryką obliczania mst jest koszt scieżki
            #region
            for (int i = 0; i < w_dodane.Length; i++)
            {
                w_dodane[i] = false;
            }

            w_dodane[0] = true; //węzeł nadawczy musi byc dodany do drzewa

            double delay;


            for (int j = 1; j < ile_PC; j++)
            {
                delay = 0;
                pomoc = pomoc.sciezka(odbiorcy[0], odbiorcy[j], n, graf, "cost");


                for (pomoc2 = pomoc; pomoc2 != null; pomoc2 = pomoc2.next)
                {
                    delay = delay + pomoc2.delay;
                }
                if (delay < delta)
                {
                    lista_sciezek[sc] = pomoc;		//obliczam sciezki 
                    w_dodane[j] = true;
                    sc++;
                }

            }

            Debug.WriteLine("");

            ///////////////////////////// KROK 1 KONIEC /////////////////////////////////////////
          
            #endregion

            ///////////////////////////// KROK 2 ///////////////////////////////////////////////
            // Obliczanie mst zawierającego ściezki nie obliczone w kroku 1
            // Metryka obliczania mst jest opóźnienie ściezki
            #region

            for (int j = 0; j < ile_PC; j++)
            {
                if (w_dodane[j] == false)
                {
                    delay = 0;
                    pomoc = pomoc.sciezka(odbiorcy[0], odbiorcy[j], n, graf, "delay");
                    lista_sciezek2[sc] = pomoc;		//obliczam sciezki 
                    w_dodane[j] = true;

                    sc++;
                }
            }

            Debug.WriteLine("");
            ////////////////////////// KROK 2 KONIE ////////////////////////////////////////////  
        
            #endregion

            ////////////////////////// KROK 3 /////////////////////////////////////////////////
            //Scalanie drzew z Kroku 1 i kroku 2
                #region
            for (int i = 0; i < ile_PC - 1; i++)
            {
                if (lista_sciezek[i] != null)
                    lista_sciezek3[i] = lista_sciezek[i];
                if (lista_sciezek2[i] != null)
                    lista_sciezek3[i] = lista_sciezek2[i];
            }

            /////////////////////////////////////////////////////////////////////////////////
            //Rozwijanie listy sciezek do grafu nieskierowanego
            bool[][] kontrolka = new bool[n][];
            for (int i = 0; i < n; i++)
            {
                kontrolka[i] = new bool[n];
            }

            //zerowanie kontrolki
            for (int f = 0; f < n; f++)
            {
                for (int h = 0; h < n; h++)
                {
                    kontrolka[f][h] = false;
                }
            }

            siec[] mst = new siec[n];
            siec nowy2;

            for (int i = 0; i < n; i++)
            {
                mst[i] = null;
            }

            for (int i = 0; i < ile_PC - 1; i++)
            {
                for (pomoc2 = lista_sciezek3[i]; pomoc2 != null; pomoc2 = pomoc2.next)
                {
                    if (kontrolka[pomoc2.from][pomoc2.to] != true && kontrolka[pomoc2.to][pomoc2.from] != true)
                    {

                        nowy2 = new siec(pomoc2.cost, pomoc2.delay, pomoc2.from, pomoc2.to, pomoc2.id, mst[pomoc2.to]); //tworzenie nowej krawedzi
                        mst[pomoc2.to] = nowy2;
                        nowy2 = new siec(pomoc2.cost, pomoc2.delay, pomoc2.to, pomoc2.from, pomoc2.id, mst[pomoc2.from]);
                        mst[pomoc2.from] = nowy2;
                        kontrolka[pomoc2.to][pomoc2.from] = true;
                        kontrolka[pomoc2.from][pomoc2.to] = true;

                    }
                }
            }
            /////////////////////////////////////////////////// KROK 3 KONIEC////////////////////////////////////////////////

            #endregion

            ////////////////////////////////////////////////// Zapis wyniku do pliku/////////////////////////////////////////
            #region
            System.IO.StreamWriter plik = new System.IO.StreamWriter(@"CSPT.txt");
            double waga_cspt = 0, delay_cspt=0;

            for (int f = 0; f < n; f++)
            { //zerowanie kontrolki
                for (int h = 0; h < n; h++)
                {
                    kontrolka[f][h] = false;
                }
            }

            for (int i = 0; i < n; i++) //oblicza calkowity koszt drzewa multicast
            {
                pomoc = mst[i];
                if (pomoc != null)
                {
                    while (pomoc != null)
                    {
                        if (kontrolka[pomoc.from][pomoc.to] != true && kontrolka[pomoc.to][pomoc.from] != true)
                        {
                            waga_cspt = waga_cspt + pomoc.cost;
                            delay_cspt = delay_cspt + pomoc.delay;
                            kontrolka[pomoc.to][pomoc.from] = true;
                            kontrolka[pomoc.from][pomoc.to] = true;
                        }
                        pomoc = pomoc.next;
                    }
                }
            }


            Debug.Write("\n");
            Debug.Write("Koszt drzewa multicast wynosi = ");
            Debug.Write(waga_cspt);
            Debug.Write("\n");
            Debug.Write("\n");

            Debug.Write("\n");
            Debug.Write("DRZEWO MULTICAST ");
            Debug.Write("\n");
            Debug.Write("\n");

            plik.WriteLine("\n" + "Koszt drzewa multicast wynosi = " + waga_cspt + "\n");
            plik.WriteLine("\n" + "DRZEWO MULTICAST " + "\n");




            for (int i = 0; i < n; i++) //wyswietla Lsonsiadow graf_kpp
            {
                pomoc = mst[i];
                if (pomoc != null)
                {
                    Debug.Write("graf_mst2[");
                    Debug.Write(i);
                    Debug.Write("] =");

                    plik.Write("graf_mst2[" + i + "] =");

                    while (pomoc != null)
                    {
                        Debug.Write(" " + pomoc.to + " ");
                        plik.Write(" " + pomoc.to + " ");

                        pomoc = pomoc.next;
                    }

                    Debug.Write("\n");
                    plik.WriteLine("");
                }
            }

            Debug.Write("\n");

            plik.Close();
            #endregion

            MessageBox.Show("Koszt drzewa transmisji grupowej wynosi:  " + Convert.ToString(waga_cspt) + "  \n " + Convert.ToString(delay_cspt));
        }

    }//heurystyki

}//Multi
