using System;
using System.Collections.Generic;
using UnityEngine;

namespace JTUtility
{
    [Serializable]
    public class SerializableUData
    {
        [Serializable] private class SttPair : PairedValue<UDStt, int> { }

        [SerializeField] private List<SttPair> stts;// State, used as bool

        [Serializable] private class Vc3Pair : PairedValue<UDVc3, Vector3> { }

        [SerializeField] private List<Vc3Pair> vc3s;

        [Serializable] private class V2iPair : PairedValue<UDV2I, Vector2Int> { }

        [SerializeField] private List<V2iPair> v2is;

        [Serializable] private class IntPair : PairedValue<UDInt, int> { }

        [SerializeField] private List<IntPair> ints;

        [Serializable] private class FltPair : PairedValue<UDFlt, float> { }

        [SerializeField] private List<FltPair> flts;

        [Serializable] private class StrPair : PairedValue<UDStr, string> { }

        [SerializeField] private List<StrPair> strs;

        [Serializable] private class RefPair : PairedValue<UDRef, UnityEngine.Object> { }

        [SerializeField] private List<RefPair> refs;

        public UData Deserialize()
        {
            UData uData = new UData();
            if (stts != null)
            {
                uData.stts = new Dictionary<UDStt, int>();
                uData.stts.AddRange(stts);
            }

            if (vc3s != null)
            {
                uData.vc3s = new Dictionary<UDVc3, Vector3>();
                uData.vc3s.AddRange(vc3s);
            }

            if (v2is != null)
            {
                uData.v2is = new Dictionary<UDV2I, Vector2Int>();
                uData.v2is.AddRange(v2is);
            }

            if (ints != null)
            {
                uData.ints = new Dictionary<UDInt, int>();
                uData.ints.AddRange(ints);
            }

            if (flts != null)
            {
                uData.flts = new Dictionary<UDFlt, float>();
                uData.flts.AddRange(flts);
            }

            if (strs != null)
            {
                uData.strs = new Dictionary<UDStr, string>();
                uData.strs.AddRange(strs);
            }

            if (refs != null)
            {
                uData.refs = new Dictionary<UDRef, object>();
                for (int i = 0; i < refs.Count; i++)
                {
                    uData.refs.Add(refs[i].Key, refs[i].Value);
                }
            }

            return uData;
        }
    }

    /// <summary>
    /// Universal Data.
    /// </summary>
    public class UData
    {
        public Dictionary<UDStt, int> stts = new Dictionary<UDStt, int>();// State, used as bool
        public Dictionary<UDVc3, Vector3> vc3s = new Dictionary<UDVc3, Vector3>();
        public Dictionary<UDV2I, Vector2Int> v2is = new Dictionary<UDV2I, Vector2Int>();
        public Dictionary<UDInt, int> ints = new Dictionary<UDInt, int>();
        public Dictionary<UDFlt, float> flts = new Dictionary<UDFlt, float>();
        public Dictionary<UDStr, string> strs = new Dictionary<UDStr, string>();
        public Dictionary<UDRef, object> refs = new Dictionary<UDRef, object>();

        #region index[]

        public int? this[UDStt idx]
        {
            get { if (stts != null && stts.ContainsKey(idx)) return stts[idx]; return null; }
            set
            {
                if (stts == null) stts = new Dictionary<UDStt, int>();
                if (value == null)
                    stts.TryRemove(idx);
                else
                    stts.TrySet(idx, value.Value);
            }
        }

        public Vector3? this[UDVc3 idx]
        {
            get { if (vc3s != null && vc3s.ContainsKey(idx)) return vc3s[idx]; return null; }
            set
            {
                if (vc3s == null) vc3s = new Dictionary<UDVc3, Vector3>();
                if (value == null)
                    vc3s.TryRemove(idx);
                else
                    vc3s.TrySet(idx, value.Value);
            }
        }

        public Vector2Int? this[UDV2I idx]
        {
            get { if (v2is != null && v2is.ContainsKey(idx)) return v2is[idx]; return null; }
            set
            {
                if (v2is == null) v2is = new Dictionary<UDV2I, Vector2Int>();
                if (value == null)
                    v2is.TryRemove(idx);
                else
                    v2is.TrySet(idx, value.Value);
            }
        }

        public int? this[UDInt idx]
        {
            get { if (ints != null && ints.ContainsKey(idx)) return ints[idx]; return null; }
            set
            {
                if (ints == null) ints = new Dictionary<UDInt, int>();
                if (value == null)
                    ints.TryRemove(idx);
                else
                    ints.TrySet(idx, value.Value);
            }
        }

        public float? this[UDFlt idx]
        {
            get { if (flts != null && flts.ContainsKey(idx)) return flts[idx]; return null; }
            set
            {
                if (flts == null) flts = new Dictionary<UDFlt, float>();
                if (value == null)
                    flts.TryRemove(idx);
                else
                    flts.TrySet(idx, value.Value);
            }
        }

        public string this[UDStr idx]
        {
            get { if (strs != null && strs.ContainsKey(idx)) return strs[idx]; return null; }
            set
            {
                if (strs == null) strs = new Dictionary<UDStr, string>();
                if (string.IsNullOrWhiteSpace(value))
                    strs.TryRemove(idx);
                else
                    strs.TrySet(idx, value);
            }
        }

        public object this[UDRef idx]
        {
            get { if (refs != null && refs.ContainsKey(idx)) return refs[idx]; return null; }
            set
            {
                if (refs == null) refs = new Dictionary<UDRef, object>();
                if (value.IsNull())
                    refs.TryRemove(idx);
                else
                    refs.TrySet(idx, value);
            }
        }

