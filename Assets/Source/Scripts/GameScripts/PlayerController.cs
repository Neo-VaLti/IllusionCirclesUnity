using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    //Define la velocidad de la paleta.
    public float PaddleSpeed;

	void FixedUpdate () {
        //Si se presiona la tecla hacia arriba
        if (Input.GetKey("up"))
        {
            //Al componente rigidbody2D de la paleta se le cambia la velocidad mediante la resta de posiciones.
            //Esto asegura que se mueva de forma suave, y no directa. Luego se divide entre el tiempo delta de la aplicacion.
            rigidbody2D.velocity = (new Vector3(transform.position.x + PaddleSpeed, transform.position.y) - transform.position) / Time.deltaTime;
        }

        //Si se presiona la tecla hacia abajo
        if (Input.GetKey("down"))
        {
            //Al componente rigidbody2D de la paleta se le cambia la velocidad mediante la resta de posiciones.
            //Esto asegura que se mueva de forma suave, y no directa. Luego se divide entre el tiempo delta de la aplicacion. 
            rigidbody2D.velocity = (new Vector3(transform.position.x - PaddleSpeed, transform.position.y) - transform.position) / Time.deltaTime;
        }

        //AUN NO SE USA ESTO--
        //Añade una ralentizacion al movimiento de la paleta.
        //Aqui si se toma en cuenta los bordes de la pantalla, ya que se le añade fuerza al cuerpo
        //y no se cambia directo las coordenadas de la paleta.
        //rigidbody2D.AddForce(new Vector2(0, PaddleSpeed));
	}
}
