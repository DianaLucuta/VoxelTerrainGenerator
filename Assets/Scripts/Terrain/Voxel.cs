using UnityEngine;

public class Voxel {

    private int x, y, z;

    private int type;

    private Voxel topNeighbor = null;

    private Voxel bottomNeighbor = null;

    private Voxel leftNeighbor = null;

    private Voxel rightNeighbor = null;

    private Voxel frontNeighbor = null;

    private Voxel backNeighbor = null;

    private bool isActive = true;

    private bool hasTree = false;

    public void SetActive (bool isActive)
    {
        this.isActive = isActive;
    }

    public void SetTerrainType (int type)
    {
        this.type = type;
    }

    public void SetPosition (int x, int y, int z)
    {
        this.x = x;

        this.y = y;

        this.z = z;
    }

    public void SetTopNeighbor (Voxel topNeighbor)
    {
        this.topNeighbor = topNeighbor;
    }

    public void SetBottomNeighbor (Voxel bottomNeighbor)
    {
        this.bottomNeighbor = bottomNeighbor;
    }

    public void SetLeftNeighbor (Voxel leftNeighbor)
    {
        this.leftNeighbor = leftNeighbor;
    }

    public void SetRightNeighbor (Voxel rightNeighbor)
    {
        this.rightNeighbor = rightNeighbor;
    }

    public void SetFrontNeighbor(Voxel frontNeighbor)
    {
        this.frontNeighbor = frontNeighbor;
    }

    public void SetBackNeighbor(Voxel backNeighbor)
    {
        this.backNeighbor = backNeighbor;
    }

    public Voxel GetTopNeighbor ()
    {
        return this.topNeighbor;
    }

    public Voxel GetBottomNeighbor()
    {
        return this.bottomNeighbor;
    }

    public Voxel GetLeftNeighbor ()
    {
        return this.leftNeighbor;
    }

    public Voxel GetRightNeighbor ()
    {
        return this.rightNeighbor;
    }

    public Voxel GetFrontNeighbor()
    {
        return this.frontNeighbor;
    }

    public Voxel GetBackNeighbor()
    {
        return this.backNeighbor;
    }

    public int GetX ()
    {
        return this.x;
    }

    public int GetY ()
    {
        return this.y;
    }

    public int GetZ ()
    {
        return this.z;
    }

    public Vector3 GetPosition ()
    {
        return new Vector3(this.x, this.y, this.z);
    }

    public int GetTerrainType ()
    {
        return this.type;
    }

    public bool IsActive ()
    {
        return this.isActive;
    }

    public void SetTree ()
    {
        if (this.IsValidForTree()) 
        {
            this.hasTree = true;
        }
    }

    public bool HasTree()
    {
        return this.hasTree;
    }

    public bool IsValidForTree ()
    {
        if (this.type <= TerrainType.SAND && this.type > -TerrainType.SNOW)
        {
            return false;
        }

        if (leftNeighbor == null || !leftNeighbor.IsActive() ||
            rightNeighbor == null || !rightNeighbor.IsActive() ||
            frontNeighbor == null || !frontNeighbor.IsActive() ||
            backNeighbor == null || !backNeighbor.IsActive())
        {
            return false;
        }

        if (leftNeighbor.GetTopNeighbor() == null ||
            rightNeighbor.GetTopNeighbor() == null ||
            frontNeighbor.GetTopNeighbor() == null ||
            backNeighbor.GetTopNeighbor() == null) 
        {
            return false;
        }

        if ((leftNeighbor.GetTopNeighbor() != null && leftNeighbor.GetTopNeighbor().IsActive()) ||
            (rightNeighbor.GetTopNeighbor() != null && rightNeighbor.GetTopNeighbor().IsActive()) ||
            (frontNeighbor.GetTopNeighbor() != null && frontNeighbor.GetTopNeighbor().IsActive()) ||
            (backNeighbor.GetTopNeighbor() != null && backNeighbor.GetTopNeighbor().IsActive()))
        {
            return false;
        }

        return true;
    }
}