        #endregion index[]

        #region Set(Sequence)

        public UData Set(UDStt key, int value)
        {
            this[key] = value;
            return this;
        }

        public UData Set(UDVc3 key, Vector3 value)
        {
            this[key] = value;
            return this;
        }

        public UData Set(UDV2I key, Vector2Int value)
        {
            this[key] = value;
            return this;
        }

        public UData Set(UDInt key, int value)
        {
            this[key] = value;
            return this;
        }

        public UData Set(UDFlt key, float value)
        {
            this[key] = value;
            return this;
        }

        public UData Set(UDStr key, string value)
        {
            this[key] = value;
            return this;
        }

        public UData Set(UDRef key, object value)
        {
            this[key] = value;
            return this;
        }

        #endregion Set(Sequence)

        #region TryGet

        public int TryGet(UDStt key, int defaultValue = default)
        {
            if (stts.ContainsKey(key))
                return stts[key];
            return defaultValue;
        }

        public Vector3 TryGet(UDVc3 key, Vector3 defaultValue = default)
        {
            if (vc3s.ContainsKey(key))
                return vc3s[key];
            return defaultValue;
        }

        public Vector2Int TryGet(UDV2I key, Vector2Int defaultValue = default)
        {
            if (v2is.ContainsKey(key))
                return v2is[key];
            return defaultValue;
        }

        public int TryGet(UDInt key, int defaultValue = default)
        {
            if (ints.ContainsKey(key))
                return ints[key];
            return defaultValue;
        }

        public float TryGet(UDFlt key, float defaultValue = default)
        {
            if (flts.ContainsKey(key))
                return flts[key];
            return defaultValue;
        }

        public string TryGet(UDStr key, string defaultValue = "")
        {
            if (strs.ContainsKey(key))
                return strs[key];
            return defaultValue;
        }

        public object TryGet(UDRef key, object defaultValue = null)
        {
            if (refs.ContainsKey(key))
                return refs[key];
            return defaultValue;
        }

        #endregion TryGet

        public UData Merge(UData data, bool keepSelf = false)
        {
            if (data.stts != null)
            {
                foreach (var item in data.stts)
                {
                    if (this[item.Key].HasValue && keepSelf) continue;
                    this[item.Key] = item.Value;
                }
            }

            if (data.vc3s != null)
            {
                foreach (var item in data.vc3s)
                {
                    if (this[item.Key].HasValue && keepSelf) continue;
                    this[item.Key] = item.Value;
                }
            }

            if (data.v2is != null)
            {
                foreach (var item in data.v2is)
                {
                    if (this[item.Key].HasValue && keepSelf) continue;
                    this[item.Key] = item.Value;
                }
            }

            if (data.ints != null)
            {
                foreach (var item in data.ints)
                {
                    if (this[item.Key].HasValue && keepSelf) continue;
                    this[item.Key] = item.Value;
                }
            }

            if (data.flts != null)
            {
                foreach (var item in data.flts)
                {
                    if (this[item.Key].HasValue && keepSelf) continue;
                    this[item.Key] = item.Value;
                }
            }

            if (data.strs != null)
            {
                foreach (var item in data.strs)
                {
                    if (!string.IsNullOrWhiteSpace(this[item.Key]) && keepSelf) continue;
                    this[item.Key] = item.Value;
                }
            }

            if (data.refs != null)
            {
                foreach (var item in data.refs)
                {
                    if (this[item.Key].IsNotNull() && keepSelf) continue;
                    this[item.Key] = item.Value;
                }
            }

            return this;
        }

        public bool Contains(UDStt key)
        {
            return stts?.ContainsKey(key) == true;
        }

        public bool Contains(UDVc3 key)
        {
            return vc3s?.ContainsKey(key) == true;
        }

        public bool Contains(UDV2I key)
        {
            return v2is?.ContainsKey(key) == true;
        }

        public bool Contains(UDInt key)
        {
            return ints?.ContainsKey(key) == true;
        }

        public bool Contains(UDFlt key)
        {
            return flts?.ContainsKey(key) == true;
        }

        public bool Contains(UDStr key)
        {
            return strs?.ContainsKey(key) == true;
        }

        public bool Contains(UDRef key)
        {
            return refs.ContainsKey(key) == true;
        }

        public UData()
        {
        }

        public UData(UData data) : base()
        {
            if (data == null)
                return;

            if (data.stts != null)
            {
                stts = new Dictionary<UDStt, int>();
                stts.AddRange(data.stts);
            }

            if (data.vc3s != null)
            {
                vc3s = new Dictionary<UDVc3, Vector3>();
                vc3s.AddRange(data.vc3s);
            }

            if (data.v2is != null)
            {
                v2is = new Dictionary<UDV2I, Vector2Int>();
                v2is.AddRange(data.v2is);
            }

            if (data.ints != null)
            {
                ints = new Dictionary<UDInt, int>();
                ints.AddRange(data.ints);
            }

            if (data.flts != null)
            {
                flts = new Dictionary<UDFlt, float>();
                flts.AddRange(data.flts);
            }

            if (data.strs != null)
            {
                strs = new Dictionary<UDStr, string>();
                strs.AddRange(data.strs);
            }

            if (data.refs != null)
            {
                refs = new Dictionary<UDRef, object>();
                refs.AddRange(data.refs);
            }
        }
    }
}