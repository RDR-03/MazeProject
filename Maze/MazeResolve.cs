/*public class Sidewinder {
    public static Grid Resolve(Grid grid, int seed = 1) {
        var rand = seed >= 0 ? new Random(seed) : new Random();
        foreach (var row in grid.Row) {
            var run = new List<Cell>();

            foreach (var cell in row) {
                run.Add(cell);

                var EasternBoundry = cell.East == null;
                var NorthernBoundry = cell.North == null;

                var ShouldCloseOut = EasternBoundry || (!NorthernBoundry && rand.Next(2) == 0);
                if (ShouldCloseOut) {
                    Cell member = run[rand.Next(seed)];
                    if (member.North != null)
                        member.Link(member.North);
                    run.Clear();                   
                }
                else cell.Link(cell.East!);
            }
        }
        return grid;
    }
}*/

namespace Project {
    public class MazeRezolve{
        public static void CreateMaze(Grid grid)
        {
            var stack = new Stack<Cell>();
            var visited = new HashSet<Cell>();

            Cell startCell = grid[0,0]!;
            stack.Push(startCell);
            visited.Add(startCell);

            // Visitar celdas vecinas hasta que no haya elementos en la pila
            while (stack.Count > 0)
            {
                Cell currentCell = stack.Peek();
                List<Cell> unvisitedNeighbors = GetUnvisitedNeighbors(currentCell, visited);

                if (unvisitedNeighbors.Count > 0)
                {
                    Cell chosenNeighbor = unvisitedNeighbors[new Random().Next(unvisitedNeighbors.Count)];
                    currentCell.Link(chosenNeighbor);
                    stack.Push(chosenNeighbor);
                    visited.Add(chosenNeighbor);
                }
                else stack.Pop();
            }
        }
        private static List<Cell> GetUnvisitedNeighbors(Cell cell, HashSet<Cell> visited)
        {
            List<Cell> un_neighbors = new List<Cell>();

            if (cell.North != null && !visited.Contains(cell.North))
                un_neighbors.Add(cell.North);
            
            if (cell.South != null && !visited.Contains(cell.South))
                un_neighbors.Add(cell.South);

            if (cell.East != null && !visited.Contains(cell.East))
                un_neighbors.Add(cell.East);

            if (cell.West != null && !visited.Contains(cell.West))
                un_neighbors.Add(cell.West);

            return un_neighbors;
        }
    }
}