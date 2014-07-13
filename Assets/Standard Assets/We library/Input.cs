using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace We
{
	// gestisce l'input per valori booleani
	public static class Input
	{
		public static bool MoveUp
		{
			get { return UnityEngine.Input.GetAxis("Vertical") > 0; }
		}
		public static bool MoveDown
		{
			get { return UnityEngine.Input.GetAxis("Vertical") < 0; }
		}
		public static bool MoveLeft
		{
			get { return UnityEngine.Input.GetAxis("Horizontal") < 0; }
		}
		public static bool MoveRight
		{
			get { return UnityEngine.Input.GetAxis("Horizontal") > 0; }
		}
		public static int MoveHorizontal
		{
			get { return MoveLeft ? -1 : MoveRight ? +1 : 0; }
		}
		public static int MoveVertical
		{
			get { return MoveUp ? -1 : MoveDown ? +1 : 0; }
		}
		public static bool Jump
		{
			get { return UnityEngine.Input.GetButton ("Jump"); }
		}
		public static bool SwitchItem
		{
            get { return UnityEngine.Input.GetKeyDown(KeyCode.Z); }
		}
		public static bool Defense
		{
			get { return UnityEngine.Input.GetKey(KeyCode.X); }
		}
        public static bool AttackSecondary
        {
            get { return UnityEngine.Input.GetKeyDown(KeyCode.C); }
        }
        public static bool AttackPrimary
        {
            get { return UnityEngine.Input.GetKeyDown(KeyCode.V); }
        }
		public static bool Pause
		{
			get { return UnityEngine.Input.GetKeyDown(KeyCode.Return); }
		}
		public static bool Exit
		{
			get { return UnityEngine.Input.GetKeyDown(KeyCode.Escape); }
		}
	}
}