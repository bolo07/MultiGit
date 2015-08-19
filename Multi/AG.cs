using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Globalization;

namespace Multi
{
    class AG 

    {
        siec sciezka;
        double koszt;
        double delay;
        public AG() { }
        public AG(siec sciezka, double koszt, double opoznienie )
        {
            this.sciezka = sciezka;
            this.koszt = koszt;
            this.delay = opoznienie;
        }
        
        public static int seed = Environment.TickCount;
        System.Random x = new Random(seed);
        public void algorytm_genetyczny(int p_pocz, Int16 m_generowania, Int16 m_selekcji, Int16 m_krzyzowania, double wsp_mutacji, siec[] graf, int[] odbiorcy, int ile_sciezek )
        {
            List<List<AG>> l_tab_r = new List<List<AG>>(); //lista tablic routingu (baza genów)
            List<AG> chromosom ;        //chromosom (drzewo transmisji multicast)
            List<List<AG>> populacja_pocz = new List<List<AG>>();
            

            /// Generowanie populacji poczatkowej
            switch(m_generowania)
            {
                
                case 1://generuje tablice routingu przeszukując grf w szerz
                    int rand1 = 0;
                  
                    
                    for (int i = 1; i < odbiorcy.Count(); i++)
                    {

                        l_tab_r.Add(Generowanie_BFS(graf, ile_sciezek, odbiorcy[0], odbiorcy[i])); 
                        
                    }
                    for (int j = 0; j < p_pocz; j++)
                    {
                        chromosom = new List<AG>(odbiorcy.Count()-1);

                        for (int i = 0; i < odbiorcy.Count() - 1; i++)
                        {
                            rand1 = x.Next(0, l_tab_r[i].Count());
                            chromosom.Add(l_tab_r[i][rand1]);
                        }

                        populacja_pocz.Add(chromosom);
                    }
                    
                    
                    siec node;

                    for (int k = 0; k < populacja_pocz.Count(); k++)
                    {
                        Debug.WriteLine("");
                        Debug.WriteLine("Osobnik ("+k+")");
                        for (int i = 0; i < populacja_pocz[k].Count(); i++)

                        {
                            for (node = populacja_pocz[k][i].sciezka; node != null; node = node.next)
                            {
                                Debug.Write(node.from + " -> ");

                            }
                            Debug.Write(odbiorcy[0] + "  " + populacja_pocz[k][i].koszt + "  " + populacja_pocz[k][i].delay);
                            Debug.WriteLine("");
                        }
                    }
                    Debug.WriteLine("Koniec_inicjacji");
                    break;
                
                case 2:


                    int rand2 = 0;
               
                    for (int i = 1; i < odbiorcy.Count(); i++)
                    {

                        l_tab_r.Add(Generowanie_Dijkstra(graf, ile_sciezek, odbiorcy[0], odbiorcy[i]));
                        
                    }
                    for (int j = 0; j < p_pocz; j++)
                    {
                        chromosom = new List<AG>(odbiorcy.Count()-1);

                        for (int i = 0; i < odbiorcy.Count() - 1; i++)
                        {
                            rand2 = x.Next(0, l_tab_r[i].Count());
                            chromosom.Add(l_tab_r[i][rand2]);
                        }

                        populacja_pocz.Add(chromosom);
                    }

                      siec node1;

                    for (int k = 0; k < populacja_pocz.Count(); k++)
                    {
                        Debug.WriteLine("");
                        Debug.WriteLine("Osobnik ("+k+")");
                        for (int i = 0; i < populacja_pocz[k].Count(); i++)

                        {
                            for (node1 = populacja_pocz[k][i].sciezka; node1 != null; node1 = node1.next)
                            {
                                Debug.Write(node1.to + " -> ");
                                if (node1.next == null) { Debug.Write(node1.from); }

                            }
                            Debug.Write("  " + populacja_pocz[k][i].koszt + "  " + populacja_pocz[k][i].delay);
                            Debug.WriteLine("");
                        }
                    }  
                        Debug.WriteLine("Koniec_inicjacji");

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

        
        public List<AG> Generowanie_BFS(siec[] graf, int ile_sciezek, int nadawca, int odbiorca)
        {
            List<siec> tab = new List<siec>();               
            List<AG> tab_r = new List<AG>();               //tablica routingu
            bool[] vis = new bool[graf.Count()];              //kontrolka
            List<int> kolejka = new List<int>();             //kolejka
            List<int> gen = new List<int>();                //scieżka
            siec node =new siec();                         //krawędź
            siec nw ;
            int random = 0;
            int koniec = nadawca;
            int wczesniej = nadawca;
            siec kopia;

            

            int licz = 0, licz2=0; 
            while(licz2 <ile_sciezek)
            {
                kolejka.Clear();
                tab.Add(null);

                gen.Clear();
                gen.Add(nadawca);
                
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
           
                
               

                

                int j = 0;
                for (j = 1; j < gen.Count();j++ )
                {
                    


                    for (nw = graf[gen[j]]; nw != null; nw = nw.next)
                    {
                        if (nw.from == gen[j] && nw.to == gen[j - 1])
                        {
                            kopia = new siec(nw);
                            node = kopia;
                            node.next = tab[licz];
                            tab[licz] = node;
                        }
                    }
                 }

              
                if (licz != 0)
                {
                   
                    
                    for (int i = 0; i < tab.Count(); i++)
                    {
                        nw = tab[licz];
                        
                        if (i != licz && nw !=null )
                        {
                            kopia = tab[i];
                            if (kopia != null)
                            {

                                while (kopia != null && nw !=null && kopia.to == nw.to )
                                {
                                    nw = nw.next;
                                    kopia = kopia.next;
                                    
                                    if (nw == null && kopia == null)
                                    {
                                        tab.RemoveAt(licz);
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
            double koszt, opoznienie ;
            
            siec pomoc;
            AG sciezka;

            for (int k = 0; k < tab.Count();k++ )
            {
                koszt = 0;
                opoznienie = 0;
                kopia = siec.DeepCopy(tab[k]);
                for (pomoc = kopia; pomoc != null; pomoc = pomoc.next)
                {
                    koszt = koszt + pomoc.cost;
                    opoznienie = opoznienie + pomoc.delay;
                }

                sciezka = new AG(kopia, koszt, opoznienie);
                tab_r.Add(sciezka);

            }


                return tab_r;

        }

        public List<AG> Generowanie_Dijkstra(siec[] graf, int ile_sciezek, int nadawca, int odbiorca) 
        {
            List<AG> tab_r = new List<AG>();               //tablica routingu
            List<AG> stos = new List<AG>();                  // stos z potencjalnymi najkrutszymi ściezkami
            List<siec> removed_node = new List<siec>();       //usuniete krawędzie
            siec dijkastra = new siec();
            siec pomoc, pomoc2, pomoc3,pomoc4;
            List<int> root_path;
            siec total_path =null, spur_path;                                //suma root_path i spur_path
            siec spur_node;
            int dlugosc_sciezki, usuwanie_nadawcy;
            double koszt, opoznienie;
            AG sciezka;
            int sprawdzenie;

            siec[] kopia_graf = siec.DeepCopy(graf);
            koszt = 0;
            opoznienie = 0;

            pomoc4=dijkastra.sciezka(nadawca, odbiorca, graf.Count(), graf, "cost"); //obliczenie najkrutszej ścieżki

            for (pomoc3 = pomoc4; pomoc3 != null; pomoc3 = pomoc3.next)
            {
                koszt = koszt + pomoc3.cost;
                opoznienie = opoznienie + pomoc3.delay;
            }
            
            sciezka = new AG(pomoc4, koszt, opoznienie);
            tab_r.Add(sciezka);

            for (int k = 1; k < ile_sciezek; k++)//główny for ile sciezek trzeba wyznaczyć
                {
                    dlugosc_sciezki = 0;
                    root_path = new List<int>();            //sciezka                   
                    spur_node = tab_r[k - 1].sciezka;              //nowy wiezchołek startowy  
                    root_path.Add(spur_node.to);
                    usuwanie_nadawcy = 0;
                    removed_node.Clear();

                    for (pomoc = tab_r[k - 1].sciezka; pomoc != null; pomoc = pomoc.next)//liczy ilość węzłów w ścieżce 
                    {
                        dlugosc_sciezki++; 
                    }

                    for(int i =0; i<dlugosc_sciezki; i++)//
                    {


                        
                        for (int c = 0; c < tab_r.Count(); c++) //usuwa wszystkie krawędzie należące do tab_r z węzłem startowym spur_node z graf
                        {

                            for (pomoc2 = tab_r[c].sciezka; pomoc2 != null; pomoc2 = pomoc2.next)
                            {
                                

                                if (spur_node.to == pomoc2.to) // && pour_node.from !=pomoc2.from 
                                {

                                    for (pomoc = kopia_graf[pomoc2.from]; pomoc != null; pomoc = pomoc.next) 
                                    {

                                        if (spur_node.to == pomoc.to && pomoc.from == pomoc2.from)
                                        {
                                            if (pomoc.before == null)
                                            {
                                                kopia_graf[pomoc2.from] = pomoc.next;
                                                if (kopia_graf[pomoc2.from] != null && kopia_graf[pomoc2.from].before != null)
                                                {
                                                    kopia_graf[pomoc2.from].before = null;
                                                }
                                                break;
                                            }
                                            else
                                            {
                                                pomoc.before.next = pomoc.next;
                                                if (pomoc.next != null)
                                                {
                                                    pomoc.next.before = pomoc.before;
                                                }
                                                break;
                                            }

                                        }


                                    } //for

                                    for (pomoc = kopia_graf[pomoc2.to]; pomoc != null; pomoc = pomoc.next)
                                    {

                                        if (pomoc2.to == pomoc.from && pomoc2.from == pomoc.to)
                                        {
                                            if (pomoc.before == null) 
                                            {
                                                kopia_graf[pomoc2.to] = pomoc.next;
                                                if (kopia_graf[pomoc2.to] != null && kopia_graf[pomoc2.to].before != null)
                                                {
                                                    kopia_graf[pomoc2.to].before = null;
                                                }
                                                break;
                                            }
                                            else
                                            {
                                                pomoc.before.next = pomoc.next;
                                                if (pomoc.next != null)
                                                {
                                                    pomoc.next.before = pomoc.before;
                                                }
                                                break;
                                            }
                                        }

                                    } //for

                                }

                                if (i == 1&& usuwanie_nadawcy ==0)
                                {
                                    int kontrolka = 0;
                                    kopia_graf[nadawca] = null;
                                    usuwanie_nadawcy = 1;
                                    while (kontrolka < kopia_graf.Count())
                                    {
                                        

                                        for (pomoc = kopia_graf[kontrolka]; pomoc != null; pomoc = pomoc.next)
                                        {

                                            if (nadawca == pomoc.to)
                                            {
                                                if (pomoc.before == null)
                                                {
                                                    kopia_graf[kontrolka] = pomoc.next;
                                                    if ( kopia_graf[kontrolka]!= null && kopia_graf[kontrolka].before != null)
                                                    {
                                                        kopia_graf[kontrolka].before = null;
                                                    }
                                                    break;
                                                }
                                                else
                                                {
                                                    

                                                    pomoc.before.next = pomoc.next;
                                                    if (pomoc.next != null)
                                                    {
                                                        pomoc.next.before = pomoc.before;
                                                    }
                                                    break;

                                                }
                                            }

                                        }
                                        kontrolka++;
                                    }
                                }
                            }
                         }//koniec for usuwa


                        koszt = 0;
                        opoznienie = 0;
                        if (i == 0)
                        {
                            sciezka = new AG(dijkastra.sciezka(spur_node.to, odbiorca, kopia_graf.Count(), kopia_graf, "cost"), 0, 0);

                            if (sciezka.sciezka != null)
                            {
                                for (pomoc3 = sciezka.sciezka; pomoc3 != null; pomoc3 = pomoc3.next)
                                {
                                    koszt = koszt + pomoc3.cost;
                                    opoznienie = opoznienie + pomoc3.delay;
                                }
                                sciezka.koszt = koszt;
                                sciezka.delay = opoznienie;

                                stos.Add(sciezka);
                            }
                           
                            
                        }
                        else
                        {
                            
                            total_path = siec.DeepCopy(tab_r[k - 1].sciezka);

                            for (spur_path=total_path; spur_path.from != spur_node.to; spur_path = spur_path.next)
                            {
 
                            }
                            pomoc4=dijkastra.sciezka(spur_node.to, odbiorca, kopia_graf.Count(), kopia_graf, "cost");
                            spur_path.next = pomoc4;



                            if (spur_path.next == null)
                            {
                                break;
                            }
                            else
                            {
                                for (pomoc3 = total_path; pomoc3 != null; pomoc3 = pomoc3.next)
                                {
                                    koszt = koszt + pomoc3.cost;
                                    opoznienie = opoznienie + pomoc3.delay;
                                }
                                sciezka = new AG(total_path, koszt, opoznienie);
                                stos.Add(sciezka);
                            }
                                                        
                        }


                        

                        spur_node = spur_node.next;
                        if(spur_node!=null)
                        root_path.Add(spur_node.to);
                       // kopia_graf = siec.DeepCopy(kopia_graf);
                        
                    }

                    kopia_graf = siec.DeepCopy(graf); //przywrucenie własności grafu
                    if (stos.Count() == 0)
                    {
                        break;
                    }
                    
                    
                    stos = stos.OrderBy(o => o.koszt).ToList();

                  
                  do
                  {
                      sprawdzenie = 0;
                      for (int spr = 0; spr < tab_r.Count(); spr++)
                      {
                          pomoc = tab_r[spr].sciezka;
                          if (stos.Count() != 0)
                          {
                              for (pomoc2 = stos.FirstOrDefault().sciezka; pomoc2 != null; pomoc2 = pomoc2.next)
                              {
                                  if (pomoc.to == pomoc2.to && pomoc.from == pomoc2.from)
                                  {
                                      if (pomoc.next == pomoc2.next && pomoc.next == null && pomoc2.next == null)
                                      {
                                          stos.Remove(stos.FirstOrDefault());
                                          sprawdzenie = 1;
                                          
                                          break;
                                      }

                                  }else
                                  {
                                      break;
                                  }

                                  pomoc = pomoc.next;
                                  if (pomoc == null)
                                  {
                                      break;
                                  }
                              }
                          }
                          else
                          {
                              break;
                          }
                          if(sprawdzenie == 1)
                          {
                              
                              break;
                          }

                      }

                  } while (sprawdzenie == 1);

                  if (stos.Count() == 0)
                  {
                      break;
                  }
                    tab_r.Add(stos.FirstOrDefault());
                    stos.Remove(stos.FirstOrDefault());
                }//for główny

                return tab_r;
        }
    
        public void ocena_przyst()
        {

        }

    }
    
}
