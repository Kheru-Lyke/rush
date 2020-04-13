///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 12/11/2019 18:22
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.DefaultCompany.Rush.Common {
	public class ArrayOfArrays : ScriptableObject
    {
        [System.Serializable]
        public class Arrays
        {
            public int[] _array;

            public int this[int i]
            {
                get
                {
                    return _array[i];
                }
                set
                {
                    _array[i] = value;
                }
            }
        }
        public ArrayOfArrays[] array;
    }
}