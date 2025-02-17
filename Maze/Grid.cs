using System;
using System.Reflection;
using System.Text;

namespace Project;
public class Grid {
    public int Rows { get; }
    public int Columns { get; }

    private List<List<Cell>>? _grid;
   
    public virtual Cell? this[int row, int column] {
        get {
            if (row < 0 || row >= Rows) return null;
            if (column < 0 || column >= Columns) return null;
            return _grid! [row][column];
        }
    }           
    
    // Iterador de filas
    public IEnumerable<List<Cell>> Row {
        get {
            foreach (var row in _grid!) {
                yield return row;
            }
        }
    }
    
    // Iterador de celdas
    public IEnumerable<Cell> Cells {
        get {
            foreach (var row in Row) {
                foreach (var cell in row) {
                    yield return cell;
                }
            }
        }
    }
    public Grid (int rows, int cols) {
        Rows = rows;
        Columns = cols;
    
        PrepareGrid();
        ConfigCells();
    }
    private void PrepareGrid() {
        _grid = new List<List<Cell>> ();
        for (int r = 0; r < Rows; r++) {
            var row = new List<Cell> ();
            for (int c = 0; c < Columns; c++) {
                row.Add(new Cell(r,c));
            }
            _grid.Add(row);
        }
    }
    private void ConfigCells() {
        foreach (var cell in Cells) {
            var row = cell.Row;
            var col = cell.Column;
            cell.North = this[row - 1, col];
            cell.South = this[row + 1, col];
            cell.East = this[row, col + 1];
            cell.West = this[row, col - 1];
        }
    }
}