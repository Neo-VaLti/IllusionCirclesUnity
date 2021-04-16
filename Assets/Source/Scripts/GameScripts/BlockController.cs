using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockController : MonoBehaviour
{

    //Esta variable sirve para determinar cuantas veces le tenemos
    //que pegar al bloque para que se destruya.
    //La vida maxima se usa para poder manejar las vidas con la lista
    //de sprites.
    private int vidaBloque;
    private int vidaMaxima;

    //Esta lista se usa para guardar las texturas de los bloques.
    private List<Texture> texturas;

    void Start()
    {
        texturas = new List<Texture>();

        //Aqui andamos poniendole el valor de la vida dependiendo
        //de que tipo de bloque.

        switch (tag)
        {
            case "RedBlock":
                vidaMaxima = 1;
                break;
            case "BlueBlock":
                vidaMaxima = 1;
                break;
            case "GreenBlock":
                vidaMaxima = 2;
                texturas.Add(GetComponent<SpriteRenderer>().material.mainTexture);
                texturas.Add(Resources.Load<Texture>("Sprites/Bloques/Texturas/BloqueVerde-2"));
                break;
            case "YellowBlock":
                vidaMaxima = 3;
                texturas.Add(GetComponent<SpriteRenderer>().material.mainTexture);
                texturas.Add(Resources.Load<Texture>("Sprites/Bloques/Texturas/BloqueAmarillo-2"));
                texturas.Add(Resources.Load<Texture>("Sprites/Bloques/Texturas/BloqueAmarillo-3"));
                break;
        }

        vidaBloque = vidaMaxima;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //En caso de colisionar con la pelota..
        if (col.gameObject.tag == "Ball")
        {
            //En caso de que colisionemos con un bloque
            //que se pueda destruir..
            if (tag != "GrayBlock")
            {
                //Se le quita uno a la vida del bloque.
                vidaBloque -= 1;

                //En caso de que la vida sea igual a 0..
                if (vidaBloque == 0)
                {
                    //Se destruye el bloque.
                    Destroy(gameObject);
                }
                else
                {
                    GetComponent<SpriteRenderer>().material.mainTexture = texturas.ElementAt(vidaMaxima - vidaBloque);
                    GetComponent<SpriteRenderer>().material.mainTexture = texturas.ElementAt(vidaMaxima - vidaBloque);
                }
            }
        }
    }
}
