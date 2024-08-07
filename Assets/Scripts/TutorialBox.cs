using UnityEngine;

public class TutorialBox : MonoBehaviour, ITargetable
{
    [SerializeField] private Vector3 targetPos;
    bool move;
    public void OnHit()
    {
        move = true;
    }

    public void SetObject(bool value)
    {
        gameObject.SetActive(value);
    }

    private void Update()
    {
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.1f);

            if (transform.position == targetPos)
            {
                move = false;
            }
        }
    }

}
