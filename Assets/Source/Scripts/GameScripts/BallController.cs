using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{

    //Define la velocidad de la pelota.
    public float ballSpeed;

    //Define la velocidad de la cola que sigue a la pelota.
    public float trailSpeed;

    //La herramienta que mueve la textura de la cola.
    private TextureMover mover;

    //Este timer sirve para reiniciar la pelota despues de 
    //unos segundos. Esta puesta por default en 3.
    //El segundo timer sirve para que vuelva a iniciar el juego.
    //Cuando termine de correr, la pelota se empezara a mover.
    //El tercer timer indica el tiempo a pasar para que se carge
    //el siguiente nivel.
    private Timer respawnTimer;
    private Timer resumeTimer;
    private Timer nextLevelTimer;

    //La variable respawnTime indica los segundos a pasar para el respawn
    //de la pelota.
    //La variable resumeTime indica los segundos a pasar para que el juego
    //siga, y la pelota empieze a moverse.
    //La variable loadTime es el tiempo a pasar para que se carge el
    //siguiente nivel.
    private int respawnTime = 2;
    private int resumeTime = 1;
    private int loadTime = 3;

    //Esta variable es para detectar si nos atoramos en un eje.
    //Esta variable indica cuantas veces la pelota puede rebotar en un solo eje.
    private int maxCollisions = 2;
    //Esta variable es la sensibilidad que tiene el algoritmo en detectar si se esta collisionando
    //en un solo eje. Mientras mas alto el valor de esta, mas sensible es el algoritmo en detectar.
    private float collisionSensitivity = 20.0f;

    //Este arreglo de vectores se usan para checar si la bola sigue rebotando en un eje.
    //Es para evitar que se quede trabada la bola en un solo eje.
    private List<Vector2> ballPoints;

    //Esta es nuestra plataforma.
    //La ocupamos para determinar en donde vamos a aparecer cuando se destruya la pelota.
    public GameObject plataforma;

    //Posicion inicial pelota
    private Vector3 posicionInicial;

    //Nuestro emisor de particulas.
    //private ParticleSystem emitter;

	void Start () {
        //Inicializando nuestro emisor.
	    //emitter = GetComponentInChildren<ParticleSystem>();
	    //emitter.emissionRate = 50;
	    //emitter.startLifetime = 1;

        //Inicializando nuestra posicion inicial-
	    posicionInicial = transform.position;

        //Inicializando nuestros timers.
        respawnTimer = new Timer();
        resumeTimer = new Timer();
        nextLevelTimer = new Timer();
	    
        //Se empieza el timer, para que le de tiempo al jugador de posicionarse.
        //Al acabarse el tiempo, se empezara a mover la pelota.
        resumeTimer.StartTimer();

        //Inicializando la lista de puntos.
        ballPoints = new List<Vector2>();

        //Inicializando nuestra herramienta.
        //Le damos el material que esta en uso del trailRenderer.
        //Poniendo la velocidad de movimiento.
	    mover = new TextureMover {material = GetComponent<TrailRenderer>().material, scrollSpeed = trailSpeed};
	}

    private void OnCollisionEnter2D(Collision2D col)
    {
        //Aqui se cambia el color de la luz que emite la pelota.
        GetComponentInChildren<Light>().light.color = getRandomColor();
        //emitter.Emit(5);

        //Andamos manejando los puntos de colision, que se usaran para checar los ejes.
        //El limite de puntos esta definido arriba.
        ballPoints.Add(transform.position);

        if (ballPoints.Count == maxCollisions)
        {
            //Esta variable es usada para checar cuantas veces colisionamos en el eje de las X
            int maxBouncesX = 0;
            //Esta variable es usada para checar cuantas veces colisionamos en el eje de las Y
            int maxBouncesY = 0;

            //Vamos a checar por cada punto que agregamos a nuestra lista de colisiones,
            //cuantas veces estamos en el mismo eje.
            foreach (Vector2 point in ballPoints)
            {
                if (point.x >= transform.position.x - collisionSensitivity && point.x <= transform.position.x + collisionSensitivity)
                {
                    maxBouncesX += 1;
                }
                if (point.y >= transform.position.y - collisionSensitivity && point.y <= transform.position.y + collisionSensitivity)
                {
                    maxBouncesY += 1;
                }
            }

            //Ahora se va a añadir fuerza dependiendo de el eje en el que estamos atorados.
            //En este caso, el de las X
            if (maxBouncesX == maxCollisions)
            {
                rigidbody2D.AddForce(new Vector2(1, 0));
            }

            //Ahora se va a añadir fuerza dependiendo de el eje en el que estamos atorados.
            //En este caso, el de las Y
            if (maxBouncesY == maxCollisions)
            {
                rigidbody2D.AddForce(new Vector2(0, 1));
            }

            //Ahora vamos a ahorrar espacio en la lista, recorriendo todos los elementos.
            //Siempre tiene que ser de 3 puntos de largo.
            if (ballPoints.Count > 3)
            {
                for (int i = 1; i < ballPoints.Count; i++)
                {
                    //Aqui estamos recorriendo los elementos hacia atras.
                    ballPoints.Insert(i-1, ballPoints.ElementAt(i));
                }

                //Se quita el ultimo elemento, puesto que ya recorrimos la lista.
                //Esto nos dejara añadir un nuevo punto de collision cuando pase el evento.
                ballPoints.RemoveAt(ballPoints.Count-1);
            }
        }

        // Si se llega a collisionar con la paleta
        if (col.gameObject.tag == "Player")
        {
            // Se calcula en que parte de la paleta le pegamos.
            float x = hitFactor(transform.position, col.transform.position, ((BoxCollider2D)col.collider).size.x);

            // Ya que calculamos la direccion, vamos a normalizar el vector.
            Vector2 dir = new Vector2(x, 1).normalized;

            // Se le añade el valor de velocidad junto con la direccion.
            // Tambien se multiplica el valor delta de la aplicacion.
            rigidbody2D.velocity = dir * ballSpeed;
        }
    }
    void Update()
    {
        //Checar si se acabo el nivel o no
        if (GameManager.getNumberOfCurrentBlocksInLevel() < 1 && !nextLevelTimer.IsTicking())
        {
            //Si no hay bloques, vamos a iniciar nuestro timer.
            nextLevelTimer.StartTimer();
        }

        //Mueve la textura de nuestra cola.
        mover.MoveTexture();

        //Actualizando nuestros timers.
        respawnTimer.UpdateTimer();
        resumeTimer.UpdateTimer();
        nextLevelTimer.UpdateTimer();
        
        //Estamos checando si debemos hacer respawn.
        if (respawnTimer.IsTicking() && respawnTimer.getPassedSeconds() >= respawnTime)
        {
            Vector3 init = new Vector3(plataforma.transform.position.x, posicionInicial.y, posicionInicial.z);
            transform.position = init;

            GetComponent<TrailRenderer>().enabled = true;

            resumeTimer.StartTimer();
            respawnTimer.Stop();
        }
        else if (resumeTimer.IsTicking())
        {
            transform.position = new Vector3(plataforma.transform.position.x, transform.position.y, transform.position.z);

            if (resumeTimer.getPassedSeconds() >= resumeTime)
            {
                rigidbody2D.velocity = new Vector2(plataforma.rigidbody2D.velocity.x,ballSpeed);
                resumeTimer.Stop();
            }
        }
        else if (nextLevelTimer.IsTicking())
        {
            transform.position = new Vector3(plataforma.transform.position.x, posicionInicial.y, transform.position.z);
            rigidbody2D.velocity *= 0;
            
            if (nextLevelTimer.getPassedSeconds() >= loadTime)
            {
                nextLevelTimer.Stop();
                GameManager.LoadNextLevel();

                resumeTimer.StartTimer();
            }
        }
    }
    //Se checa si pegamos al fondo de la pantalla.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "DeathBounds")
        {
            GameManager.TakeLife();

            if (GameManager.IsGameRunning())
            {
                rigidbody2D.velocity *= 0;
                respawnTimer.StartTimer();
                GetComponent<TrailRenderer>().enabled = false;
            }
            else //GAME OVER
            {
                
            }
        }
    }

    //Este metodo nos regresa la fuerza que le dimos a la pelota,
    //dependiendo de que parte le pegamos.
    float hitFactor(Vector2 ballPos, Vector2 padPos, float padWidth)
    {
        // Representacion:
        //
        // 1  -0.5  0  0.5   1  <- valor de x
        // ===================  <- paleta
        //
        return (ballPos.x - padPos.x) / padWidth;
    }
    Color getRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
}
