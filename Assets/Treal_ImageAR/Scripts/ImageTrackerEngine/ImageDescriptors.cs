/** 
*
* Copyright 2016-2018 SK Telecom. All Rights Reserved.
*
* This file is part of T real Platform.
*
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
*/

using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Treal.BrowserCore
{

    public class ImageDescriptors
    {

        private static IImageTrackerDescriptor nativePlugin;

        private IntPtr m_descriptorSet;

        ~ImageDescriptors()
        {
            ReleaseDescriptorSet();
        }

        public IntPtr LoadDescriptorSet(string path)
        {
			byte[] defaultBytes = Encoding.Default.GetBytes(path);

            byte[] convertedBytes;
            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {
                convertedBytes = Encoding.Convert(Encoding.Default, Encoding.GetEncoding("euc-kr"), defaultBytes);
            }
            else
            {
                convertedBytes = Encoding.Convert(Encoding.Default, Encoding.ASCII, defaultBytes);
            }

            byte[] convertedBytesNull = new byte[convertedBytes.Length + 1];
            convertedBytes.CopyTo(convertedBytesNull, 0);
            convertedBytesNull[convertedBytes.Length] = 0;

            //UnityEngine.Debug.LogWarning(ByteArrayToString(convertedBytes));

            GCHandle pinnedArray = GCHandle.Alloc(convertedBytesNull, GCHandleType.Pinned);
            IntPtr pathPtr = pinnedArray.AddrOfPinnedObject();

            if (nativePlugin == null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    nativePlugin = new ImageTrackerDescriptorIOSPlugin();
                }
                else if ( Application.platform == RuntimePlatform.Android )
                {
                    nativePlugin = new ImageTrackerDescriptorAndroidPlugin();
                }
                else 
                {
                    nativePlugin = new ImageTrackerDescriptorAndroidPlugin();
                }
            }
            m_descriptorSet = nativePlugin.idsLoadDescriptorSet_(pathPtr);

            pinnedArray.Free();

			return m_descriptorSet;
        }

        public void ReleaseDescriptorSet()
        {
            if (nativePlugin == null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    nativePlugin = new ImageTrackerDescriptorIOSPlugin();
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    nativePlugin = new ImageTrackerDescriptorAndroidPlugin();
                }
            }
            nativePlugin.idsReleaseDescriptorSet_(m_descriptorSet);
        }

        public IntPtr GetDescriptorSet()
        {
            return m_descriptorSet;
        }

        public interface IImageTrackerDescriptor
        {
            IntPtr idsLoadDescriptorSet_(IntPtr path);
            void idsReleaseDescriptorSet_(IntPtr descSet);
            void idsSaveDescriptorSet_(IntPtr descSet, IntPtr path);
            string idsGetID_(IntPtr descSet);
            IntPtr idsGetDescriptorByIndex_(IntPtr descSet, int index);
            IntPtr idsGetDescriptorByName_(IntPtr descSet, IntPtr descId);
            int idsGetNumDescriptor_(IntPtr descSet);
        }

        public class ImageTrackerDescriptorAndroidPlugin : IImageTrackerDescriptor
        {
            [DllImport("ImageTracker")]
            private static extern IntPtr idsLoadDescriptorSet(IntPtr path);
            [DllImport("ImageTracker")]
            private static extern void idsReleaseDescriptorSet(IntPtr descSet);
            [DllImport("ImageTracker")]
            private static extern void idsSaveDescriptorSet(IntPtr descSet, IntPtr path);
            [DllImport("ImageTracker")]
            private static extern string idsGetID(IntPtr descSet);
            [DllImport("ImageTracker")]
            private static extern IntPtr idsGetDescriptorByIndex(IntPtr descSet, int index);
            [DllImport("ImageTracker")]
            private static extern IntPtr idsGetDescriptorByName(IntPtr descSet, IntPtr descId);
            [DllImport("ImageTracker")]
            private static extern int idsGetNumDescriptor(IntPtr descSet);            


            public IntPtr idsLoadDescriptorSet_(IntPtr path)
            {
                return idsLoadDescriptorSet(path);
            }

            public void idsReleaseDescriptorSet_(IntPtr descSet)
            {
                idsReleaseDescriptorSet(descSet);
            }

            public void idsSaveDescriptorSet_(IntPtr descSet, IntPtr path)
            {
                idsSaveDescriptorSet(descSet, path);
            }

            public string idsGetID_(IntPtr descSet)
            {
                return idsGetID( descSet );
            }

            public IntPtr idsGetDescriptorByIndex_(IntPtr descSet, int index)
            {
                return idsGetDescriptorByIndex(descSet, index);
            }

            public IntPtr idsGetDescriptorByName_(IntPtr descSet, IntPtr descId)
            {
                return idsGetDescriptorByName(descSet, descId);
            }

            public int idsGetNumDescriptor_(IntPtr descSet)
            {
                return idsGetNumDescriptor(descSet);
            }
        }

        public class ImageTrackerDescriptorIOSPlugin : IImageTrackerDescriptor
        {
            [DllImport("__Internal")]
            private static extern IntPtr idsLoadDescriptorSet(IntPtr path);
            [DllImport("__Internal")]
            private static extern void idsReleaseDescriptorSet(IntPtr descSet);
            [DllImport("__Internal")]
            private static extern void idsSaveDescriptorSet(IntPtr descSet, IntPtr path);
            [DllImport("__Internal")]
            private static extern string idsGetID(IntPtr descSet);
            [DllImport("__Internal")]
            private static extern IntPtr idsGetDescriptorByIndex(IntPtr descSet, int index);
            [DllImport("__Internal")]
            private static extern IntPtr idsGetDescriptorByName(IntPtr descSet, IntPtr descId);
            [DllImport("__Internal")]
            private static extern int idsGetNumDescriptor(IntPtr descSet);

            public IntPtr idsLoadDescriptorSet_(IntPtr path)
            {
                return idsLoadDescriptorSet(path);
            }

            public void idsReleaseDescriptorSet_(IntPtr descSet)
            {
                idsReleaseDescriptorSet(descSet);
            }

            public void idsSaveDescriptorSet_(IntPtr descSet, IntPtr path)
            {
                idsSaveDescriptorSet(descSet, path);
            }

            public string idsGetID_(IntPtr descSet)
            {
                return idsGetID(descSet);
            }

            public IntPtr idsGetDescriptorByIndex_(IntPtr descSet, int index)
            {
                return idsGetDescriptorByIndex(descSet, index);
            }

            public IntPtr idsGetDescriptorByName_(IntPtr descSet, IntPtr descId)
            {
                return idsGetDescriptorByName(descSet, descId);
            }

            public int idsGetNumDescriptor_(IntPtr descSet)
            {
                return idsGetNumDescriptor(descSet);
            }
        }


    }
}

