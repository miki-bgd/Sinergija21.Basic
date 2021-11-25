using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Sinergija21.Basic.Models
{
	public interface IDisplayManager
	{
		event Action Initialized;

		int DrawLine();

		int DrawSphere(Vector3 position, float radius);

		int DrawCoordinateSystem();

		/// <summary>
		/// Rotates modela round Z axis (right-handed coordinate system 
		/// rotation goes counter-clockwise).
		/// </summary>
		void SetModelZRotation(int id, float angleDeg);
	}
}
