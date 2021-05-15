using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float mySpeedX;
    [SerializeField] float jumpPower;
   [SerializeField] float speed;//serial ile bu ozelliği unity uzerınden vermemizi saglar
    private Rigidbody2D myBody;
    private Vector3 defaultLocalScale;
    public bool onGround;//zemin üzerinde mi kontrolu yapacak
    private bool doubleJump;//bazen birden fazla zıplamasını istersek
    [SerializeField] GameObject arrow;
    [SerializeField] bool attack;
    [SerializeField] float currentAttackTimer;
    [SerializeField] float defaultAttackTimer;
    private Animator myAnimator;
    
    // Start is called before the first frame update
    void Start()
    {//genelde ilk degerler atanma işlemlerinde kullanırız.
        attack = false;
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        defaultLocalScale = transform.localScale;


    }

    // Update is called once per frame
    void Update()
    {
        mySpeedX = Input.GetAxis("Horizontal");//klavyeden sag veya sol ok tusuna basılma suresine gore degerler gelecek.
        myAnimator.SetFloat("Speed",Mathf.Abs(mySpeedX));//burada koddaki speed animator condition speed gonderiyoruz.matf kullanma amacımız vektorel degeri scaler yapmak için.
       myBody.velocity = new Vector2(mySpeedX * speed, myBody.velocity.y);//getcomponent bu scripte sahip objenin componentlerine ulaşmamızı sağlar.Boylece y ekseni sabit kalıyor
        #region player sag sol hareketine göre yüzünün dönmesi
        if (mySpeedX>0)
        {
            transform.localScale = new Vector3(defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);//unityden scale ulasarak sureklı degistirmekle ugrasmayacagız
        }else if(mySpeedX<0)
        {
            transform.localScale = new Vector3(-defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);
        }
        #endregion
        #region player zıplaması kontrolu
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(onGround==true)//bir kere zıpladığımızda 2. zıplama yapmak için doubleJump true yapıyoruz ki diger kontrolde bir daha zıplama kodumuz calıssın.
            {
                myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);
                doubleJump = true;
                myAnimator.SetTrigger("Jump");//zıplatmak için anitamor parametre trigger ekleriz burada zıplamaya geçiş yapmak içn onu tetikleriz.
            }
            else
            {
                if(doubleJump==true)
                {
                    myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);
                    doubleJump = false;
                }
            } //surekli basmada zıplamanın artması için yer ile teması kontrol edip zıplatmalıyız.
        }
        #endregion
        #region player ok atması kontrolu
        if (Input.GetMouseButtonDown(0))//mouse sola basması kontrolu
        { 
            if (attack == false)
            {
                attack = true;
                myAnimator.SetTrigger("Attack");
                Invoke("Fire",0.5f);//okun yaydan cıkmasını esitleme
               
            }

           
        }
        #endregion
        if (attack==true)
            {

            currentAttackTimer -= Time.deltaTime;//iki frame arasında çalışma süresinin arasındaki geçiş süresini temsil eder.

            }
        else
        {
            currentAttackTimer = defaultAttackTimer;
        }
        if(currentAttackTimer < 0)
        {
            attack = false;
          
        }

    }
        void Fire()
        {
            GameObject myOk = Instantiate(arrow, transform.position, Quaternion.identity);//bize player pozisyonunda ok olusturacak,quaternion ise  dönüs prensibi o anki rotasyonunu koruyacak
            //methodun aldığı game object bu metotla olusturaln game objecttir.Yani suan arrow.
            if (transform.localScale.x > 0)
            {
                myOk.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0f);
            }
            else
            {
                Vector3 myOkScale = myOk.transform.localScale;
                myOk.transform.localScale = new Vector3(-1 * myOkScale.x, myOkScale.y, myOkScale.z);
                myOk.GetComponent<Rigidbody2D>().velocity = new Vector2(-5f, 0f);//sola dondugumuzde ok sola gidicek
            }

        }
    private void OnCollisionEnter2D(Collision2D collision)//ÇARPIŞMA KONTROLU YAPIYORUZ
    {
        if(collision.gameObject.tag=="Enemy")
        {
            Die();
        }
    }
    void Die()
    {
        myAnimator.SetFloat("Speed", 0);//öldükten sonra hareket engellemek için speed 0 lıyoruz ki hareket edemesin
        myAnimator.SetTrigger("Die");
        myBody.constraints = RigidbodyConstraints2D.FreezePosition;//öldükten sonra hareketi engellemek için kullanırız
        enabled = false;// burada oldukten sonra karekterin saga sola donmesini engelliyoruz bu kod scripti devre dışı bırakır.Boylece tuslara bastıgımızda kodlar calısmaycagından karekter saga sola donmez



    }
}

