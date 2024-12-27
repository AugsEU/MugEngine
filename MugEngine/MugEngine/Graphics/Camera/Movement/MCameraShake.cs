using MugEngine.Library.Maths.Easing;

namespace MugEngine.Graphics
{
	public class MCameraShake : MCameraMovement
	{
		IMEase mDiminishFunc;
		Vector2 mAmplitude;
		float mAngularSpeed;

		public MCameraShake(Vector2 amplitude, float speed = 100.0f) : base()
		{
			mAmplitude = amplitude;
			mAngularSpeed = speed * 10.0f;
			mDiminishFunc = new MEaseRecipJBend(10.0f);
		}

		public MCameraShake(Vector2 amplitude, float speed, IMEase diminishFunc) : base()
		{
			mAmplitude = amplitude;
			mAngularSpeed = speed * 10.0f;
			mDiminishFunc = diminishFunc;
		}

		public override MCameraSpec GetSpecDelta(float time)
		{
			MCameraSpec spec = new MCameraSpec();

			float angle = mAngularSpeed * time * MathF.Tau;
			float diminish = 1.0f - mDiminishFunc.Func(time);

			(float s, float c) = MathF.SinCos(angle);

			spec.mPosition.X = c * mAmplitude.X * diminish;
			spec.mPosition.Y = s * mAmplitude.Y * diminish;

			return spec;
		}
	}
}
