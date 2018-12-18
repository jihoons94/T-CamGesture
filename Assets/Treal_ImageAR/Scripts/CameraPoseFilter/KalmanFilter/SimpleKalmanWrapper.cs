using UnityEngine;

namespace Kalman
{

    /// <summary>
    /// Simple kalman wrapper.
    /// </summary>
    public class SimpleKalmanWrapper : IKalmanWrapper
    {

        private KalmanFilterSimple1D m00;
		private KalmanFilterSimple1D m01;
		private KalmanFilterSimple1D m02;
		private KalmanFilterSimple1D m03;

		private KalmanFilterSimple1D m10;
		private KalmanFilterSimple1D m11;
		private KalmanFilterSimple1D m12;
		private KalmanFilterSimple1D m13;

		private KalmanFilterSimple1D m20;
		private KalmanFilterSimple1D m21;
		private KalmanFilterSimple1D m22;
		private KalmanFilterSimple1D m23;

		private KalmanFilterSimple1D m30;
		private KalmanFilterSimple1D m31;
		private KalmanFilterSimple1D m32;
		private KalmanFilterSimple1D m33;


        public SimpleKalmanWrapper()
        {
            /*
			X0 : predicted state
			P0 : predicted covariance
			
			F : factor of real value to previous real value
			Q : measurement noise
			H : factor of measured value to real value
			R : environment noise

			*/
            double q = 0.1;
            double r = 0.8;
            double f = 1.0;
            double h = 1.0;

            m00 = makeKalmanFilter(q, r, f, h);
            m01 = makeKalmanFilter(q, r, f, h);
            m02 = makeKalmanFilter(q, r, f, h);
			m03 = makeKalmanFilter(q, r, f, h);

			m10 = makeKalmanFilter(q, r, f, h);
			m11 = makeKalmanFilter(q, r, f, h);
			m12 = makeKalmanFilter(q, r, f, h);
			m13 = makeKalmanFilter(q, r, f, h);

			m20 = makeKalmanFilter(q, r, f, h);
			m21 = makeKalmanFilter(q, r, f, h);
			m22 = makeKalmanFilter(q, r, f, h);
			m23 = makeKalmanFilter(q, r, f, h);

			m30 = makeKalmanFilter(q, r, f, h);
			m31 = makeKalmanFilter(q, r, f, h);
			m32 = makeKalmanFilter(q, r, f, h);
			m33 = makeKalmanFilter(q, r, f, h);
        }


		public Matrix4x4 Update(Matrix4x4 current)
        {
			m00.Correct(current.m00);
			m01.Correct(current.m01);
			m02.Correct(current.m02);
			m03.Correct(current.m03);

			m10.Correct(current.m10);
			m11.Correct(current.m11);
			m12.Correct(current.m12);
			m13.Correct(current.m13);

			m20.Correct(current.m20);
			m21.Correct(current.m21);
			m22.Correct(current.m22);
			m23.Correct(current.m23);

			m30.Correct(current.m30);
			m31.Correct(current.m31);
			m32.Correct(current.m32);
			m33.Correct(current.m33);

			Matrix4x4 filtered = Matrix4x4.identity;

			filtered.m00 = (float)m00.State;
			filtered.m01 = (float)m01.State;
			filtered.m02 = (float)m02.State;
			filtered.m03 = (float)m03.State;

			filtered.m10 = (float)m10.State;
			filtered.m11 = (float)m11.State;
			filtered.m12 = (float)m12.State;
			filtered.m13 = (float)m13.State;

			filtered.m20 = (float)m20.State;
			filtered.m21 = (float)m21.State;
			filtered.m22 = (float)m22.State;
			filtered.m23 = (float)m23.State;

			filtered.m30 = (float)m30.State;
			filtered.m31 = (float)m31.State;
			filtered.m32 = (float)m32.State;
			filtered.m33 = (float)m33.State;

            return filtered;
        }

        public void Dispose()
        {

        }

        #region Privates
        KalmanFilterSimple1D makeKalmanFilter(double q, double r, double f, double h)
        {
            var filter = new KalmanFilterSimple1D(q, r, f, h);
            // set initial value
            filter.SetState(1000.0, 50.0);
            return filter;
        }
        #endregion


    }
}