using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
   [SerializeField] bool OnGround;//zemin kontrolu için
    private float width;
    private Rigidbody2D myBody;
    [SerializeField] LayerMask engel;//objenin layerı üzerinden kontrol edeceğiz ki engel üzerindeyken ısın kontrolu yapabilelim.
    // Start is called before the first frame update
    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.extents.x;//karekterimizin ışınını dogru atabilmemiz için sprite renderer yarısını alıyoruz ki sağ tarafta dogru ışın kullanalım
        myBody=GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {   /* 5*(2,2,2)=(10,10,10), birim vektor (1,0,0) x için,y için (0,1,0), z için (0,0,1)*/
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (transform.right /*sağrataf birim vektörü*/ * width/2), Vector2.down, 2f,engel);   //cisimlere ışın atarak çarpışma kontrolu yapmamızı sağlar parametreleri baslama noktası ışını nerden atsın, nerye dogru atsın ne kdar mesafeye atsın
        if(hit.collider != null)//yerde olup olmadığının kontrolunu hareket ettirmek için yapıyoruz
        {
            OnGround = true;
        }
        else
        {
            OnGround = false;
        }
        if(!OnGround)
        {
            transform.eulerAngles += new Vector3(0, 180f, 0);//enemy zemin sınırına gelince enemy diger gtarafa ceviriyor
        }
        myBody.velocity = new Vector2(transform.right.x * 3f, 0f);//düşmanı hareket ettiriyoruz sag tarafa*/
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 playerRealPositions = transform.position + (transform.right * width / 2);
        Gizmos.DrawLine(playerRealPositions, playerRealPositions + new Vector3(0, -2f, 0));
    }
}
