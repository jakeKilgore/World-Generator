using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Collections.LowLevel.Unsafe;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class Utilities
    {
        public static T[] DynamicBufferToArray<T>(DynamicBuffer<T> buffer) where T : struct {
            T[] array = new T[buffer.Length];
            for(int i = 0; i < array.Length; i++) {
                array[i] = buffer[i];
            }
            return array;
        }
    }
}