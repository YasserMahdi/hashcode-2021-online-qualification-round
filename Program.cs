using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace Hash2021
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Thread MainThread = new Thread(delegate() {
                var filePath = @"d:\hash\";

                foreach (string file in Directory.EnumerateFiles(filePath, "*.txt"))
                {
                    var name = Path.GetFileName(file);
                    string filename = @"d:\hash\solve\v6\" + name + " _" + DateTime.Now.TimeOfDay.Hours
                                                             + "-" + DateTime.Now.TimeOfDay.Minutes
                                                             + "-" + DateTime.Now.TimeOfDay.Seconds
                                                             + ".txt";

                    //Console.WriteLine("Hello World!");
                    //var filePath = @"d:\hash\" + name + ".txt";

                    var input = File.ReadAllText(filePath + name);
                    Console.WriteLine(name);
                    var InputList = input.Split('\n').ToList();

                    var FirstRow = InputList[0].Split(' ');

                    var timeTotal = int.Parse(FirstRow[0]);
                    var intersctionCount = int.Parse(FirstRow[1]);
                    var RoadsCount = int.Parse(FirstRow[2]);
                    var CarsCount = int.Parse(FirstRow[3]);
                    var BeniftCount = int.Parse(FirstRow[4]);

                    var usige = Enumerable.Repeat(0, RoadsCount).ToList();
                    var Roads = new List<Road>();
                    string[] data;
                    Thread t1 = new Thread(delegate ()
                    {

                        for (int i = 1; i <= RoadsCount; i++)
                        {
                            data = InputList[i].Split(' ');
                            Roads.Add(new Road
                            {
                                From = int.Parse(data[0]),
                                To = int.Parse(data[1]),
                                Name = data[2],
                                Time = int.Parse(data[0]),
                                index = i - 1
                            });
                        }
                    });
                    t1.Start();
                    t1.Join();
                    var Cars = new List<Car>();
                    var dataRoads = new List<string>();
                    Thread t2 = new Thread(delegate ()
                    {
                        for (int i = RoadsCount + 1; i < InputList.Count - 1; i++)
                        {
                            data = InputList[i].Split(' ');
                            //dataRoads = data.Skip(1).ToList();
                            dataRoads = data.Skip(1).ToList();

                            var TempPath = new List<Road>();
                            foreach (var item in dataRoads)
                            {
                                TempPath.Add(Roads.Where(r => r.Name == item).First());

                                TempPath.Last().isused = 1;
                                usige[TempPath.Last().index] = usige[TempPath.Last().index] + 1;

                            }

                            Cars.Add(new Car
                            {
                                NumberOfRoud = int.Parse(data[0]),
                                Roads = TempPath,
                                TimeAvg = TempPath.Sum(r => r.Time)


                                //Roads = dataRoads.Where(r => Roads.Select(rr => rr.Name).Contains(r)).ToList()
                            });
                        }
                    });
                    t2.Start();
                    t2.Join();
                    var intersctions = new List<Intersection>();
                    var tempIntersction = new Intersection();
                    for (int i = 0; i < intersctionCount; i++)
                    {
                        tempIntersction = new Intersection();
                        tempIntersction.Index = i;
                        tempIntersction.Income = Roads.Where(r => r.To == i).ToList();
                        tempIntersction.Throutcars = Cars.Where(c => tempIntersction.Income.Select(r => r.Name).Contains(c.Roads.Select(cc => cc.Name).First())).ToList();

                        intersctions.Add(tempIntersction);




                    }
                    var theFile = new StreamWriter(filename);
                    int TempCounter = 0;



                    //.WriteLine(WholeTotal);
                    //var CarsCount = 0;
                    int ISC = 0;
                    intersctions.ForEach(i =>
                    {
                        if (i.Income.Count == 1)
                        {
                            ISC++;
                            theFile.WriteLine(i.Index);
                            theFile.WriteLine(1);
                            theFile.WriteLine(i.Income.First().Name + " " + timeTotal);
                        }
                        else
                        {
                            ISC++;
                            theFile.WriteLine(i.Index);
                            theFile.WriteLine(i.Income.Count);
                            //foreach (var car in Cars)
                            //{
                            //    foreach (var Destination in car.Roads)
                            //    {
                            //        if (Destination.To == i.Index)
                            //        {
                            //            int TempSecond = car.Roads.Skip(1).Sum(s => s.Time);
                            //            if (TempSecond == 0)
                            //            {
                            //                i.CurrentS.Add(new seconds
                            //                {
                            //                    CurrentSecond = 0,
                            //                    Road = Destination.Name

                            //                });
                            //            }
                            //            else
                            //            {
                            //                i.CurrentS.Add(new seconds
                            //                {
                            //                    CurrentSecond = TempSecond,
                            //                    Road = Destination.Name

                            //                });
                            //            }
                            //        }
                            //    }
                            //}
                            foreach (var street in i.Income)
                            {

                                //if (street.isused == 1)
                                //{
                                //    if(usige[street.index] > timeTotal)
                                //    {
                                //        theFile.WriteLine(street.Name + " " + (timeTotal/usige[street.index]));

                                //    }
                                //    else
                                //    {
                                //        theFile.WriteLine(street.Name + " " + (timeTotal / usige[street.index]));

                                //    }

                                //}
                                //theFile.WriteLine(street.Name + " " + 1);

                                if (street.isused == 1)
                                {
                                    theFile.WriteLine(street.Name + " " + 3);


                                }
                                else
                                {
                                    theFile.WriteLine(street.Name + " " + 1);

                                }

                            }




                        }
                        //CarsCount = i.Income.Where(r => r.)
                    });

                    theFile.Close();
                    string str;
                    using (StreamReader sreader = new StreamReader(filename))
                    {
                        str = sreader.ReadToEnd();
                    }
                    File.Delete(filename);
                    using (StreamWriter swriter = new StreamWriter(filename, false))
                    {
                        str = ISC + Environment.NewLine + str;
                        swriter.Write(str);
                    }


                    Console.WriteLine(ISC.ToString());
                }
                
            });
            MainThread.Start();
            MainThread.Join();
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds.ToString());

        }

        public class Road
        {

            public int From { get; set; }
            public int To { get; set; }
            public string Name { get; set; }
            public int Time { get; set; }
            public int index { get; set; }

            public int isused { get; set; }
        }

        public class Car
        {
            public int NumberOfRoud { get; set; }
            public List<Road> Roads { get; set; }

            public int TimeAvg { get; set; }

        }

        public class Intersection
        {
            public int Index { get; set; }
            public List<Road> Income { get; set; }
            public List<Road> Outcome { get; set; }
            public List<Car> Throutcars { get; set; }
            public int IsUsed { get; set; }

            public List<seconds> CurrentS { get; set; }


        }

        public class seconds
        {
            public int CurrentSecond { get; set; }
            public string Road { get; set; }




        }

    }
}
