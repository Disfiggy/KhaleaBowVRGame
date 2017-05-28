namespace VRTK.Examples.Archery
{
    using UnityEngine;

    public class BowAnimation : MonoBehaviour
    {
		public Animator animator;
		public LerpBetweenTwoPoints lerp;

		public void SetDraw (float drawAmount)
		{
			animator.SetFloat ("DrawState", drawAmount);
			lerp.value = drawAmount;
		}

		public void Release ()
		{
			animator.SetTrigger ("Release");
			SetDraw (0f);
		}
    }
}