using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kalman;

namespace CameraPoseFilter
{
	public class FilterManager : MonoBehaviour
	{

		private IKalmanWrapper kalman;

		private Matrix4x4 pastInputMatrix;
		private Matrix4x4 pastOutputMatrix;

		private FilterStyleConfig filterStyle = FilterStyleConfig.None;

		public FilterManager(FilterStyleConfig style = FilterStyleConfig.None )
		{
			pastInputMatrix = Matrix4x4.identity;
			pastOutputMatrix = Matrix4x4.identity;

			filterStyle = style;

			if (filterStyle == FilterStyleConfig.SimpleKalman) {
				kalman = new SimpleKalmanWrapper();
			} else if ( filterStyle == FilterStyleConfig.MatrixKalman ) {
				kalman = new MatrixKalmanWrapper();
			}
		}
			
		private Matrix4x4 SimpleLowPassFilter( Matrix4x4 inMatrix, float offset )
		{
			
			Matrix4x4 outMatrix = Matrix4x4.identity;

			outMatrix.m00 = offset * inMatrix.m00 + (1.0f - offset) * pastOutputMatrix.m00;
			outMatrix.m01 = offset * inMatrix.m01 + (1.0f - offset) * pastOutputMatrix.m01;
			outMatrix.m02 = offset * inMatrix.m02 + (1.0f - offset) * pastOutputMatrix.m02;
			outMatrix.m03 = offset * inMatrix.m03 + (1.0f - offset) * pastOutputMatrix.m03;
			outMatrix.m10 = offset * inMatrix.m10 + (1.0f - offset) * pastOutputMatrix.m10;
			outMatrix.m11 = offset * inMatrix.m11 + (1.0f - offset) * pastOutputMatrix.m11;
			outMatrix.m12 = offset * inMatrix.m12 + (1.0f - offset) * pastOutputMatrix.m12;
			outMatrix.m13 = offset * inMatrix.m13 + (1.0f - offset) * pastOutputMatrix.m13;
			outMatrix.m20 = offset * inMatrix.m20 + (1.0f - offset) * pastOutputMatrix.m20;
			outMatrix.m21 = offset * inMatrix.m21 + (1.0f - offset) * pastOutputMatrix.m21;
			outMatrix.m22 = offset * inMatrix.m22 + (1.0f - offset) * pastOutputMatrix.m22;
			outMatrix.m23 = offset * inMatrix.m23 + (1.0f - offset) * pastOutputMatrix.m23;
			outMatrix.m30 = offset * inMatrix.m30 + (1.0f - offset) * pastOutputMatrix.m30;
			outMatrix.m31 = offset * inMatrix.m31 + (1.0f - offset) * pastOutputMatrix.m31;
			outMatrix.m32 = offset * inMatrix.m32 + (1.0f - offset) * pastOutputMatrix.m32;
			outMatrix.m33 = offset * inMatrix.m33 + (1.0f - offset) * pastOutputMatrix.m33;

			pastOutputMatrix = outMatrix;

			return outMatrix;		
		}

		private float ExtendedLowPassFilterElement(float Input,  float SamplingFrequency, float CutOffFrequency, float PastInput, float PastOutput)
		{
			float Output;
			float a1,b0,b1,w0;
			w0 = 2.0f*3.14f*CutOffFrequency;
			a1 = (w0 - 2.0f*SamplingFrequency)/(2.0f*SamplingFrequency + w0);
			b0 = w0/(2*SamplingFrequency + w0);
			b1 = b0;

			Output = b0*(Input) + b1*(PastInput) - a1*(PastOutput);

			return Output;
		}

		private Matrix4x4 ExtendedLowPassFilter( Matrix4x4 curInput , float samplingFreq = 0.033f, float cutOff = 0.001f)
		{
			Matrix4x4 curOutput = Matrix4x4.identity;

			curOutput.m00 = ExtendedLowPassFilterElement (curInput.m00, samplingFreq, cutOff, pastInputMatrix.m00, pastOutputMatrix.m00);
			curOutput.m01 = ExtendedLowPassFilterElement (curInput.m01, samplingFreq, cutOff, pastInputMatrix.m01, pastOutputMatrix.m01);
			curOutput.m02 = ExtendedLowPassFilterElement (curInput.m02, samplingFreq, cutOff, pastInputMatrix.m02, pastOutputMatrix.m02);
			curOutput.m03 = ExtendedLowPassFilterElement (curInput.m03, samplingFreq, cutOff, pastInputMatrix.m03, pastOutputMatrix.m03);
			curOutput.m10 = ExtendedLowPassFilterElement (curInput.m10, samplingFreq, cutOff, pastInputMatrix.m10, pastOutputMatrix.m10);
			curOutput.m11 = ExtendedLowPassFilterElement (curInput.m11, samplingFreq, cutOff, pastInputMatrix.m11, pastOutputMatrix.m11);
			curOutput.m12 = ExtendedLowPassFilterElement (curInput.m12, samplingFreq, cutOff, pastInputMatrix.m12, pastOutputMatrix.m12);
			curOutput.m13 = ExtendedLowPassFilterElement (curInput.m13, samplingFreq, cutOff, pastInputMatrix.m13, pastOutputMatrix.m13);
			curOutput.m20 = ExtendedLowPassFilterElement (curInput.m20, samplingFreq, cutOff, pastInputMatrix.m20, pastOutputMatrix.m20);
			curOutput.m21 = ExtendedLowPassFilterElement (curInput.m21, samplingFreq, cutOff, pastInputMatrix.m21, pastOutputMatrix.m21);
			curOutput.m22 = ExtendedLowPassFilterElement (curInput.m22, samplingFreq, cutOff, pastInputMatrix.m22, pastOutputMatrix.m22);
			curOutput.m23 = ExtendedLowPassFilterElement (curInput.m23, samplingFreq, cutOff, pastInputMatrix.m23, pastOutputMatrix.m23);
			curOutput.m30 = ExtendedLowPassFilterElement (curInput.m30, samplingFreq, cutOff, pastInputMatrix.m30, pastOutputMatrix.m30);
			curOutput.m31 = ExtendedLowPassFilterElement (curInput.m31, samplingFreq, cutOff, pastInputMatrix.m31, pastOutputMatrix.m31);
			curOutput.m32 = ExtendedLowPassFilterElement (curInput.m32, samplingFreq, cutOff, pastInputMatrix.m32, pastOutputMatrix.m32);
			curOutput.m33 = ExtendedLowPassFilterElement (curInput.m33, samplingFreq, cutOff, pastInputMatrix.m33, pastOutputMatrix.m33);

			pastInputMatrix = curInput;
			pastOutputMatrix = curOutput;

			return curOutput;
		}
			
		public Matrix4x4 cameraPoseFilter( Matrix4x4 curInput )
		{
			Matrix4x4 curOutput = Matrix4x4.identity;

			if (filterStyle == FilterStyleConfig.Simple) {
				curOutput = SimpleLowPassFilter(curInput, 0.5f);
			} else if (filterStyle == FilterStyleConfig.Extended) {
				curOutput = ExtendedLowPassFilter( curInput, 0.033f, 0.001f );
			} else if( filterStyle == FilterStyleConfig.SimpleKalman || filterStyle == FilterStyleConfig.MatrixKalman  ) {
				curOutput = kalman.Update(curInput);
			} else {
				return curInput;
			}

			return curOutput;
		}
	}
}
