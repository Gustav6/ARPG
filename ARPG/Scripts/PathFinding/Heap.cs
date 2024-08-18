﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class Heap<T>(int maxHeapSize) where T : IHeapItem<T>
    {
        T[] items = new T[maxHeapSize];
        int currentItemCount;
        public int Count { get { return currentItemCount; } }

        public void Add(ref T item)
        {
            item.HeapIndex = currentItemCount;
            items[currentItemCount] = item;
            SortUp(ref item);
            currentItemCount++;
        }

        public T RemoveFirst()
        {
            T firstItem = items[0];
            currentItemCount--;
            items[0] = items[currentItemCount];
            items[0].HeapIndex = 0;
            SortDown(items[0]);
            return firstItem;
        }

        public bool Contains(T item)
        {
            return Equals(items[item.HeapIndex], item);
        }

        public void UpdateItem(T item)
        {
            SortUp(ref item);
        }

        public void SortDown(T item)
        {
            while (true)
            {
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = item.HeapIndex * 2 + 2;
                int swapIndex = 0;

                if (childIndexLeft < currentItemCount)
                {
                    swapIndex = childIndexLeft;

                    if (childIndexRight < currentItemCount)
                    {
                        if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (item.CompareTo(items[swapIndex]) < 0)
                    {
                        Swap(ref item, ref items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void SortUp(ref T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                T parentItem = items[parentIndex];

                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(ref item, ref parentItem);
                }
                else
                {
                    break; 
                }

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        private void Swap(ref T itemA, ref T itemB)
        {
            (itemA, itemB) = (itemB, itemA);
            (itemA.HeapIndex, itemB.HeapIndex) = (itemB.HeapIndex, itemA.HeapIndex);
        }
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}
