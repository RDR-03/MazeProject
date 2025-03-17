namespace Project;
public class MazeResolve{
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
            
            // Evitar tener celdas sin ninguna pared
            if (currentCell.IsLinked(currentCell.East!) &&currentCell.IsLinked(currentCell.North!)
                &&currentCell.IsLinked(currentCell.West!) &&currentCell.IsLinked(currentCell.South!))
            {
                Cell[] neighbors = {currentCell.West!, currentCell.North!, currentCell.South!, currentCell.East!};
                Random rand = new Random();
                currentCell.Unlink(neighbors[rand.Next(4)]);
            }
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
