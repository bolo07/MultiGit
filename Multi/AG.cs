using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Globalization;
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

                    l_tab_r.Add(Generowanie_Dijkstra(graf,ile_sciezek, odbiorcy[0], odbiorcy[1]));
                    

                    break;
   

            }//Gen_populacji

         /////ocena
         
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

        public List<siec> Generowanie_Dijkstra(siec[] graf, int ile_sciezek, int nadawca, int odbiorca)
        {
            List<siec> tab_r = new List<siec>();               //tablica routingu
            List<siec> stos = new List<siec>();                  // stos z potencjalnymi najkrutszymi ściezkami
            List<siec> removed_node = new List<siec>();       //usuniete krawędzie
            siec dijkastra = new siec();
            siec pomoc, pomoc2;
            siec root_path;
            siec total_path;                                //suma root_path i spur_path
            siec spur_node;
            int dlugosc_sciezki;

            siec[] kopia_graf = siec.DeepCopy(graf);
      

         
            tab_r.Add(dijkastra.sciezka(nadawca, odbiorca, graf.Count(), graf, "cost")); //obliczenie najkrutszej ścieżki 
              
            for (int k = 1; k < ile_sciezek; k++)//główny for ile sciezek trzeba wyznaczyć
                {
                    dlugosc_sciezki = 0;
                    for (pomoc = tab_r[k-1]; pomoc != null; pomoc = pomoc.next){dlugosc_sciezki++;}//liczy ilość węzłów w ścieżce
                    root_path = new siec(tab_r[k - 1]);     //sciezka                   
                    spur_node = tab_r[k - 1];              //nowy wiezchołek startowy  
                    root_path.next=null;

                    for(int i =0; i<dlugosc_sciezki; i++)//
                    {


                        
                        for (int c = 0; c < tab_r.Count(); c++) //usuwa wszystkie krawędzie należące do tab_r z węzłem startowym spur_node z graf
                        {
                            
                            for (pomoc2 = tab_r[c]; pomoc2 != null; pomoc2 = pomoc2.next)
                            {
                                

                                if (spur_node.to == pomoc2.to)
                                {

                                    for (pomoc = kopia_graf[spur_node.from]; pomoc != null; pomoc = pomoc.next)
                                    {

                                        if (spur_node.to == pomoc.to && spur_node.from == pomoc.from)
                                        {
                                            if (pomoc.before == null)
                                            {
                                                pomoc = pomoc.next;
                                            }
                                            else
                                            {
                                                removed_node.Add(pomoc);
                                                pomoc.before.next = pomoc.next;
                                            }

                                        }


                                    } //for
                                    for (pomoc = kopia_graf[spur_node.to]; pomoc != null; pomoc = pomoc.next)
                                    {

                                        if (spur_node.to == pomoc.from && spur_node.from == pomoc.to)
                                        {
                                            if (pomoc.before == null) 
                                            {
                                                pomoc = pomoc.next;
                                            }
                                            else
                                            {
                                                removed_node.Add(pomoc);
                                                pomoc.before.next = pomoc.next;
                                            }
                                        }

                                    } //for

                                }
                                if (i == 1)
                                {
                                    int kontrolka = 0;
                                    kopia_graf[nadawca] = null;
                                    while (kontrolka < kopia_graf.Count())
                                    {
                                        

                                        for (pomoc = kopia_graf[kontrolka]; pomoc != null; pomoc = pomoc.next)
                                        {

                                            if (nadawca == pomoc.to)
                                            {
                                                if (pomoc.before == null)
                                                {
                                                    kopia_graf[kontrolka] = pomoc.next;
                                                    break;
                                                }
                                                else
                                                {
                                                    removed_node.Add(pomoc);
                                                    pomoc.before.next = pomoc.next;
                                                    break;

                                                }
                                            }

                                        }
                                        kontrolka++;
                                    }
                                }
                            }
                         }//koniec for usuwa


                        total_path = new siec(root_path);
                        if (i == 0)
                        {
                            stos.Add(dijkastra.sciezka(spur_node.to, odbiorca, kopia_graf.Count(), kopia_graf, "cost"));
                        }
                        else
                        {
                           
                            stos.Add(dijkastra.sciezka(spur_node.to, odbiorca, kopia_graf.Count(), kopia_graf, "cost"));
                        }

                        
                        //kopia_graf[spur_node.to] = new siec(graf[spur_node.to]);
                        //kopia_graf[spur_node.from] = new siec(graf[spur_node.from]);

                        

                        root_path.next = spur_node.next;
                        spur_node = spur_node.next;
                    }
                    if (stos.Count() == 0)
                    {
                        break;
                    }
                    
                    

                }//for główny

                return tab_r;
        }
    }


}
