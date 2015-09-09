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
    class osobnik //klasa wykozystana do przechowywania osobników w liście populacja
    {
        public List<gen> chromosom;
        public double przystosowanie;
        

        public osobnik(List<gen> chromosom, double koszt)
        {
            this.chromosom = chromosom;
            this.przystosowanie = koszt;
           
        }

        public osobnik()
        {

        }
    }
    #region
    
    #endregion
    class gen
    {
        public siec sciezka;    //sciezka od nadawcy do odbiorcy
        public double cost;     //koszt sciezki
        public double delay;    //opóźnienie
        public gen() { }
        public gen(siec sciezka, double koszt, double opoznienie)
        {
            this.sciezka = sciezka;
            this.cost = koszt;
            this.delay = opoznienie;
        }
    }
  
    class AG

    {    
        public bool algorytm_genetyczny(int p_pocz, Int16 m_generowania, Int16 m_selekcji, Int16 m_krzyzowania, double p_mutacji, siec[] graf, int[] odbiorcy, int ile_sciezek, int ile_pokolen, int delta )
        {
            System.IO.StreamWriter plik = System.IO.File.AppendText(@"AG.txt");
            System.IO.StreamWriter plikcsv = System.IO.File.AppendText(@"AG.csv");  
            List<List<gen>> l_tab_r = new List<List<gen>>();     //lista tablic routingu (baza genów)
            List<gen> drzewo ;                                  //drzewo transmisji multicas    
            List<osobnik> populacja = new List<osobnik>();     //populacja osobników
            osobnik chromosom;                                //chromosom (drzewo transmisji)
            List<int> wylosowani = new List<int>();          //lista wybranych do krzyżowania osobników  
            List<osobnik> dzieci = new List<osobnik>();     //lista dzieci
            List<osobnik> dzieci_nowe = new List<osobnik>();
            dzieci.Clear();
            l_tab_r.Clear();
            dzieci_nowe.Clear();

            ////////////////////////////// 1 Generowanie populacji poczatkowej//////////////////////////////////
            #region
            switch (m_generowania)
            {
                
                case 1://generuje tablice routingu przeszukując grf w szerz
                    
                    int rand1 = 0;
                    populacja.Clear();
                    
                    for (int i = 1; i < odbiorcy.Count(); i++)
                    {

                        l_tab_r.Add(Generowanie_BFS(graf, ile_sciezek, odbiorcy[0], odbiorcy[i])); 
                        
                    }
                    for (int j = 0; j < p_pocz; j++)
                    {
                        drzewo = new List<gen>(odbiorcy.Count()-1);

                        for (int i = 0; i < odbiorcy.Count() - 1; i++)
                        {

                            rand1 = Multi.Form1.x.Next(0, l_tab_r[i].Count());
                            drzewo.Add(siec.DeepCopy(l_tab_r[i][rand1]));
                        }
                        chromosom = new osobnik(drzewo, -1);
                        populacja.Add(chromosom);
                    }
                    
                  /*  
                    siec node;

                    for (int k = 0; k < populacja.Count(); k++)
                    {
                        Debug.WriteLine("");
                        Debug.WriteLine("Osobnik ("+k+")");
                        for (int i = 0; i < populacja[k].chromosom.Count(); i++)

                        {
                            for (node = populacja[k].chromosom[i].sciezka; node != null; node = node.next)
                            {
                                Debug.Write(node.from + " -> ");

                            }
                            Debug.Write(odbiorcy[0] + "  " + populacja[k].chromosom[i].cost + "  " + populacja[k].chromosom[i].delay);
                            Debug.WriteLine("");
                        }
                    }
                    Debug.WriteLine("Koniec_inicjacji");*/
                    break;
                
                case 2:
                    
                    populacja.Clear();
                    int rand2 = 0;
               
                    for (int i = 1; i < odbiorcy.Count(); i++)
                    {

                        l_tab_r.Add(Generowanie_YEN(graf, ile_sciezek, odbiorcy[0], odbiorcy[i]));
                        
                    }
                    for (int j = 0; j < p_pocz; j++)
                    {
                        drzewo = new List<gen>(odbiorcy.Count()-1);

                        for (int i = 0; i < odbiorcy.Count() - 1; i++)
                        {
                            rand2 = Multi.Form1.x.Next(0, l_tab_r[i].Count());
                            drzewo.Add(siec.DeepCopy(l_tab_r[i][rand2]));
                        }
                        chromosom = new osobnik(drzewo, -1);
                        populacja.Add(chromosom);
                    }

                  /*    siec node1;

                    for (int k = 0; k < populacja.Count(); k++)
                    {
                        Debug.WriteLine("");
                        Debug.WriteLine("Osobnik ("+k+")");
                        for (int i = 0; i < populacja[k].chromosom.Count(); i++)

                        {
                            for (node1 = populacja[k].chromosom[i].sciezka; node1 != null; node1 = node1.next)
                            {
                                Debug.Write(node1.to + " -> ");
                                if (node1.next == null) { Debug.Write(node1.from); }

                            }
                            Debug.Write("  " + populacja[k].chromosom[i].cost + "  " + populacja[k].chromosom[i].delay);
                            Debug.WriteLine("");
                        }
                    }  
                        Debug.WriteLine("Koniec_inicjacji");
                    */
                    break;
   

            }//Gen_populacji
            #endregion//

            ///////////////////////////////////2 ocena populacji ///////////////////////////////////////////////
            ocena_przyst(populacja, delta);
            
           // for (int i = 0; i < populacja.Count(); i++) { Debug.WriteLine(populacja[i].przystosowanie); }

           ///////////////////////////////// 3 selekcja osobnikow///////////////////////////////////////////////
            for (int ile_pok = 0; ile_pok < ile_pokolen; ile_pok++)
            {
                
                #region
                switch (m_selekcji)
                {
                    case 1:

                        wylosowani = selekcja_ruletka(populacja);
                        break;

                    case 2:

                        wylosowani = selekcja_turniej(populacja);
                        break;

                    case 3:
                        wylosowani = selekcja_ranking(populacja);
                        break;
                }//selekcja
                #endregion

                /////////////////////////////////// 4 krzyżowanie ///////////////////////////////////////////////////
                
                #region
                switch (m_krzyzowania)
                {
                    case 1:
                        dzieci.Clear();
                        dzieci = krzyzowanie_jednop(populacja, wylosowani, p_mutacji, l_tab_r);
                       
                        break;

                    case 2:
                        dzieci.Clear();
                        dzieci = krzyzowanie_dwup(populacja, wylosowani, p_mutacji, l_tab_r);

                        break;

                    case 3:
                        dzieci.Clear();
                        dzieci = krzyzowanie_rownomierne(populacja, wylosowani, p_mutacji, l_tab_r);

                        break;
                }//krzyżowanie


                //sukcesja

                #endregion

                /////////////////////////////// 5 redukcja duplikatów //////////////////////////////////////////////

                dzieci = redukcja_duplikatów(dzieci, l_tab_r);
                
                populacja = sukcesja(dzieci, populacja);
                
                ocena_przyst(populacja, delta);
               

            }/////główna pętla iteracja pokoleń

            populacja = populacja.OrderByDescending(o => o.przystosowanie).ToList();

            /////////////////////////////////////6 zapis wyniku /////////////////////////////////////////////////
            #region
            
             begin:
                
                
                double koszt = 0, opoznienie = 0; ;
                siec pomoc = new siec();
                siec pomoc2 = new siec();
                int n = Convert.ToInt16(graf.Count());
                siec[] lista_sciezek = new siec[odbiorcy.Count() - 1];

                bool[][] kontrolka1 = new bool[n][];
                for (int i = 0; i < n; i++)
                {
                    kontrolka1[i] = new bool[n];


                }

                //zerowanie kontrolki
                for (int f = 0; f < n; f++)
                {
                    for (int h = 0; h < n; h++)
                    {
                        kontrolka1[f][h] = false;
                    }
                }

                siec[] mst = new siec[n];
                siec nowy2;

                for (int i = 0; i < n; i++)
                {
                    mst[i] = null;
                }

                for (int i = 0; i < odbiorcy.Count() - 1; i++)
                {
                    for (pomoc2 = populacja[0].chromosom[i].sciezka; pomoc2 != null; pomoc2 = pomoc2.next)
                    {
                        if (kontrolka1[pomoc2.from][pomoc2.to] != true && kontrolka1[pomoc2.to][pomoc2.from] != true)
                        {

                            nowy2 = new siec(pomoc2.cost, pomoc2.delay, pomoc2.from, pomoc2.to, pomoc2.id, mst[pomoc2.to]); //tworzenie nowej krawedzi
                            mst[pomoc2.to] = nowy2;
                            nowy2 = new siec(pomoc2.cost, pomoc2.delay, pomoc2.to, pomoc2.from, pomoc2.id, mst[pomoc2.from]);
                            mst[pomoc2.from] = nowy2;
                            kontrolka1[pomoc2.to][pomoc2.from] = true;
                            kontrolka1[pomoc2.from][pomoc2.to] = true;

                        }
                    }
                }





                int sc = 0;
                for (int j = 1; j < odbiorcy.Count(); j++)
                {


                    lista_sciezek[sc] = pomoc.sciezka(odbiorcy[0], odbiorcy[j], n, mst, "cost");		//obliczam sciezk     
                    if (lista_sciezek[sc] == null)
                    {
                        plikcsv.WriteLine("zly wynik " + odbiorcy[j]);
                        

                        populacja.RemoveAt(0);
                        
                        goto begin;

                    }
                    sc++;
                }



                for (int i = 0; i < n; i++)
                {
                    kontrolka1[i] = new bool[n];


                }



                siec[] mst2 = new siec[n];
                for (int i = 0; i < n; i++)
                {
                    mst2[i] = null;
                }

                for (int i = 0; i < lista_sciezek.Count(); i++)
                {
                    for (pomoc2 = lista_sciezek[i]; pomoc2 != null; pomoc2 = pomoc2.next)
                    {
                        if (kontrolka1[pomoc2.from][pomoc2.to] != true && kontrolka1[pomoc2.to][pomoc2.from] != true)
                        {

                            nowy2 = new siec(pomoc2.cost, pomoc2.delay, pomoc2.from, pomoc2.to, pomoc2.id, mst2[pomoc2.to]); //tworzenie nowej krawedzi
                            mst2[pomoc2.to] = nowy2;
                            nowy2 = new siec(pomoc2.cost, pomoc2.delay, pomoc2.to, pomoc2.from, pomoc2.id, mst2[pomoc2.from]);
                            mst2[pomoc2.from] = nowy2;
                            kontrolka1[pomoc2.to][pomoc2.from] = true;
                            kontrolka1[pomoc2.from][pomoc2.to] = true;

                        }
                    }
                }


                


                for (int f = 0; f < n; f++)
                { //zerowanie kontrolki
                    for (int h = 0; h < n; h++)
                    {
                        kontrolka1[f][h] = false;
                    }
                }

                for (int i = 0; i < n; i++) //oblicza calkowity koszt drzewa multicast
                {
                    pomoc = mst2[i];
                    if (pomoc != null)
                    {
                        while (pomoc != null)
                        {
                            if (kontrolka1[pomoc.from][pomoc.to] != true && kontrolka1[pomoc.to][pomoc.from] != true)
                            {
                                koszt = koszt + pomoc.cost;
                                opoznienie = opoznienie + pomoc.delay;
                                kontrolka1[pomoc.to][pomoc.from] = true;
                                kontrolka1[pomoc.from][pomoc.to] = true;
                            }
                            pomoc = pomoc.next;
                        }
                    }
                }


                Debug.Write("\n");
                Debug.Write("Koszt drzewa multicast wynosi = ");
                Debug.Write(koszt);
                Debug.Write("\n");
                Debug.Write("\n");

                Debug.Write("\n");
                Debug.Write("DRZEWO MULTICAST ");
                Debug.Write("\n");
                Debug.Write("\n");
                plik.WriteLine("\n" + "delta = " + delta + "\n");
                plik.WriteLine("\n" + "Koszt drzewa multicast wynosi = " + koszt + "\n");
                plikcsv.WriteLine(koszt);
                plik.WriteLine("\n" + "Nadawca = " + odbiorcy[0] + "\n");
                plik.Write("Odbiorcy = ");
                for (int i = 1; i < odbiorcy.Count(); i++)
                {
                    plik.Write(odbiorcy[i] + ", ");
                }
                plik.WriteLine("");
                plik.WriteLine("");
                plik.WriteLine("\n" + "DRZEWO MULTICAST " + "\n");

                for (int i = 0; i < n; i++) //wyswietla Lsonsiadow graf_kpp
                {
                    pomoc = mst2[i];
                    if (pomoc != null)
                    {
                        Debug.Write("DMT[");
                        Debug.Write(i);
                        Debug.Write("] =");

                        plik.Write("DMT[" + i + "] ->");

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

                
            #endregion


                Debug.WriteLine(koszt);
           
            plik.Close();
            plikcsv.Close();
            return true;
           // MessageBox.Show("Koszt drzewa transmisji grupowej wynosi:  " + Convert.ToString(koszt) + "  \n " + Convert.ToString(opoznienie));
        }//algorytm_genetyczny

        
        public List<gen> Generowanie_BFS(siec[] graf, int ile_sciezek, int nadawca, int odbiorca)
        {
            List<siec> tab = new List<siec>();               
            List<gen> tab_r = new List<gen>();               //tablica routingu
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

                    random = Multi.Form1.x.Next(0, kolejka.Count); //wylosowanie kolejnego allelu
                    node = graf[kolejka[random]];
                    koniec = kolejka[random];

                    gen.Add(kolejka[random]); //dodaj wylosowany wierzcholek do genu      

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
            gen sciezka;

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

                sciezka = new gen(kopia, koszt, opoznienie);
                tab_r.Add(sciezka);

            }


                return tab_r;

        } //Generowanie tablic routingu metoda k losowych ściezek
        public List<gen> Generowanie_YEN(siec[] graf, int ile_sciezek, int nadawca, int odbiorca) 
        {
            List<gen> tab_r = new List<gen>();               //tablica routingu
            List<gen> stos = new List<gen>();                  // stos z potencjalnymi najkrutszymi ściezkami
            List<siec> removed_node = new List<siec>();       //usuniete krawędzie
            siec dijkastra = new siec();
            siec pomoc, pomoc2, pomoc3,pomoc4;
            List<int> root_path;
            siec total_path =null, spur_path;                                //suma root_path i spur_path
            siec spur_node;
            int dlugosc_sciezki, usuwanie_nadawcy;
            double koszt, opoznienie;
            gen sciezka;
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
            
            sciezka = new gen(pomoc4, koszt, opoznienie);
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
                            sciezka = new gen(dijkastra.sciezka(spur_node.to, odbiorca, kopia_graf.Count(), kopia_graf, "cost"), 0, 0);

                            if (sciezka.sciezka != null)
                            {
                                for (pomoc3 = sciezka.sciezka; pomoc3 != null; pomoc3 = pomoc3.next)
                                {
                                    koszt = koszt + pomoc3.cost;
                                    opoznienie = opoznienie + pomoc3.delay;
                                }
                                sciezka.cost = koszt;
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
                                sciezka = new gen(total_path, koszt, opoznienie);
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
                    
                    stos = stos.OrderBy(o => o.cost).ToList();

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
        } //generowanie tablic routingu algorytmem k najkrutszych ścieżek YEN'A
        public void ocena_przyst(List<osobnik> populacja, int delta) 
        {
            double koszt, opoznienie;
            List<object[]> kontrolka = new List<object[]>();
            object[] rekord;
            siec pomoc1;
            bool flaga;
           // kontrolka.Add(new object[] { 1, 2 }); dodawanie wartości
            //rekord = kontrolka[0];  podstawienie żeby wyświetlić

            for(int k=0; k<populacja.Count(); k++)
            {
                koszt = 0;
                opoznienie = 0;
                kontrolka.Clear();

                for (int i = 0; i < populacja[k].chromosom.Count(); i++ )
                {
                    for (pomoc1 = populacja[k].chromosom[i].sciezka; pomoc1 != null; pomoc1 = pomoc1.next)
                    {
                        flaga = false;
                        for (int c = 0; c < kontrolka.Count(); c++) //sprawdzam czy dany link był juz sumowany
                        {
                            rekord = kontrolka[c];
                            if (Convert.ToInt16(rekord[0]) == pomoc1.from && Convert.ToInt16(rekord[1]) == pomoc1.to || Convert.ToInt16(rekord[1]) == pomoc1.from && Convert.ToInt16(rekord[0]) == pomoc1.to)
                            {
                                flaga = true; //jesli juz go sumowałem ustawiam flagę na true
                            }
                        }

                        if(flaga == false) //jesli nie sumowany to dodaje wartość
                        {
                            koszt = koszt + pomoc1.cost;
                            
                            kontrolka.Add(new object[] { pomoc1.from, pomoc1.to });
                        }

                        if (populacja[k].chromosom[i].delay <delta)
                            opoznienie = opoznienie + populacja[k].chromosom[i].delay*1000;
                        else
                            opoznienie = opoznienie + populacja[k].chromosom[i].delay*10000000;
                    }
                    
                }
                                
                    populacja[k].przystosowanie = (1 / koszt) + (1 / opoznienie);
                
                }

        } //oblicza przystosowanie f(x)=(4*(1/koszt) + (1/opoznienie))*100
        public List<int> selekcja_ruletka(List<osobnik> populacja)
        {
            List<int> wylosowani;
            double kolo = 0, praw, spr=0; 
            int a, b, it, j;
            
            List<double> prawdop = new List<double>();
            for (int i = 0; i < populacja.Count(); i++)
            {
                kolo = kolo + populacja[i].przystosowanie;
            }
            for (int i = 0; i < populacja.Count(); i++)
            {
                prawdop.Add((( (populacja[i].przystosowanie) / kolo)*100));
            }
            
            wylosowani = new List<int>(new int [populacja.Count()]);
            j = 1;
            for (int i = 0; i < prawdop.Count(); i=i+2)
            {
                a = 0; b = 0;
                do  
                {
                    a = Multi.Form1.x.Next(0, 100); //losowanie pierwszego rodzica
                    b = Multi.Form1.x.Next(0, 100);  //losowanie drugiego rodzica
                    it = 0;
                    praw = 0;
                    do
                    {

                        praw = praw + prawdop[it];
                        it++;
                    } while (praw < a && it < prawdop.Count()); //obliczanie 1 rodzica

                    wylosowani[i] = it - 1; //wylosowny - indeks chromosomu 1 w tablicy populacja wylosowanego do krzyzowania

                    it = 0;
                    praw = 0;
                    do
                    {
                        praw = praw + prawdop[it];
                        it++;
                    } while (praw < b && it < prawdop.Count()); //obliczanie drugiego rodzica

                    wylosowani[j] = it - 1;		//wylosowny - indeks chromosomu 2 w tablicy populacja wylosowanego do krzyzowania
                } while (wylosowani[i] == wylosowani[j]);
                

                j += 2;
            }

          return wylosowani;
        } //selekcja osobników metodą ruletki
        public List<int> selekcja_turniej(List<osobnik> populacja)
        { 
            List<int> wylosowani = new List<int>(new int[populacja.Count()]);
            int a,b,c,j;
            double[] los = new double[3];
            j = 1;
            for (int i = 0; i < populacja.Count(); i = i + 2) //turnieje 
            {
                
                do
                {
                    ////////////////////////////////////turniej 1 osobnik/////////////////////////////////////////
                    a = Multi.Form1.x.Next(0, populacja.Count());  //losowanie pierwszego osobnika do turnieju
                    b = Multi.Form1.x.Next(0, populacja.Count());  //losowanie drugiego osobnika do turnieju
                    c = Multi.Form1.x.Next(0, populacja.Count());  //losowanie drugiego osobnika do turnieju
                    los[0] = populacja[a].przystosowanie;
                    los[1] = populacja[b].przystosowanie;
                    los[2] = populacja[c].przystosowanie;

                    if (los.Min() == los[0]) //wybór osobnika 1
                    {
                        wylosowani[i] = a;
                    }
                    else if (los.Min() == los[1])
                    {
                        wylosowani[i] = b;
                    }
                    else if (los.Min() == los[2])
                    {
                        wylosowani[i] = c;
                    }

                    ////////////////////////////////////turniej 2 osobnik/////////////////////////////////////////

                    a = Multi.Form1.x.Next(0, populacja.Count());  //losowanie pierwszego osobnika do turnieju
                    b = Multi.Form1.x.Next(0, populacja.Count());  //losowanie drugiego osobnika do turnieju
                    c = Multi.Form1.x.Next(0, populacja.Count());  //losowanie drugiego osobnika do turnieju
                    los[0] = populacja[a].przystosowanie;
                    los[1] = populacja[b].przystosowanie;
                    los[2] = populacja[c].przystosowanie;

                    if (los.Min() == los[0])  //wybór osobnika 2
                    {
                        wylosowani[j] = a;
                    }
                    else if (los.Min() == los[1])
                    {
                        wylosowani[j] = b;
                    }
                    else if (los.Min() == los[2])
                    {
                        wylosowani[j] = c;
                    }

                } while (wylosowani[i] == wylosowani[j]);
                j += 2;

            }



            return wylosowani;
        } //selekcja osobników metodą turniejową
        public List<int> selekcja_ranking(List<osobnik> populacja)
        {
            List<int> wylosowani = new List<int>(new int[populacja.Count()]);

            populacja = populacja.OrderByDescending(o => o.przystosowanie).ToList(); //sortowanie populacji według przystosowania malejąco

            int a,b,j;
            j=1;
            for (int i = 0; i < populacja.Count(); i = i + 2) //turnieje 
            {

                do
                {
                    /////////////////////z lepiej przystosowanej połówki populacji wybór osobników do krzyzowania/////////////
                    a = Multi.Form1.x.Next(0, populacja.Count()/2);  //losowanie pierwszego osobnika
                    b = Multi.Form1.x.Next(0, populacja.Count()/2);  //losowanie drugiego osobnika

                    wylosowani[i]=a;
                    wylosowani[j]=b;

                } while (wylosowani[i] == wylosowani[j]);
                j += 2;

            }


            return wylosowani;
        } //selekcja metodą rankingową
        public List<osobnik> krzyzowanie_jednop(List<osobnik> populacja, List<int> wylosowani, double p_mutacji, List<List<gen>>l_tab_r ) 
        {
            List <osobnik> dzieci = new List<osobnik>();
            osobnik drzewo;           
            List<gen> dziecko1;
            List<gen> dziecko2; 
            int j, p_ciecia;

            dzieci.Clear();
           
            j=1;

            for (int i = 0; i < wylosowani.Count(); i += 2) //tworzenie nowej populacji
            {
                
                p_ciecia = Multi.Form1.x.Next(1, populacja[i].chromosom.Count()); //losowanie punktu cięcia
                dziecko1 = new List<gen>();
                dziecko2 = new List<gen>();
              
                for (int k = 0; k < populacja[i].chromosom.Count(); k++ )
                {
                   
                    if(k<p_ciecia)
                    {
                        
                        dziecko1.Add(siec.DeepCopy(populacja[wylosowani[i]].chromosom[k]));                  
                        dziecko2.Add(siec.DeepCopy(populacja[wylosowani[j]].chromosom[k]));
                    }

                    if(k>=p_ciecia)
                    {
                       
                        dziecko1.Add(siec.DeepCopy(populacja[wylosowani[j]].chromosom[k]));                       
                        dziecko2.Add(siec.DeepCopy(populacja[wylosowani[i]].chromosom[k]));
                    }
                    
                } //reprodukcja

                mutacja(p_mutacji, dziecko1, l_tab_r); //mutacja dziecko1
                drzewo = new osobnik(dziecko1, -1);
                dzieci.Add(drzewo);

                mutacja(p_mutacji, dziecko2, l_tab_r); //mutacja dziecko2
                drzewo = new osobnik(dziecko2, -1);
                dzieci.Add(drzewo);

               j += 2;

            }

           

            return dzieci;
        } //krzyrzowanie jednopunktowe
        public List<osobnik> krzyzowanie_dwup(List<osobnik> populacja, List<int> wylosowani, double p_mutacji, List<List<gen>> l_tab_r)
        {
            List <osobnik> dzieci = new List<osobnik>();
            osobnik drzewo;
           
            List<gen> dziecko1 ;
            List<gen> dziecko2 ;
            int j, p_ciecia1, p_ciecia2;
            dzieci.Clear();

            j=1;
            for (int i = 0; i < wylosowani.Count(); i += 2) //tworzenie nowej populacji
            {
                dziecko1 = new List<gen>();
                dziecko2 = new List<gen>();
                do
                {
                    p_ciecia1 = Multi.Form1.x.Next(1, populacja[i].chromosom.Count() );
                    p_ciecia2 = Multi.Form1.x.Next(1, populacja[i].chromosom.Count() );
                } while (p_ciecia1 == p_ciecia2 || p_ciecia1 > p_ciecia2);//losowanie punktów ciecia
                

                for (int k = 0; k < populacja[i].chromosom.Count(); k++)
                {

                    if (k < p_ciecia1)
                    {
                        dziecko1.Add(siec.DeepCopy(populacja[wylosowani[i]].chromosom[k]));
                        dziecko2.Add(siec.DeepCopy(populacja[wylosowani[j]].chromosom[k]));
                    }

                    if (k >= p_ciecia1 && k < p_ciecia2)
                    {
                        dziecko1.Add(siec.DeepCopy(populacja[wylosowani[j]].chromosom[k]));
                        dziecko2.Add(siec.DeepCopy(populacja[wylosowani[i]].chromosom[k]));
                    }


                    if (k >= p_ciecia2)
                    {
                        dziecko1.Add(siec.DeepCopy(populacja[wylosowani[i]].chromosom[k]));
                        dziecko2.Add(siec.DeepCopy(populacja[wylosowani[j]].chromosom[k]));
                    }
                } //reprodukcja

                mutacja(p_mutacji, dziecko1, l_tab_r); //mutacja dziecko1
                drzewo = new osobnik(dziecko1, -1);
                dzieci.Add(drzewo);

                mutacja(p_mutacji, dziecko2, l_tab_r); //mutacja dziecko1
                drzewo = new osobnik(dziecko2, -1);
                dzieci.Add(drzewo);

                j += 2;
            }

            return dzieci;
        } //krzyżowanie dwupunktowe
        public List<osobnik> krzyzowanie_rownomierne(List<osobnik> populacja, List<int> wylosowani, double p_mutacji, List<List<gen>> l_tab_r)
        {
            List <osobnik> dzieci = new List<osobnik>();
            osobnik drzewo;
            
            List<gen> dziecko1;
            List<gen> dziecko2;
            int j;
            List<int> wektor = new List<int>();
            dzieci.Clear();
                        
            j=1;
            for (int i = 0; i < wylosowani.Count(); i += 2) //tworzenie nowej populacji
            {
                dziecko1 = new List<gen>();
                dziecko2 = new List<gen>();
                wektor.Clear();

                for (int c = 0; c < populacja[i].chromosom.Count(); c++)
                {
                    wektor.Add(Multi.Form1.x.Next(0,2));
                }

                for (int k = 0; k < populacja[i].chromosom.Count(); k++) //reprodukcja
                {
                    if(wektor[k] == 0)
                    {
                        dziecko1.Add(siec.DeepCopy(populacja[wylosowani[j]].chromosom[k]));
                        dziecko2.Add(siec.DeepCopy(populacja[wylosowani[i]].chromosom[k]));

                    }
                    else if (wektor[k] == 1)
                    {
                        dziecko1.Add(siec.DeepCopy(populacja[wylosowani[i]].chromosom[k]));
                        dziecko2.Add(siec.DeepCopy(populacja[wylosowani[j]].chromosom[k]));
                    }


                }//reprodukcja

                mutacja(p_mutacji, dziecko1, l_tab_r); //mutacja dziecko1
                drzewo = new osobnik(dziecko1, -1); //dodanie dziecka do populacji
                dzieci.Add(drzewo);

                mutacja(p_mutacji, dziecko1, l_tab_r); //mutacja dziecko1 
                drzewo = new osobnik(dziecko2, -1);//dodanie dziecka do populacji
                dzieci.Add(drzewo);

                j += 2;
            }

            return dzieci;
        } //krzyzowanie rownomierne
        public void mutacja(double prawdop, List<gen> dzieco, List<List<gen>> tab_r)
        {
            prawdop =prawdop* 100;
            int m,m2,p,p2,l;
            m = Multi.Form1.x.Next(1, 101); //czy wystąpi mutacja
            gen scie;
            if(m<prawdop)
            {
                if (dzieco.Count() > 16) // losowanie ile mutacji wystąpi
                {
                    l = Multi.Form1.x.Next(1, 3);
                }
                else { l = 1; }

                if (l == 1) //jeśli 1 mutacja to
                {
                    m = Multi.Form1.x.Next(0, dzieco.Count()); //który gen mutuje
                    p = Multi.Form1.x.Next(0, tab_r[m].Count() );

                    
                    scie = new gen(tab_r[m][p].sciezka, tab_r[m][p].cost, tab_r[m][p].delay);
                    dzieco[m] = scie;
                }

                if (l == 2) //jeśli 1 mutacja to
                {
                    m = Multi.Form1.x.Next(0, dzieco.Count()); //który gen mutuje
                    p = Multi.Form1.x.Next(0, tab_r[m].Count());

                    m2 = Multi.Form1.x.Next(0, dzieco.Count()); //który gen mutuje
                    p2 = Multi.Form1.x.Next(0, tab_r[m2].Count());
                    
                    scie = new gen(tab_r[m][p].sciezka, tab_r[m][p].cost, tab_r[m][p].delay);
                    dzieco[m] = scie;
                    scie = new gen(tab_r[m2][p2].sciezka, tab_r[m2][p2].cost, tab_r[m2][p2].delay);
                    dzieco[m2] = scie;
                }
            }

        } //mutacja 
        public List<osobnik> redukcja_duplikatów(List<osobnik> populacja_do_spr, List<List<gen>> l_tab_r)
        {
            List<osobnik> dzieci_nowe = new List<osobnik>();
            osobnik nowy;   
            siec gen1,gen2;
            int drzewo;

            for (int c = 0; c < populacja_do_spr.Count(); c++ )
            {
                dzieci_nowe.Add(null);
            }

           

                for (int i = 0; i < populacja_do_spr.Count(); i++) //porównanie wszystkich dzieci
                {
                    

                    for (int j = i+1; j < populacja_do_spr.Count(); j++)//porównanie ze innymi dziećmi
                    {
                        if (i != j)//nie porównuje do samego siebie
                        {
                            drzewo = 0;
                            for (int k = 0; k < populacja_do_spr[i].chromosom.Count(); k++) //porównywanie osobników
                            {
                                gen1 = populacja_do_spr[i].chromosom[k].sciezka;
                                gen2 = populacja_do_spr[j].chromosom[k].sciezka;
                                
                                if (gen1.from == gen2.from && gen1.to == gen2.to)
                                {
                                    for (; gen1 != null; gen1 = gen1.next)
                                    {
                                        if (gen1.from == gen2.from && gen1.to == gen2.to)
                                        {
                                            if (gen2 != null)
                                            {
                                                gen2 = gen2.next;
                                            }
                                        }
                                        else
                                        {
                                            break; 
                                        }
                                    }

                                  drzewo++;
                                }
                                else
                                {
                                    break;
                                }
                                
                                
                            }


                            if (drzewo == populacja_do_spr[i].chromosom.Count())
                            {
                                List<gen> chromos = new List<gen>();
                                nowy = new osobnik(chromos, -1);

                                for (int z = 0; z < populacja_do_spr[i].chromosom.Count(); z++)
                                {
                                    
                                    nowy.chromosom.Add(siec.DeepCopy(l_tab_r[z][Multi.Form1.x.Next(0, l_tab_r[z].Count())]));
                                }

                               
                                dzieci_nowe[j] = nowy;
                           //     Debug.WriteLine("redukcja_duplikatów " + i+ " " +j);
                            }
                            
                        }
                    }

                }//for - porównanie wszystkich dzieci

                for (int c = 0; c < populacja_do_spr.Count(); c++)
                {
                    if (dzieci_nowe[c] == null)
                        dzieci_nowe[c] = siec.DeepCopy(populacja_do_spr[c]);
                }

               
            return dzieci_nowe;
        } //redukcja duplikatów
        public List<osobnik> sukcesja(List<osobnik> dzieci, List<osobnik>populacja)
        {
            List<osobnik> nowa_populacja = new List<osobnik>();
            siec gen1, gen2;
            int drzewo;

            populacja = populacja.OrderByDescending(o => o.przystosowanie).ToList(); //sortowanie populacji rodziców malejąco
   
            //////////////////////////////////usuń z populacji pierwotnej osobniki podobne do potmnych////////////////////

            for (int i = 0; i < dzieci.Count(); i++) //porównanie wszystkich dzieci
            {
                for (int j = 0; j < populacja.Count(); j++)//porównanie z rodzicami
                {
                    drzewo = 0;
                    for (int k = 0; k < populacja[j].chromosom.Count(); k++) //porównywanie osobników
                    {
                        gen1 = dzieci[i].chromosom[k].sciezka;
                        gen2 = populacja[j].chromosom[k].sciezka;

                        if (gen1.from == gen2.from && gen1.to == gen2.to)
                        {
                            for (; gen1 != null; gen1 = gen1.next)
                            {
                                if (gen1.from == gen2.from && gen1.to == gen2.to)
                                {
                                    if (gen2 != null)
                                    {
                                        gen2 = gen2.next;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                            drzewo++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (drzewo == populacja[j].chromosom.Count())
                    {
                        populacja.RemoveAt(j);
                      
                    }
                }
            }/////////////////////usuń duplikaty//////////

            nowa_populacja = siec.DeepCopy(dzieci); //dodaje dzieci do nowej populacji


            ////////////////////////dodaje 0, 2 lub 4 rózniących się od dzieci rodziców do nowej populacji//////////////
            int dodaj=0;
            if (populacja.Count() >= 2)
            {
                if (populacja.Count() > 3)
                {
                    while (dodaj < 4 && dodaj < populacja.Count())
                    {
                        nowa_populacja.Add(siec.DeepCopy(populacja[0]));
                        dodaj++;
                       
                    }
                }
                else
                {
                    while (dodaj < 2 && dodaj < populacja.Count())
                    {

                        nowa_populacja.Add(siec.DeepCopy(populacja[0]));
                        dodaj++;
                       
                    }
                }

            }

            return nowa_populacja;
        }//tworzenie nowej pipulacji

    }
    
}
