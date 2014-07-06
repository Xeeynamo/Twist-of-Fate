using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace We
{
	/// <summary>
	/// Sistema personalizzato per la fisica
	/// </summary>
	public class Physics
	{
		Vector3 direction;
		float acceleration;
		float deceleration;
		float friction;
		float maxspeed;
        bool isInAir;

		private float DeltaTime
		{
			get { return Time.smoothDeltaTime; }
		}
		
		/// <summary>
		/// Numero di pixel da percorrere per raggiungere la velocità massima
		/// Valore di default: 4
		/// </summary>
		public float Acceleration
		{
			get { return acceleration; }
			set { acceleration = value; }
		}
		
		/// <summary>
		/// Numero di pixel di frenata quando si cambia direzione
		/// Valore di default: 32
		/// </summary>
		public float Deceleration
		{
			get { return deceleration; }
			set { deceleration = value; }
		}

		/// <summary>
		/// Valore di frizione; entra in gioco quando non viene premuto alcun
		/// tasto direzionale.
		/// Valore di default: 4
		/// </summary>
		/// <value>The friction.</value>
		public float Friction
		{
			get { return friction; }
			set { friction = value; }
		}

		/// <summary>
		/// Velocità massima in pixel per secondo
		/// Valore di default: 128
		/// </summary>
		/// <value>The maximum speed.</value>
		public float MaximumSpeed
		{
			get { return maxspeed; }
			set { maxspeed = value; }
		}

		/// <summary>
		/// Controlla se l'oggetto è in aria
		/// </summary>
		public bool IsInAir
		{
			get { return isInAir;}
		}

		public Physics()
		{
			Acceleration = 0.1f;
			Deceleration = 0.5f;
			Friction = 0.2f;
			MaximumSpeed = 1f;
			direction = new Vector3 ();
		}

		public Vector3 Process()
		{
			if (Input.MoveLeft)
			{
				if (direction.x > 0)
					direction.x -= Mathf.Min (direction.x, deceleration);
				else
					direction.x -= acceleration;
			}
			else if (Input.MoveRight)
			{
				if (direction.x < 0)
					direction.x += deceleration;
				else
					direction.x += acceleration;
			}
			else
			{
				// Math.Min serve per non far andare sotto lo 0 lo spostamento durante
				// la frizione, ondevitare artefatti nel sistema di fisica
				direction.x -= Mathf.Min (Mathf.Abs(direction.x), friction) * Mathf.Sign (direction.x);
			}
			if (Mathf.Abs(direction.x) > maxspeed)
				direction.x = maxspeed * Mathf.Sign(direction.x);

			return GetDirection();
		}
		public Vector3 GetDirection()
		{
			return new Vector3(direction.x * DeltaTime,
			                   direction.y * DeltaTime,
			                   direction.z * DeltaTime);
		}
	}
}