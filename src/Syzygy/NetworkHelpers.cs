using System;
using System.Runtime.InteropServices;
using Lidgren.Network;

namespace Syzygy
{
    static class NetworkHelpers
    {
        public static void Write<T>(this NetBuffer buffer, T s)
            where T : struct
        {
            buffer.Write(ref s);
        }

        public static void Write<T>(this NetBuffer buffer, ref T s)
            where T : struct
        {
            buffer.Write(ToByteArray(ref s));
        }

        public static T Read<T>(this NetBuffer buffer)
            where T : struct
        {
            T s;
            buffer.Read(out s);
            return s;
        }

        public static void Read<T>(this NetBuffer buffer, out T s)
            where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            var array = buffer.ReadBytes(size);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(array, 0, ptr, size);
            s = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
        }

        public static byte[] ToByteArray<T>(this T s)
            where T : struct
        {
            return ToByteArray(ref s);
        }

        public static byte[] ToByteArray<T>(ref T s)
            where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            var array = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(s, ptr, true);
            Marshal.Copy(ptr, array, 0, size);
            Marshal.FreeHGlobal(ptr);
            return array;
        }

        public static T ToStruct<T>(this byte[] array)
            where T : struct
        {
            T s;
            ToStruct(array, out s);
            return s;
        }

        public static void ToStruct<T>(byte[] array, out T s)
            where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            if(size != array.Length)
                throw new Exception("Size of array and structure do not match.");
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(array, 0, ptr, size);
            s = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
        }
    }
}
