using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWall;

    [SerializeField]
    private GameObject _rightWall;

    [SerializeField]
    private GameObject _frontWall;

    [SerializeField]
    private GameObject _backWall;

    public bool IsVisited { get; private set; }

    public void Visit()
    {
        IsVisited = true;
    }

    public void ClearLeftWall()
    {
        _leftWall.SetActive(false);
    }

    public void ClearRightWall()
    {
        _rightWall.SetActive(false);
    }

    public void ClearFrontWall()
    {
        _frontWall.SetActive(false);
    }

    public void ClearBackWall()
    {
        _backWall.SetActive(false);
    }


    public bool LeftWallStatus()
    {
        bool status = _leftWall.activeSelf;
        return status;
    }

    public bool RightWallStatus()
    {
        bool status = _rightWall.activeSelf;
        return status;
    }

    public bool FrontWallStatus()
    {
        bool status = _frontWall.activeSelf;
        return status;
    }

    public bool BackWallStatus()
    {
        bool status = _leftWall.activeSelf;
        return status;
    }
}
