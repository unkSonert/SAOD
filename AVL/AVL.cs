using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVL
{
    class KeyValueNode<K, V>
    {
        public K Key;
        public V Value;
        public int Height = 1;
        public KeyValueNode<K, V> Left;
        public KeyValueNode<K, V> Right;

        public static implicit operator KeyValuePair<K, V>(KeyValueNode<K, V> node) => new KeyValuePair<K, V>(node.Key, node.Value); 
    }

    class AVL<K, V> : IDictionary<K, V>
        where K : IComparable<K>
    {
        KeyValueNode<K, V> rootNode;

        public V this[K key]
        {
            get
            {
                var (find, node) = FindValue(key);

                if (!find)
                    throw new KeyNotFoundException("Meow");

                return node.Value;
            }
            set
            {
                var (find, node) = FindValue(key);

                if (find)
                    node.Value = value;
                else
                    Add(key, value);
            }
        }

        public int Size { get; private set; }

        private ICollection<K> keys()
        {
            var result = new List<K>();
            foreach (var iter in this)
            {
                result.Add(iter.Key);
            }
            return result;
        }

        private ICollection<V> values()
        {
            var result = new List<V>();
            foreach (var iter in this)
            {
                result.Add(iter.Value);
            }
            return result;
        }

        public int Count => Size;

        public bool IsReadOnly => false;

        ICollection<K> IDictionary<K, V>.Keys => keys();

        ICollection<V> IDictionary<K, V>.Values => values();

        public void Add(K key, V value)
        {
            rootNode = insert(rootNode, key, value);
        }

        public void Add(KeyValuePair<K, V> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            rootNode = null;
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            var (find, node) = FindValue(item.Key);
            return find && node.Value.Equals(item.Value);
        }

        public bool ContainsKey(K key)
        {
            return FindValue(key).Item1;
        }

        private (bool, KeyValueNode<K, V>) FindValue(K key)
        {
            var node = rootNode;

            while (node != null)
            {
                int result = key.CompareTo(node.Key);

                if (result < 0)
                    node = node.Left;
                else if (result > 0)
                    node = node.Right;
                else
                    return (true, node);
            }

            return (false, default);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            List<KeyValuePair<K, V>> inArray = new List<KeyValuePair<K, V>>(this);
            Array.Copy(inArray.ToArray(), arrayIndex, array, 0, array.Length - 1);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() //https://cdn.discordapp.com/attachments/712624104242675764/717748448278872164/unknown.png
        {
            var node = rootNode;

            Stack<KeyValueNode<K, V>> order = new Stack<KeyValueNode<K, V>>();
        STEP3:
            while (node != null)
            {
                order.Push(node);
                node = node.Left;
            }

            if (order.Count != 0)
            {
                node = order.Pop();
                yield return node;

                node = node.Right;
                goto STEP3;
            }
        }

        public bool Remove(K key)
        {
            return Remove(new KeyValuePair<K, V>(key, default(V)));
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            bool result;
            (result, rootNode) = remove(rootNode, item.Key, item.Value);
            return result;
        }

        public bool TryGetValue(K key, out V value)
        {
            var (find, node) = FindValue(key);
            value = node.Value;
            return find;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private KeyValueNode<K, V> insert(KeyValueNode<K, V> node, K key, V value)
        {
            if (node == null)
            {
                Size++;
                return new KeyValueNode<K, V> { Key = key, Value = value };
            }

            int result = key.CompareTo(node.Key);

            if (result < 0)
                node.Left = insert(node.Left, key, value);
            else if (result > 0)
                node.Right = insert(node.Right, key, value);
            else
                return node;

            return balance(node);
        }

        private static KeyValueNode<K, V> balance(KeyValueNode<K, V> node)
        {
            fixHeight(node);
            switch (BalanceFactor(node))
            {
                case 2:
                    {
                        if (BalanceFactor(node.Right) < 0)
                            node.Right = RotateRight(node.Right);
                        return RotateLeft(node);
                    }
                case -2:
                    {
                        if (BalanceFactor(node.Left) > 0)
                            node.Left = RotateLeft(node.Left);
                        return RotateRight(node);
                    }
                default:
                    return node;
            }
        }

        private static int BalanceFactor(KeyValueNode<K, V> node)
        {
            return Height(node.Right) - Height(node.Left);
        }

        private static void fixHeight(KeyValueNode<K, V> node)
        {
            var left = Height(node.Left);
            var right = Height(node.Right);

            node.Height = Math.Max(left, right) + 1;
        }

        private static int Height(KeyValueNode<K, V> node)
        {
            if (node == null)
                return 0;
            return node.Height;
        }

        private static KeyValueNode<K, V> RotateLeft(KeyValueNode<K, V> node)
        {
            KeyValueNode<K, V> right = node.Right;
            node.Right = right.Left;
            right.Left = node;

            fixHeight(node);
            fixHeight(right);
            return right;
        }

        private static KeyValueNode<K, V> RotateRight(KeyValueNode<K, V> node)
        {
            KeyValueNode<K, V> left = node.Left;
            node.Left = left.Right;
            left.Right = node;

            fixHeight(node);
            fixHeight(left);
            return left;
        }

        private (bool, KeyValueNode<K, V>) remove(KeyValueNode<K, V> node, K key, V value)
        {
            if (node == null)
                return (false, null);
            bool result;
            int res = key.CompareTo(node.Key);

            if (res < 0)
                (result, node.Left) = remove(node.Left, key, value);
            else if (res > 0)
                (result, node.Right) = remove(node.Right, key, value);
            else
            {
                if (!node.Value.Equals(value) && !value.Equals(default(V)))
                    return (false, rootNode);

                KeyValueNode<K, V> left = node.Left;
                KeyValueNode<K, V> right = node.Right;

                Size--;

                if (right == null)
                    return (true, left);

                KeyValueNode<K, V> min = FindMinNode(right);

                min.Right = RemoveMin(right);
                min.Left = left;

                return (true, balance(min));
            }

            return (false, balance(node));
        }

        private static KeyValueNode<K, V> FindMinNode(KeyValueNode<K, V> node)
        {
            if (node != null)
            {
                while (node.Left != null)
                {
                    node = node.Left;
                }
            }
            return node;
        }

        private static KeyValueNode<K, V> RemoveMin(KeyValueNode<K, V> node)
        {
            if (node.Left == null)
            {
                return node.Right;
            }

            node.Left = RemoveMin(node.Left);

            return balance(node);
        }
    }
}
