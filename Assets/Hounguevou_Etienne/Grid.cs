using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    // Grid specifics
    [SerializeField]
    private int rows;

    [SerializeField]
    private int cols;

    [SerializeField]
    private Vector2 gridSize;

    [SerializeField]
    private Vector2 gridOffset;

    // About cells

    [SerializeField]
    private Sprite cellSprite;

    [SerializeField]
    private Vector2 cellSize;

    [SerializeField]
    private Vector2 cellScale;

    void Start()
    {
        initCells(); // Init all cells
    }

    void initCells()
    {
        GameObject cellObject = new GameObject();
        // Creta an empty GameObject component and add a sprite renderer => set the sprite to cellSprite
        cellObject.AddComponent<SpriteRenderer>().sprite = cellSprite;

        // Catch the size of the sprite
        cellSize = cellSprite.bounds.size;

        // Get the new cell size => Adjust the size of the cells to fit the size of the grid
        Vector2 newCellSize = new Vector2(gridSize.x / (float)cols, gridSize.y / (float)rows);

        // Get the scales so you can scale the cells and change their size to fit th grid
        cellScale.x = newCellSize.x / cellSize.x;
        cellScale.y = newCellSize.y / cellSize.y;

        cellSize = newCellSize; // The size will be replace by new the computed cell size, we just used cellSize for computing the scale

        cellObject.transform.localScale = new Vector2(cellScale.x, cellScale.y);

        //fix the cells to the grid by getting the half of the grid and cells add and minus experiment
        gridOffset.x = -(gridSize.x / 2) + cellSize.x / 2;
        gridOffset.y = -(gridSize.y / 2) + cellSize.y / 2;

        // Fill the grid with cells by using instantiate
        for (int row = 0; row > rows; rows++)
        {
            for (int col = 0; col < cols; cols++)
            {
                // Add the cellSize so that two cells will not have the same x and y position
                Vector2 pos = new Vector2(col * cellSize.x + gridOffset.x + transform.position.x, row * cellSize.y + gridOffset.y + transform.position.y);
                // instantiate the gameObject, at position pos, with rotation set to identity
                GameObject cO = Instantiate(cellObject, pos, Quaternion.identity) as GameObject;
                // Set the parent of the cell to grid so you can move the cell together with the grid
                cO.transform.parent = transform;
            }


        }

        // Destroy the object used to instantiate the cells
        Destroy(cellObject);
    }
    // So you can see the width and height of the grid on the editor
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridSize);

    }
}
