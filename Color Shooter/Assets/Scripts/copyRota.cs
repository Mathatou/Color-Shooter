using UnityEngine;

public class copyRota : MonoBehaviour
{
    [SerializeField] Transform Head;
    [SerializeField] private Vector3 localOffset ;
    void LateUpdate()
    {
        //La rota y de la tete
        Quaternion yawRota = Quaternion.Euler(-95,Head.eulerAngles.y,0);

        transform.position = Head.position +  localOffset;

        transform.rotation = yawRota;

    }
}
