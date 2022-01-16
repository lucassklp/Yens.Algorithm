using Dijkstra.Algorithm.Pathing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Path = Dijkstra.Algorithm.Pathing.Path;

namespace Yens.Algorithm
{
    internal static class GraphExtensions
    {

        public static PathSegment Clone(this PathSegment pathSegment)
        {
            return new PathSegment(pathSegment.Origin, pathSegment.Destination, pathSegment.Weight);
        }

        public static Path CloneTo(this Path path, int i)
        {
            var edges = new List<PathSegment>();
            int l = path.Segments.Count;
            if (i > l)
                i = l;

            for (int j = 0; j < i; j++)
            {
                edges.Add(path.Segments[j].Clone());
            }

            var newPath = new Path(path.Origin);
            edges.ForEach(e => newPath.AddSegment(e));
            return newPath;
        }


        public static Path Clone(this Path path)
        {
            var newPath = new Path(path.Origin);
            path.Segments.ToList().ForEach(e => path.AddSegment(new PathSegment(e.Origin, e.Destination, e.Weight)));
            return newPath;
        }

        public static bool IsEqualsTo(this Path path, Path path2)
        {
            if (path2 == null)
                return false;

            var edges2 = path2.Segments;

            int numEdges1 = path.Segments.Count;
            int numEdges2 = edges2.Count;

            if (numEdges1 != numEdges2)
            {
                return false;
            }

            for (int i = 0; i < numEdges1; i++)
            {
                var edge1 = path.Segments[i];
                var edge2 = edges2[i];
                if (edge1.Origin.Id != edge2.Origin.Id)
                    return false;
                if (edge1.Destination.Id != edge2.Destination.Id)
                    return false;
            }

            return true;
        }
    }
}
