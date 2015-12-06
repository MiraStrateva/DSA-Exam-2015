namespace OfficeSpace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Program
    {
        public class Task
        {
            public Task(int number, int time)
            {
                this.Number = number;
                this.Time = time;
                this.waitFor = new List<Task>();
                this.waitedFrom = new List<Task>();
            }

            public int Number { get; set; }
            public int Time { get; set; }
            public List<Task> waitFor { get; set; }
            public List<Task> waitedFrom { get; set; }
        }

        static void Main()
        {
            int n = int.Parse(Console.ReadLine());
            Task[] tasks = new Task[n + 1];
            Task[] tasksToS = new Task[n + 1];
            var taskTimes = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            for (int i = 0; i < n; i++)
            {
                var task = new Task(i + 1, taskTimes[i]);
                tasks[i + 1] = task;
                var task1 = new Task(i + 1, taskTimes[i]);
                tasksToS[i + 1] = task1;
            }

            for (int i = 1; i <= n; i++)
            {
                int[] dependences = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();

                foreach (var d in dependences)
                {
                    if (d != 0)
                    {
                        tasks[i].waitFor.Add(tasks[d]);
                        tasks[d].waitedFrom.Add(tasks[i]);

                        tasksToS[i].waitFor.Add(tasksToS[d]);
                        tasksToS[d].waitedFrom.Add(tasksToS[i]);
                    }
                }
            }

            Stack<int> sorted = Sort(tasksToS);

            if (sorted.Count == 0)
            {
                Console.WriteLine(-1);
            }
            else
            {
                Dictionary<int, int> taskTime = new Dictionary<int, int>();
                while (sorted.Count > 0)
                {
                    var current = sorted.Pop();

                    int wT = 0;
                    foreach (var item in tasks[current].waitFor)
                    {
                        if (taskTime[item.Number] > wT)
                        {
                            wT = taskTime[item.Number];
                        }
                    }

                    taskTime.Add(current, wT + tasks[current].Time);
                }

                int maxTime = 0;
                foreach (var item in taskTime)
                {
                    if (item.Value > maxTime)
                    {
                        maxTime = item.Value;
                    }
                }
                Console.WriteLine(maxTime);
            }
        }

        static Stack<int> Sort(Task[] arr)
        {
            Stack<int> sorted = new Stack<int>();   //            L ← Empty list that will contain the sorted elements
            
            List<Task> noIncom = new List<Task>();  // S ← Set of all nodes with no incoming edges
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i].waitedFrom.Count == 0)
                {
                    noIncom.Add(arr[i]);
                }
            }

            while (noIncom.Count > 0)               // while S is non-empty do
            {
                Task n = noIncom.First();
                noIncom.Remove(n);              // remove a node n from S
                sorted.Push(n.Number);                  //    insert n into L

                List<Task> wF = new List<Task>(n.waitFor);
                
                foreach (var m in wF)    //    for each node m with an edge e from n to m do
                {
                    n.waitFor.Remove(m);        //        remove edge e from the graph
                    m.waitedFrom.Remove(n);

                    if (m.waitedFrom.Count == 0)//        if m has no other incoming edges then
                    {
                        noIncom.Add(m);          //            insert m into S
                    }
                }
            }

//if graph has edges then
//    return error (graph has at least one cycle)
//else 
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i].waitedFrom.Count != 0 || arr[i].waitFor.Count != 0)
                {
                    sorted.Clear();
                }
            }

            return sorted;      //    return L (a topologically sorted order)
        }
    }
}
