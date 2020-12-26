using System.Collections.Generic;
using System;
using System.Collections;

namespace Stos
{
    public class StosWTablicy<T> : IStos<T>, IEnumerable<T>
    {
        private T[] tab;
        private int szczyt = -1;
        public T this[int indexer] {get => tab[indexer];}
        public StosWTablicy(int size = 10)
        {
            tab = new T[size];
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
            if (szczyt == tab.Length - 1)
            {
                Array.Resize(ref tab, tab.Length*2);
            }

            szczyt++;
            tab[szczyt] = value;
        }

        public T[] ToArray()
        {
            //return tab;  //bardzo źle - reguły hermetyzacji

            //poprawnie:
            T[] temp = new T[szczyt + 1];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = tab[i];
            return temp;
        }


        //Metoda TrimExcess, zwiększająca wewnętrzną reprezentacje stosu, do proporcji: 90% zajęte/10% puste
        public void TrimExcess() {Array.Resize(ref tab, tab.Length*10/9);}


        //Metoda zwracająca tablicę tylko do odczytu
        public System.Collections.ObjectModel.ReadOnlyCollection<T> ToArrayReadOnly()
        {
            return Array.AsReadOnly(tab);
        }


        //Domyślny iterator wykorzystujący iterator StackEnumerator<T1>:
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        //Domyślny iterator wykorzystujący iterator z słowem kluczowym yield:

        // IEnumerator IEnumerable.GetEnumerator()
        // {
        //     return (IEnumerator)TopToBottom.GetEnumerator();
        // }

        // IEnumerator<T> IEnumerable<T>.GetEnumerator()
        // {
        //     return TopToBottom.GetEnumerator();
        // }

        //Enumerator StackEnumerator<T> - implementacja
        StackEnumerator<T> GetEnumerator()
        {
            return new StackEnumerator<T>(this);
        }

        //Enumeratory z słowem kluczowym yield
        public IEnumerable<T> TopToBottom
        {
            get
            {
                foreach(T element in tab)
                    if(element!=null) yield return element;
            }
        }

        public IEnumerable<T> BottomToTop
        {
            get
            {
                var bufor = tab;
                Array.Reverse(bufor);
                foreach(T element in bufor)
                {
                    if(element!=null) yield return element;
                }
            }
        }

        //Enumerator StackEnumerator<T> - definicja
        class StackEnumerator<T1> : IEnumerator<T1>
        {
            private StosWTablicy<T1> stack;
            private int index= -1;

            public StackEnumerator(StosWTablicy<T1> _stack)
            {
                stack=_stack;
            }

            public bool MoveNext()
            {
                try
                {
                    T1 test = stack[index+1];
                    if(test == null) return false;
                }
                catch(IndexOutOfRangeException)
                {
                    return false;
                }
                index++;
                return true;
            }

            public void Reset()
            {
                index = -1;
            }

            void IDisposable.Dispose() { }

            object IEnumerator.Current
            {
                get { return Current;}
            }

            public T1 Current
            {
                get { return stack[index]; }
            }
        }
    }
}
