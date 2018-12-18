using UnityEngine;

namespace Kalman
{

    /// <summary>
    /// Matrix kalman wrapper.
    /// </summary>
    public class MatrixKalmanWrapper : IKalmanWrapper
    {
        private KalmanFilter m00;
        private KalmanFilter m01;
        private KalmanFilter m02;
		private KalmanFilter m03;

		private KalmanFilter m10;
		private KalmanFilter m11;
		private KalmanFilter m12;
		private KalmanFilter m13;

		private KalmanFilter m20;
		private KalmanFilter m21;
		private KalmanFilter m22;
		private KalmanFilter m23;

		private KalmanFilter m30;
		private KalmanFilter m31;
		private KalmanFilter m32;
		private KalmanFilter m33;

        public MatrixKalmanWrapper()
        {
            /*
			X0 : predicted state
			P0 : predicted covariance
			
			F : factor of real value to previous real value
			B : the control-input model which is applied to the control vector uk;
			U : the control-input model which is applied to the control vector uk;
			Q : measurement noise
			H : factor of measured value to real value
			R : environment noise
			*/
            var f = new Matrix(new[,] { { 1.0, 1 }, { 0, 1.0 } });
            var b = new Matrix(new[,] { { 0.0 }, { 0 } });
            var u = new Matrix(new[,] { { 0.0 }, { 0 } });
            var r = Matrix.CreateVector(10);
            var q = new Matrix(new[,] { { 0.01, 0.4 }, { 0.1, 0.02 } });
            var h = new Matrix(new[,] { { 1.0, 0 } });

            m00 = makeKalmanFilter(f, b, u, q, h, r);
            m01 = makeKalmanFilter(f, b, u, q, h, r);
            m02 = makeKalmanFilter(f, b, u, q, h, r);
			m03 = makeKalmanFilter(f, b, u, q, h, r);

			m10 = makeKalmanFilter(f, b, u, q, h, r);
			m11 = makeKalmanFilter(f, b, u, q, h, r);
			m12 = makeKalmanFilter(f, b, u, q, h, r);
			m13 = makeKalmanFilter(f, b, u, q, h, r);

			m20 = makeKalmanFilter(f, b, u, q, h, r);
			m21 = makeKalmanFilter(f, b, u, q, h, r);
			m22 = makeKalmanFilter(f, b, u, q, h, r);
			m23 = makeKalmanFilter(f, b, u, q, h, r);

			m30 = makeKalmanFilter(f, b, u, q, h, r);
			m31 = makeKalmanFilter(f, b, u, q, h, r);
			m32 = makeKalmanFilter(f, b, u, q, h, r);
			m33 = makeKalmanFilter(f, b, u, q, h, r);
        }

		public Matrix4x4 Update(Matrix4x4 current)
        {
            m00.Correct(new Matrix(new double[,] { { current.m00 } }));
			m01.Correct(new Matrix(new double[,] { { current.m01 } }));
			m02.Correct(new Matrix(new double[,] { { current.m02 } }));
			m03.Correct(new Matrix(new double[,] { { current.m03 } }));

			m10.Correct(new Matrix(new double[,] { { current.m10 } }));
			m11.Correct(new Matrix(new double[,] { { current.m11 } }));
			m12.Correct(new Matrix(new double[,] { { current.m12 } }));
			m13.Correct(new Matrix(new double[,] { { current.m13 } }));

			m20.Correct(new Matrix(new double[,] { { current.m20 } }));
			m21.Correct(new Matrix(new double[,] { { current.m21 } }));
			m22.Correct(new Matrix(new double[,] { { current.m22 } }));
			m23.Correct(new Matrix(new double[,] { { current.m23 } }));

			m30.Correct(new Matrix(new double[,] { { current.m30 } }));
			m31.Correct(new Matrix(new double[,] { { current.m31 } }));
			m32.Correct(new Matrix(new double[,] { { current.m32 } }));
			m33.Correct(new Matrix(new double[,] { { current.m33 } }));


            // rashod
            // kX.State [1,0];
            // kY.State [1,0];
            // kZ.State [1,0];

			Matrix4x4 filtered = Matrix4x4.identity;

			filtered.m00 = (float) m00.State [0, 0];
			filtered.m01 = (float) m01.State [0, 0];
			filtered.m02 = (float) m02.State [0, 0];
			filtered.m03 = (float) m03.State [0, 0];

			filtered.m10 = (float) m10.State [0, 0];
			filtered.m11 = (float) m11.State [0, 0];
			filtered.m12 = (float) m12.State [0, 0];
			filtered.m13 = (float) m13.State [0, 0];

			filtered.m20 = (float) m20.State [0, 0];
			filtered.m21 = (float) m21.State [0, 0];
			filtered.m22 = (float) m22.State [0, 0];
			filtered.m23 = (float) m23.State [0, 0];

			filtered.m30 = (float) m30.State [0, 0];
			filtered.m31 = (float) m31.State [0, 0];
			filtered.m32 = (float) m32.State [0, 0];
			filtered.m33 = (float) m33.State [0, 0];

            return filtered;
        }

        public void Dispose()
        {

        }

        #region Privates
        KalmanFilter makeKalmanFilter(Matrix f, Matrix b, Matrix u, Matrix q, Matrix h, Matrix r)
        {
            var filter = new KalmanFilter(
                f.Duplicate(),
                b.Duplicate(),
                u.Duplicate(),
                q.Duplicate(),
                h.Duplicate(),
                r.Duplicate()
            );
            // set initial value
            filter.SetState(
                Matrix.CreateVector(500, 0),
                new Matrix(new[,] { { 10.0, 0 }, { 0, 5.0 } })
            );
            return filter;
        }
        #endregion



    }

}