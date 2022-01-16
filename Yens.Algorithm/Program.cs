// See https://aka.ms/new-console-template for more information
using Dijkstra.Algorithm.Graphing;
using Dijkstra.Algorithm.Pathing;
using Yens.Algorithm;
using Path = Dijkstra.Algorithm.Pathing.Path;

var GenerateGraphBuilder = () =>
{
   var builder = new GraphBuilder();

   for (int i = 1; i <= 14; i++)
   {
       builder.AddNode(i.ToString());
   }

   builder.AddLink("1", "4", 1136)
       .AddLink("1", "8", 2828)
       .AddLink("1", "11", 1702)
       .AddLink("2", "3", 596)
       .AddLink("2", "5", 2349)
       .AddLink("2", "10", 789)
       .AddLink("3", "2", 596)
       .AddLink("3", "9", 366)
       .AddLink("3", "14", 385)
       .AddLink("4", "1", 1136)
       .AddLink("4", "5", 959)
       .AddLink("4", "11", 683)
       .AddLink("5", "2", 2349)
       .AddLink("5", "6", 573)
       .AddLink("5", "4", 959)
       .AddLink("6", "5", 573)
       .AddLink("6", "7", 732)
       .AddLink("6", "12", 1450)
       .AddLink("7", "6", 732)
       .AddLink("7", "8", 750)
       .AddLink("8", "1", 2828)
       .AddLink("8", "7", 750)
       .AddLink("8", "9", 706)
       .AddLink("9", "8", 706)
       .AddLink("9", "3", 366)
       .AddLink("9", "10", 451)
       .AddLink("9", "13", 839)
       .AddLink("10", "2", 789)
       .AddLink("10", "9", 451)
       .AddLink("10", "14", 246)
       .AddLink("11", "4", 683)
       .AddLink("11", "1", 1702)
       .AddLink("11", "12", 2049)
       .AddLink("12", "6", 1450)
       .AddLink("12", "11", 2049)
       .AddLink("12", "14", 1976)
       .AddLink("12", "13", 1128)
       .AddLink("13", "12", 1128)
       .AddLink("13", "9", 839)
       .AddLink("14", "12", 1976)
       .AddLink("14", "10", 246)
       .AddLink("14", "3", 385);

   return builder;
};

var yenPaths = new Dictionary<string, List<Path>>();

for (int i = 1; i <= 14; i++)
{
    for (int j = 1; j <= 14; j++)
    {
        if (i != j)
        {
            var builder = GenerateGraphBuilder();
            var yen = new Yen(builder);
            var paths = yen.GetPaths(i.ToString(), j.ToString(), 2);
            yenPaths.Add($"{i}->{j}", paths);
            Console.WriteLine($"{i}->{j}:");
            paths.ForEach(path =>
            {
                Console.Write($"\t{path.Segments[0].Origin.Id} -> {path.Segments[0].Destination.Id}");
                for (int i = 1; i < path.Segments.Count; i++)
                {
                    Console.Write($" -> {path.Segments[i].Destination.Id}");
                }                
                Console.WriteLine();
            });
        }
    }
}



Console.WriteLine(yenPaths);