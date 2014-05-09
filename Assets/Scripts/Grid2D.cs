using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Grid2DBase<T> : MonoBehaviour 
    where T : Component
{
    // Don't change these values at runtime.  Use the properties below
    // instead.
    public int width;
	public int height;
	public float cellWidth;
	public float cellHeight;
	public int oldWidth;
	public int oldHeight;


    // If you need to change the size of the grid at runtime, do it
    // through these properties instead of the public fields above.
    // This will make sure the OnResized event is fired, which gives
    // objects in the grid a change to reposition themselves.
    public int Width 
    {
        get { return width; }
        set
        {
			oldWidth = width;
            width = value;
            Resize();
        }
    }

    public int Height
    {
        get { return height; }
        set
        {
			oldHeight = height;
            height = value;
            Resize();
        }
    }

    public float CellWidth
    {
        get { return cellWidth; }
        set
        {
            cellWidth = value;
            Resize();
        }
    }

    public float CellHeight
    {
        get { return cellHeight; }
        set
        {
            cellHeight = value;
            Resize();
        }
    }

    public Plane Plane
    {
        get
        {
            return new Plane(transform.forward, transform.position);
        }
    }

    // Objects in the grid can register for this event to
    // reposition themselves if the size of the grid changes.
    event System.Action<Grid2DBase<T>> OnResized;

    private struct Cell
    {
        public int X;
        public int Y;
        public List<T> Entries;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            Entries = new List<T>();
        }
    }

    // The cell data
    private Cell[] cells;

    public void Add(T obj, int x, int y)
    {
        if (!IsValidPosition(x, y))
        {
            throw new System.IndexOutOfRangeException("Invalid position");
        }
        
        int index = GetIndex(x, y);
        
        // If there haven't been any entries in this cell, the list will be
        // null.  Init it.
        if (cells[index].Entries == null)
        {
            cells[index].Entries = new List<T>();
        }
        else
        {
            // Check if this object is already in the cell
            if (cells[index].Entries.Contains(obj))
            {
                return;
            }
        }
        
        // Add
        cells[index].Entries.Add(obj);
    }

    public void Remove(T obj, int x, int y)
    {
        if (!IsValidPosition(x, y))
        {
            throw new System.IndexOutOfRangeException("Invalid position");
        }

        int index = GetIndex(x, y);

        if (cells[index].Entries != null)
        {
            cells[index].Entries.Remove(obj);
        }
    }

    public List<T> Get(int x, int y)
    {
        if (!IsValidPosition(x, y))
        {
            throw new System.IndexOutOfRangeException("Invalid position");
        }

        int index = GetIndex(x, y);

        return cells[index].Entries;
    }

    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }


    // Cell position functions.  Use these to snap or animate objects to positions
    // on the grid.
  
    public Vector3 GetCellBottomLeft(int x, int y)
    {
        Vector3 pos = new Vector3((float)x * CellWidth - 2, (float)y * CellHeight - 2, 0.0f);

        return transform.TransformPoint(pos);
    }

    public Vector3 GetCellBottomRight(int x, int y)
    {
        Vector3 pos = new Vector3((float)x * CellWidth + CellWidth - 2, (float)y * CellHeight - 2, 0.0f);

        return transform.TransformPoint(pos);
    }

    public Vector3 GetCellTopRight(int x, int y)
    {
        Vector3 pos = new Vector3((float)x * CellWidth + CellWidth - 2, (float)y * CellHeight + CellHeight - 2, 0.0f);

        return transform.TransformPoint(pos);
    }

    public Vector3 GetCellTopLeft(int x, int y)
    {
        Vector3 pos = new Vector3((float)x * CellWidth - 2, (float)y * CellHeight + CellHeight - 2, 0.0f);

        return transform.TransformPoint(pos);
    }

    public Vector3 GetBottomCenter(int x, int y)
    {
        Vector3 pos = new Vector3((float)x * CellWidth + CellWidth * 0.5f - 2, (float)y * CellHeight - 2, 0.0f);

        return transform.TransformPoint(pos);
    }

    public Vector3 GetLeftCenter(int x, int y)
    {
        Vector3 pos = new Vector3((float)x * CellWidth - 2, (float)y * CellHeight + CellHeight * 0.5f - 2, 0.0f);

        return transform.TransformPoint(pos);
    }

    public Vector3 GetRightCenter(int x, int y)
    {
        Vector3 pos = new Vector3((float)x * CellWidth + CellWidth - 2, (float)y * CellHeight + CellHeight * 0.5f - 2, 0.0f);

        return transform.TransformPoint(pos);
    }

    public Vector3 GetTopCenter(int x, int y)
    {
        Vector3 pos = new Vector3((float)x * CellWidth + CellWidth * 0.5f - 2, (float)y * CellHeight + CellHeight - 2, 0.0f);

        return transform.TransformPoint(pos);
    }

    public Vector3 GetCellCenter(int x, int y)
    {
        Vector3 pos = new Vector3((float)x * CellWidth + CellWidth * 0.5f - 2, (float)y * CellHeight + CellHeight * 0.5f - 2, 0.0f);

        return transform.TransformPoint(pos);
    }

	//MARK: -2 may not be the proper method for adjusting for grid offset
    public bool GetCellFromWorldSpacePosition(Vector3 position, out int x, out int y)
    {
        Vector3 gridSpacePosition = transform.InverseTransformPoint(position);

        x = Mathf.FloorToInt(gridSpacePosition.x / CellWidth) - 2;
        y = Mathf.FloorToInt(gridSpacePosition.y / CellHeight) - 2;
    
        return IsValidPosition(x, y);
    }

    // I haven't tested this function. It's possible that the mouse Y position
    // will need to be flipped.
    public bool GetPositionUnderMouse(Camera camera, out int x, out int y)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        float dist = 0.0f;
        if (this.Plane.Raycast(ray, out dist))
        {
            return GetCellFromWorldSpacePosition(ray.GetPoint(dist), out x, out y);
        }

        // We didn't hit the plane
        x = -1;
        y = -1;
        return false;
    }

    private void OnDrawGizmos()
    {
        Vector3 start, end;

        for (int y = 0; y <= Height; ++y)
        {
            for (int x = 0; x <= Width; ++x)
            {
                start = transform.TransformPoint(new Vector3(0.0f-2, y * CellHeight-2, 0.0f));
                end = transform.TransformPoint(new Vector3(CellWidth * Width-2, y * CellHeight-2, 0.0f));
                Debug.DrawLine(start, end, Color.white);

                start = transform.TransformPoint(new Vector3(x * CellWidth-2, 0.0f-2, 0.0f));
                end = transform.TransformPoint(new Vector3(x * CellWidth-2, CellHeight * Height-2, 0.0f));
                Debug.DrawLine(start, end, Color.white);
            }
        }
    }

    private void Resize()
    {
        Cell[] oldCells = cells;

        cells = new Cell[Width * Height];

        // Init new cells
        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                int index = GetIndex(x, y);
                cells[index] = new Cell(x, y);
            }
        }

		/*
        foreach (Cell oldCell in oldCells)
        {
            // if the old cell's position is still valid, transfer the cell's objects
            if (IsValidPosition(oldCell.X, oldCell.Y))
            {
                foreach (T entry in oldCell.Entries)
                {
                    Add(entry, oldCell.X, oldCell.Y);
                }
            }
        }*/

		for(int i = 0; i < (oldHeight*oldWidth) ; i+= 1)
		{
			// if the old cell's position is still valid, transfer the cell's objects
			if (IsValidPosition(oldCells[i].X, oldCells[i].Y))
			{
				foreach (T entry in oldCells[i].Entries)
				{
					Add(entry, oldCells[i].X, oldCells[i].Y);
				}
			}
		}

        if (OnResized != null)
        {
            OnResized(this);
        }
    }

    private int GetIndex(int x, int y)
    {
        return y * Width + x;
    }
}

// Concrete Grid2D subclass that will work for any component.  If you want to 
// be more type-safe, derive from Grid2DBase<> with more specific types.
public class Grid2D : Grid2DBase<Component> {}
