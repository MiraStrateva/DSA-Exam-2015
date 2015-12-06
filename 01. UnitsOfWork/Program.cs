namespace UnitsOfWork
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Program
    {
        public class Unit : IComparable<Unit>
        {
            public Unit(string name, string type, int attack)
            {
                this.Name = name;
                this.Type = type;
                this.Attack = attack;
            }

            public string Name { get; set; }
            public string Type { get; set; }
            public int Attack { get; set; }

            public int CompareTo(Unit other)
            {
                if (other == null)
                {
                    return 1; 
                }

                if (this.Attack == other.Attack)
                {
                    return this.Name.CompareTo(other.Name);
                }
                else
                {
                    if (this.Attack > other.Attack)
                    {
                        return -1;
                    }
                    else if (this.Attack == other.Attack)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }

            public override string ToString()
            {
                return string.Format("{0}[{1}]({2})", this.Name, this.Type, this.Attack);
            }
        }

        private static void Main()
        {
            StringBuilder sb = new StringBuilder();
            Dictionary<string, SortedSet<Unit>> unitsByType = new Dictionary<string,SortedSet<Unit>>();
            SortedSet<Unit> rank = new SortedSet<Unit>();
            Dictionary<string, Unit> unitsByName = new Dictionary<string, Unit>(); 

            while (true)
            {
                var line = Console.ReadLine();

                if (line == "end")
                {
                    break;
                }

                string command = line.Substring(0, line.IndexOf(' '));
                string[] param = line.Substring(line.IndexOf(' ') + 1).Split(' ');


                if (command == "add")
                {
                    var newUnit = new Unit(param[0], param[1], int.Parse(param[2]));

                    if (!unitsByType.ContainsKey(param[1]))
                    {
                        unitsByType.Add(param[1], new SortedSet<Unit>());
                    }

                    if (unitsByType[param[1]].Contains(newUnit))
                    {
                        sb.AppendLine(string.Format("FAIL: {0} already exists!", param[0]));
                    }
                    else
                    {
                        unitsByType[param[1]].Add(newUnit);
                        rank.Add(newUnit);
                        unitsByName.Add(param[0], newUnit);
                        sb.AppendLine(string.Format("SUCCESS: {0} added!", param[0]));
                    }
                }
                else if (command == "remove")
                {
                    if (!unitsByName.ContainsKey(param[0]))
                    {
                        sb.AppendLine(string.Format("FAIL: {0} could not be found!", param[0]));
                    }
                    else
                    {
                        var toR = unitsByName[param[0]];
                        unitsByName.Remove(param[0]);
                        unitsByType[toR.Type].Remove(toR);
                        rank.Remove(toR);
                        sb.AppendLine(string.Format("SUCCESS: {0} removed!", param[0]));
                    }
                }
                else if (command == "find")
                {
                    sb.Append("RESULT: ");
                    if (unitsByType.ContainsKey(param[0]))
                    {
                        int count = 0;
                        foreach (var unit in unitsByType[param[0]])
                        {
                            count++;

                            sb.Append(unit.ToString());

                            if (count == 10)
                            {
                                break;
                            }

                            if (count < unitsByType[param[0]].Count)
                            {
                                sb.Append(", ");
                            }
                        }
                    }
                    sb.AppendLine();
                }
                else if (command == "power")
                {
                    int power = int.Parse(param[0]);
                    sb.Append("RESULT: ");

                    int count = 0;
                    foreach (var unit in rank)
                    {
                        count++;

                        sb.Append(unit.ToString());

                        if (count == power)
                        {
                            break;
                        }

                        if (count < rank.Count)
                        {
                            sb.Append(", ");
                        }
                    }

                    sb.AppendLine();
                }
                
            }

            Console.WriteLine(sb.ToString());
        }
    }
}
