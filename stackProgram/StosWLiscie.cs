using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Stos
{
    public class StosWLiscie<T> : IEnumerable<T>//, IStos<T>, 
    {
        private BasicLinkedList<T> tab;
        private int szczyt = -1;
        // public T this[int indexer] {get => tab[indexer];}
        public StosWLiscie()
        {
            tab = new BasicLinkedList<T>();
            szczyt = -1;
        }


        public T Peek => IsEmpty ? throw new StosEmptyException() : tab[szczyt];

        public int Count => szczyt + 1;

        public bool IsEmpty => szczyt == -1;

        public void Clear() => szczyt = -1;

        public T Pop()
        {
            if (IsEmpty)
                throw new StosEmptyException();

            szczyt--;
            return tab[szczyt + 1];
        }

        public void Push(T value)
        {
            tab.SetValue(value);
            if(!tab.Next())
            {
                tab.newNode();
                tab.Next();
            }
            szczyt++;
        }

        public T[] ToArray()
        {
            T[] temp = new T[szczyt + 1];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = tab[i];
            return temp;
        }


        //Domyślny iterator wykorzystujący iterator z słowem kluczowym yield:

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)TopToBottom.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return TopToBottom.GetEnumerator();
        }

        //Enumeratory z słowem kluczowym yield
        public IEnumerable<T> TopToBottom
        {
            get
            {
                foreach(T element in tab)
                    yield return element;
            }
        }

        public IEnumerable<T> BottomToTop
        {
            get
            {
                foreach(T element in tab.BottomToTop)
                {
                    yield return element;
                }
            }
        }

        class Node<T1>
        {
            public T1 element;
            public Node<T1> next;
            public Node<T1> prev;
            public Node(Node<T1> _prev = null)
            {
                prev = _prev;
            }
        }

        class BasicLinkedList<T1> : IEnumerable<T1>
        {
            private Node<T1> Head = new Node<T1>();
            private Node<T1> Current;
            public T1 this[int indexer]
            {
                get
                {
                    if(indexer <0) throw new ArgumentOutOfRangeException();
                    Node<T1> Bufor = Head;
                    for(int i = 0; i<indexer; i++)
                    {  
                        if(Bufor.next == null) throw new ArgumentOutOfRangeException();
                        Bufor = Bufor.next;
                    }
                    return Bufor.element;
                }
            }

            public BasicLinkedList()
            {
                Current = Head;
            }

            public void SetValue(T1 value) => Current.element = value;
            public T1 GetValue() => Current.element;

            public void newNode() => Current.next = new Node<T1>(Current);
                       
            public bool Next()
            {
                if(Current.next == null) return false;
                Current = Current.next;
                return true;
            }
            public bool Prev()
            {
                if(Current.prev == null) return false;
                Current = Current.prev;
                return true;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return (IEnumerator)TopToBottom.GetEnumerator();
            }

            IEnumerator<T1> IEnumerable<T1>.GetEnumerator()
            {
                return TopToBottom.GetEnumerator();
            }

            public IEnumerable<T1> TopToBottom
            {
                get
                {
                    Node<T1> Bufor = Head;
                    while(Bufor.next!=null)
                    {
                        if(Bufor.element!= null)yield return Bufor.element;
                        Bufor = Bufor.next;
                    }
                }
            }
            public IEnumerable<T1> BottomToTop
            {
                get
                {
                    Node<T1> Bufor = Current;
                    while(true)
                    {
                        if(Bufor.element!= null) yield return Bufor.element;
                        if(Bufor.prev!=null) Bufor = Bufor.prev;
                            else break;
                    }
                }
            }
        } 
    }       
}