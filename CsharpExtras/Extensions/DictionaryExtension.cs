﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Extensions
{
    public static class DictionaryExtension
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
        TKey key, TValue defaultValue)
        {
            return dictionary.GetValueOrDefault(key, () => defaultValue);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
             TKey key, Func<TValue> defaultValueProvider)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value
                 : defaultValueProvider();
        }

        public static IDictionary<TKey, TMapped> MapValues<TKey, TValue, TMapped>(this IDictionary<TKey, TValue> dictionary, Func<TValue, TMapped> mapper)
        {
            Dictionary<TKey, TMapped> dictToReturn = new Dictionary<TKey, TMapped>();
            foreach (TKey k in dictionary.Keys)
            {
                dictToReturn.Add(k, mapper(dictionary[k]));
            }
            return dictToReturn;
        }

        /// <summary>
        /// Zips together this dictionary with the other dictionary for all common keys.
        /// The keyset of the resulting dictionary will be the intersection of keysets of the two starting dictionaries.
        /// </summary>
        /// <typeparam name="TKey">The key type in both dictionaries</typeparam>
        /// <typeparam name="TValue">The value type in this dictionary</typeparam>
        /// <typeparam name="TOther">The value type in the other dictionary</typeparam>
        /// <typeparam name="TResult">The value type in the resultant dictionary</typeparam>
        /// <param name="zipper">The function that defines the values in the resultant dictionary.</param>
        /// <returns></returns>
        public static IDictionary<TKey, TResult> ZipValues<TKey, TValue, TOther, TResult>(
            this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TOther> other, Func<TValue, TOther, TResult> zipper)
        {
            IDictionary<TKey, TResult> resultDict = new Dictionary<TKey, TResult>();
            foreach(TKey key in dictionary.Keys)
            {
                if (other.ContainsKey(key))
                {
                    resultDict.Add(key, zipper(dictionary[key], other[key]));
                }
            }
            return resultDict;
        }

        public static IDictionary<TKey, TValue> FilterValues<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<TValue, bool> filter)
        {
            Dictionary<TKey, TValue> dictToReturn = new Dictionary<TKey, TValue>();
            foreach (TKey k in dictionary.Keys)
            {
                TValue value = dictionary[k];
                if (filter(value))
                {
                    dictToReturn.Add(k, value);
                }
            }
            return dictToReturn;
        }

        public static void IncrementValue<TKey>(this IDictionary<TKey, int> dictionary, TKey key, int incrementAmount = 1)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] += incrementAmount;
            }
            else
            {
                dictionary[key] = incrementAmount;
            }
        }
    }
}
