using Dijkstra.Algorithm.Graphing;
using Dijkstra.Algorithm.Pathing;
using Path = Dijkstra.Algorithm.Pathing.Path;

namespace Yens.Algorithm
{
    public class Yen
    {
        private GraphBuilder graphBuilder;
        private Graph graph;
        public Yen(GraphBuilder graphBuilder)
        {
            this.graphBuilder = graphBuilder;
            this.graph = graphBuilder.Build();
        }

        public List<Path> GetPaths(string sourceLabel, string targetLabel, int K)
        {
            var ksp = new List<Path>();
            var candidates = new Queue<Path>();

            Path kthPath = graph.Dijkstra(sourceLabel, targetLabel);
            ksp.Add(kthPath);

            for (int k = 1; k < K; k++)
            {
                Path previousPath = ksp[k - 1];

                for (int i = 0; i < previousPath.Segments.Count; i++)
                {
                    var removedEdges = new List<PathSegment>();
                    string spurNode = previousPath.Segments[i].Origin.Id;
                    var rootPath = previousPath.CloneTo(i);

                    foreach (var p in ksp)
                    {
                        Path stub = p.CloneTo(i);

                        if (rootPath.IsEqualsTo(stub))
                        {
                            var re = p.Segments[i];
                            try
                            {
                                graph = graphBuilder
                                    .RemoveLink(re.Origin.Id, re.Destination.Id)
                                    .Build();
                                removedEdges.Add(re);
                            } catch (Exception ex)
                            {
                                //Console.WriteLine(ex.Message);
                            }
                        }
                    }

                    foreach (var rootPathEdge in rootPath.Segments)
                    {
                        string rn = rootPathEdge.Origin.Id;
                        if (rn != spurNode)
                        {
                            if(graph.Nodes.Any(n => n.Id == rn))
                            {
                                graphBuilder.RemoveNode(rn);
                                var removedNode = graph.GetNode(rn);
                                var removedLinks = removedNode.Links
                                    .Select(link => new PathSegment(removedNode, link.Destination, link.Weight));

                                removedEdges.AddRange(removedLinks);
                                graph = graphBuilder.Build();
                            }
                        }
                    }

                    try
                    {
                        // Spur path = shortest path from spur node to target node in the reduced graph
                        Path spurPath = graph.Dijkstra(spurNode, targetLabel);

                        // Concatenate the root and spur paths to form the new candidate path
                        Path totalPath = rootPath.Clone();
                        spurPath.Segments.ToList().ForEach(e => totalPath.AddSegment(e));

                        // If candidate path has not been generated previously, add it
                        if (!candidates.Contains(totalPath))
                            candidates.Enqueue(totalPath);


                        // Restore all of the edges that were removed during this iteration
                        removedEdges.ForEach(e =>
                        {
                            if(!graph.Nodes.Any(node => e.Origin.Id == node.Id))
                            {
                                graphBuilder.AddNode(e.Origin.Id);
                            }

                            if (!graph.Nodes.Any(node => e.Destination.Id == node.Id))
                            {
                                graphBuilder.AddNode(e.Destination.Id);
                            }

                            graphBuilder.AddLink(e.Origin.Id, e.Destination.Id, e.Weight);
                        });
                        graph = graphBuilder.Build();
                    } catch (Exception ex)
                    {
                        //Console.WriteLine(ex.Message);
                    }

                }

                bool isNewPath;
                do
                {
                    if(candidates.Count == 0)
                    {
                        break;
                    }

                    kthPath = candidates.Dequeue();
                    isNewPath = true;
                    if (kthPath != null)
                    {
                        foreach (var p in ksp)
                        {
                            // Check to see if this candidate path duplicates a previously found path
                            if (p.IsEqualsTo(kthPath))
                            {
                                isNewPath = false;
                                break;
                            }
                        }
                    }
                } while (!isNewPath);

                // If there were not any more candidates, stop
                if (kthPath == null)
                    break;

                // Add the best, non-duplicate candidate identified as the k shortest path
                ksp.Add(kthPath);
            }

            return ksp;
        }
    }
}
