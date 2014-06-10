using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace We
{
	/// <summary>
	/// Sistema personalizzato per le animazioni
	/// </summary>
	public class Animation
	{
		class State
		{
			public int id;
			public Sprite[] sprites;
			public int loop;
			public int speed;
			
			public State(int id, Sprite[] sprites, int loop, int speed)
			{
				this.id = id;
				this.sprites = sprites;
				this.loop = loop;
				this.speed = speed;
			}
		}

		// Lista delle animazioni
		List<State> listState;
		// Stato correntemnte selezionato
		State currentState;
		// Animazione correntemente selezionata
		int currentAnim;
		// Frame correntemente attivo
		int currentFrame;
		// Numero di passi correnti per arrivare al frame successivo
		float currentStep;

		public int CurrentAnimation
		{
			get { return currentAnim; }
			set 
			{
				if (currentAnim != value)
				{
					currentAnim = value;
					if (value >= 0 && value < listState.Count)
					{
						currentState = listState[value];
						currentAnim = value;
						currentFrame = 0;
						currentStep = 0;
					}
					else
					{
						currentState = null;
					}
				}
			}
		}

		public Sprite CurrentFrame
		{
			get
			{
				if (CurrentAnimation < listState.Count)
				{
					if (currentState != null)
					{
						Sprite[] s = currentState.sprites;
						if (s.Length == 0)
							return null;
						return s[currentFrame % s.Length];
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Inizializza il sistema di animazioni
		/// </summary>
		public Animation()
		{
			listState = new List<State> ();
			CurrentAnimation = -1;
		}

		/// <summary>
		/// Aggiunge un'animazione
		/// </summary>
		/// <returns>L'ID dell'animazione per poterla selezionre</returns>
		/// <param name="sprites">Elenco di frames</param>
		/// <param name="loop">Frame di loop; se è -1, significa che ha una fine</param>
		/// <param name="speed">Velocità per 1/60 frame per secondo</param>
		public int AddAnimation(Sprite[] sprites, int loop, int speed)
		{
			listState.Add (new State (listState.Count, sprites, loop, speed));
			return listState.Count - 1;
		}

		/// <summary>
		/// Process the animation.
		/// </summary>
		public bool Process()
		{
			if (currentState != null)
			{
				currentStep += (Time.smoothDeltaTime * 50);
				while (currentStep >= currentState.speed)
				{
					currentFrame++;
					int spriteLength = currentState.sprites.Length;
					if (currentFrame >= spriteLength)
					{
						int value = currentState.loop;
						if (value < 0 || value >= spriteLength)
							return false;
						currentFrame = value;
					}
					currentStep -= currentState.speed;
				}
			}
			return true;
		}
	}
}
