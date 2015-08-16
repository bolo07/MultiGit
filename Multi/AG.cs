using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace Multi
{
    class AG 

    {
        
        public static int seed = Environment.TickCount;
        System.Random x = new Random(seed);
        public void algorytm_genetyczny(int p_pocz, Int16 m_generowania, Int16 m_selekcji, Int16 m_krzyzowania, double wsp_mutacji, siec[] graf, int[] odbiorcy, int ile_sciezek )
        {
            List<List<siec>> l_tab_r = new List<List<siec>>(); //lista tablic routingu (baza genów)
            List<siec> chromosom ;        //chromosom (drzewo transmisji multicast)
            List<List<siec>> populacja_pocz = new List<List<siec>>();
            

            /// Generowanie populacji poczatkowej
            switch(m_generowania)
            {
                case 1://generuje tablice routingu przeszukując grf w szerz
                    int rand=0;
                  

                    for (int i = 1; i < odbiorcy.Count(); i++)
                    {

                        l_tab_r.Add(Generowanie_BFS(graf, ile_sciezek, odbiorcy[0], odbiorcy[i])); 
                        
                    }
                    for (int j = 0; j < p_pocz; j++)
                    {
                        chromosom = new List<siec>(odbiorcy.Count()-1);

                        for (int i = 0; i < odbiorcy.Count() - 1; i++)
                        {
                            rand = x.Next(0, l_tab_r[i].Count());
                            chromosom.Add(l_tab_r[i][rand]);
                        }

                        populacja_pocz.Add(chromosom);
                    }


                  /*  siec node;

                    for (int k = 0; k < populacja_pocz.Count(); k++)
                    {
                        Debug.WriteLine("");
                        Debug.WriteLine("Osobnik ("+k+")");
                        for (int i = 0; i < populacja_pocz[k].Count(); i++)

                        {
                            for (node = populacja_pocz[k][i]; node != null; node = node.next)
                            {
                                Debug.Write(node.from + " -> ");

                            }
                            Debug.Write(odbiorcy[0]);
                            Debug.WriteLine("");
                        }
                    }*/  
                        Debug.Write("pause");
                    break;
                
                case 2:
                    break;
   

            }//Gen_populacji

          ////ocena
         
         ////selekcja
#region
            switch (m_selekcji)
            {
                case 1:

                    break;

                case 2:

                    break;

                case 3:

                    break;
            }//selekcja
#endregion
            //krzyżowanie
#region
            switch(m_krzyzowania)
            {
                case 1:

                    break;

                case 2:

                    break;

                case 3:

                    break;
            }//krzyżowanie
#endregion
        }//algorytm_genetyczny

        
        public List<siec> Generowanie_BFS(siec[] graf, int ile_sciezek, int nadawca, int odbiorca)
        {
            List<siec> tab_r = new List<siec>();               //tablica routingu
            bool[] vis = new bool[graf.Count()];              //kontrolka
            List<int> kolejka = new List<int>();             //kolejka
            List<int> gen = new List<int>();                //scieżka
            siec node =new siec();                         //krawędź
            siec nw ;
            int random = 0;
            int koniec = nadawca;
            int wczesniej = nadawca;


            

            int licz = 0, licz2=0; 
            while(licz2 <ile_sciezek)
            {
                kolejka.Clear();
                tab_r.Add(null);

                gen.Clear();
                gen.Add(0);
                
                for (int i = 0; i < graf.Count(); i++) { vis[i] = false; } //zerowanie kontrolki

                vis[nadawca] = true; //odwiedzony

                




                for (node = graf[nadawca]; node != null; node = node.next) //dodanie sasiadow startu
                {
                    if (vis[node.to] != true)
                    {
                        kolejka.Add(node.to);
                    }
                } //for


                do
                {


                    while (kolejka.Count == 0 && koniec != odbiorca) //jak wejdzie w slepy zaulek
                    {
                        //cout<<gen.back();
                        gen.RemoveAt(gen.Count() - 1);
                        for (node = graf[gen[gen.Count - 1]]; node != null; node = node.next) //dodanie sasiadow startu
                        {
                            if (vis[node.to] != true)
                            {
                                kolejka.Add(node.to);
                            }
                        } //for

                    }

                    random = x.Next(0, kolejka.Count); //wylosowanie kolejnego genu
                    node = graf[kolejka[random]];
                    koniec = kolejka[random];

                    gen.Add(kolejka[random]); //dodaj wylosowany wierzcholek do chromosomu       

                    vis[kolejka[random]] = true;
                    kolejka.Clear();

                    for (; node != null; node = node.next) //dodanie sasiadow wylosowanego wierzcholka
                    {
                        if (!vis[node.to])
                        {
                            kolejka.Add(node.to);
                        }
                    } //for

                } while (koniec != odbiorca); //do while
           
                
               

                siec kopia;

                int j = 0;
                for (j = 1; j < gen.Count();j++ )
                {
                    


                    for (nw = graf[gen[j]]; nw != null; nw = nw.next)
                    {
                        if (nw.from == gen[j] && nw.to == gen[j - 1])
                        {
                            kopia = new siec(nw);
                            node = kopia;
                            node.next = tab_r[licz];
                            tab_r[licz] = node;
                        }
                    }
                 }

              
                if (licz != 0)
                {
                   
                    
                    for (int i = 0; i < tab_r.Count(); i++)
                    {
                        nw = tab_r[licz];
                        
                        if (i != licz && nw !=null )
                        {
                            kopia = tab_r[i];
                            if (kopia != null)
                            {

                                while (kopia.to == nw.to)
                                {
                                    nw = nw.next;
                                    kopia = kopia.next;
                                    
                                    if (nw == null && kopia == null)
                                    {
                                        tab_r.RemoveAt(licz);
                                        licz--;
                                       
                                        break;
                                        
                                    }

                                }
                            }
                        }

                    }

                }
                
                


                licz++;
                licz2++;
            }//while główny
    /*
            Debug.WriteLine("");
            for (int i = 0; i < tab_r.Count(); i++)
            {
                for (node = tab_r[i]; node != null; node = node.next)
                {
                    Debug.Write(node.from + " -> ");

                }
                Debug.Write(nadawca);
                Debug.WriteLine("");
            }

         */   
            
            return tab_r;

        }


    }


}
