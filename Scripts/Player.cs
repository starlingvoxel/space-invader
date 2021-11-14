using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5.0f;
    public Projectile laserPrefab;
    public System.Action killed;
    public bool laserActive { get; private set; }
    AudioSource Audio_Source;

    //Clips de audio
    public AudioClip
            player_shoot;
     
        
      

    private void Update()
    {

          Audio_Source = GetComponent<AudioSource>();

        Vector3 position = this.transform.position;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            position.x -= this.speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            position.x += this.speed * Time.deltaTime;
        }

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);


        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);
        this.transform.position = position;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            Audio_Source.clip = player_shoot;
            Audio_Source.Play();
            Shoot();
        }
    }

    private void Shoot()
    {
       
        if (!this.laserActive)
        {
            this.laserActive = true;

            Projectile laser = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            laser.destroyed += OnLaserDestroyed;
        }
    }

    private void OnLaserDestroyed(Projectile laser)
    {
        this.laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            if (this.killed != null) {
                this.killed.Invoke();
            }
        }
    }

}
