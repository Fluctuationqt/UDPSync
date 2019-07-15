using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPObjectSync.UDPSync.UDP
{
    public class SyncObjectRef<T>
    {
        public SyncObjectRef() { }
        public SyncObjectRef(T value) { Value = value; }
        public T Value { get; set; }
        public override string ToString()
        {
            T value = Value;
            return value == null ? "" : value.ToString();
        }
        public static implicit operator T(SyncObjectRef<T> r) { return r.Value; }
        public static implicit operator SyncObjectRef<T>(T value) { return new SyncObjectRef<T>(value); }
    }
}
