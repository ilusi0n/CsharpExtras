﻿using CsharpExtras.Extensions;
using CsharpExtras.Extensions;
using System;

namespace CsharpExtras.Enumerable.OneBased
{
    class OneBasedArray2DImpl<TVal> : IOneBasedArray2D<TVal>
    {
        public TVal[,] ZeroBasedEquivalent { get; }

        public OneBasedArray2DImpl(TVal[,] backingArray)
        {
            ZeroBasedEquivalent = backingArray;
        }

        public TVal this[int oneBasedIndex0, int oneBasedIndex1]
        {
            get
            {
                ValidateIndices(oneBasedIndex0, oneBasedIndex1);
                return ZeroBasedEquivalent[oneBasedIndex0 - 1, oneBasedIndex1 - 1];
            }
            set
            {
                ValidateIndices(oneBasedIndex0, oneBasedIndex1);
                ZeroBasedEquivalent[oneBasedIndex0 - 1, oneBasedIndex1 - 1] = value;
            }
        }

        private void ValidateIndices(int oneBasedIndex0, int oneBasedIndex1)
        {
            int len0 = ZeroBasedEquivalent.GetLength(0);
            int len1 = ZeroBasedEquivalent.GetLength(1);
            ValidateIndex(oneBasedIndex0, len0);
            ValidateIndex(oneBasedIndex1, len1);
        }

        private void ValidateIndex(int oneBasedIndex, int len)
        {
            if (oneBasedIndex < 1 || oneBasedIndex > len)
            {
                throw new IndexOutOfRangeException(string.Format(
                    "One-based index {0} out of bounds. Should be in the range [{1}, {2}].",
                    oneBasedIndex, 1, len));
            }
        }

        public int GetLength(int dimZeroBased) => ZeroBasedEquivalent.GetLength(dimZeroBased);

        public IOneBasedArray2D<TResult> Map<TResult>(Func<TVal, TResult> mapper)
        {
            TResult[,] mapped = ZeroBasedEquivalent.Map(mapper);
            return new OneBasedArray2DImpl<TResult>(mapped);
        }

        public IOneBasedArray2D<TResult> ZipArray<TOther, TResult>(Func<TVal, TOther, TResult> zipper, IOneBasedArray2D<TOther> other)
        {
            TResult[,] zipped = ZeroBasedEquivalent.ZipArray(zipper, other.ZeroBasedEquivalent);
            return new OneBasedArray2DImpl<TResult>(zipped);
        }

        public bool Any(Func<TVal, bool> checkerFunction)
        {
            return ZeroBasedEquivalent.Any(checkerFunction);
        }

        public bool All(Func<TVal, bool> checkerFunction)
        {
            return ZeroBasedEquivalent.All(checkerFunction);
        }

        public int Count()
        {
            return ZeroBasedEquivalent.Count();
        }

        public int Count(Func<TVal, bool> checkerFunction)
        {
            return ZeroBasedEquivalent.Count(checkerFunction);
        }

        public IOneBasedArray<TVal> SliceRow(int row)
        {
            return new OneBasedArrayImpl<TVal>(ZeroBasedEquivalent.SliceRow(row - 1));
        }

        public IOneBasedArray<TVal> SliceColumn(int column)
        {
            return new OneBasedArrayImpl<TVal>(ZeroBasedEquivalent.SliceColumn(column - 1));
        }

        public IOneBasedArray<TVal> FoldToSingleColumn(Func<TVal, TVal, TVal> foldFunction)
        {
            return new OneBasedArrayImpl<TVal>(ZeroBasedEquivalent.FoldToSingleColumn(foldFunction));
        }

        public IOneBasedArray<TVal> FoldToSingleRow(Func<TVal, TVal, TVal> foldFunction)
        {
            return new OneBasedArrayImpl<TVal>(ZeroBasedEquivalent.FoldToSingleRow(foldFunction));
        }

        public void WriteToRow(IOneBasedArray<TVal> values, int row, int offset)
        {
            ZeroBasedEquivalent.WriteToRow(values.ZeroBasedEquivalent, row - 1, offset);
        }

        public void WriteToColumn(IOneBasedArray<TVal> values, int column, int offset)
        {
            ZeroBasedEquivalent.WriteToColumn(values.ZeroBasedEquivalent, column - 1, offset);
        }

        public void WriteToArea(IOneBasedArray2D<TVal> values, int rowOffset, int columnOffset)
        {
            ZeroBasedEquivalent.WriteToArea(values.ZeroBasedEquivalent, rowOffset, columnOffset);
        }

        public IOneBasedArray2D<TVal> SubArray(int startAtRow, int startAtColumn, int stopBeforeRow, int stopBeforeColumn)
        {
            TVal[,] zeroBased = ZeroBasedEquivalent.SubArray(startAtRow - 1, startAtColumn - 1,
                stopBeforeRow - 1, stopBeforeColumn - 1);
            return new OneBasedArray2DImpl<TVal>(zeroBased);
        }
    }
}
