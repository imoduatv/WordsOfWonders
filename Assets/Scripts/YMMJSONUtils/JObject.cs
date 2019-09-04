using System;
using System.Collections.Generic;

namespace YMMJSONUtils
{
	public class JObject
	{
		public JObjectKind Kind
		{
			get;
			private set;
		}

		public Dictionary<string, JObject> ObjectValue
		{
			get;
			private set;
		}

		public List<JObject> ArrayValue
		{
			get;
			private set;
		}

		public string StringValue
		{
			get;
			private set;
		}

		public bool BooleanValue
		{
			get;
			private set;
		}

		public int Count => (Kind == JObjectKind.Array) ? ArrayValue.Count : ((Kind == JObjectKind.Object) ? ObjectValue.Count : 0);

		public double DoubleValue
		{
			get;
			private set;
		}

		public float FloatValue
		{
			get;
			private set;
		}

		public ulong ULongValue
		{
			get;
			private set;
		}

		public long LongValue
		{
			get;
			private set;
		}

		public uint UIntValue
		{
			get;
			private set;
		}

		public int IntValue
		{
			get;
			private set;
		}

		public ushort UShortValue
		{
			get;
			private set;
		}

		public short ShortValue
		{
			get;
			private set;
		}

		public byte ByteValue
		{
			get;
			private set;
		}

		public sbyte SByteValue
		{
			get;
			private set;
		}

		public bool IsNegative
		{
			get;
			private set;
		}

		public bool IsFractional
		{
			get;
			private set;
		}

		public IntegerSize MinInteger
		{
			get;
			private set;
		}

		public FloatSize MinFloat
		{
			get;
			private set;
		}

		public JObject this[string key] => ObjectValue[key];

		public JObject this[int key] => ArrayValue[key];

		private JObject(string str)
		{
			Kind = JObjectKind.String;
			StringValue = str;
		}

		private JObject(bool b)
		{
			Kind = JObjectKind.Boolean;
			BooleanValue = b;
		}

		private JObject()
		{
			Kind = JObjectKind.Null;
		}

		private JObject(bool isNegative, bool isFractional, bool negativeExponent, ulong integerPart, ulong fractionalPart, int fractionalPartLength, ulong exponent)
		{
			Kind = JObjectKind.Number;
			if (!isFractional)
			{
				MakeInteger(isNegative, integerPart);
			}
			else
			{
				MakeFloat(isNegative, negativeExponent, integerPart, fractionalPart, fractionalPartLength, exponent);
			}
		}

		private JObject(List<JObject> list)
		{
			Kind = JObjectKind.Array;
			ArrayValue = list;
		}

		private JObject(Dictionary<string, JObject> dict)
		{
			Kind = JObjectKind.Object;
			ObjectValue = dict;
		}

		public static explicit operator string(JObject obj)
		{
			return obj.StringValue;
		}

		public static explicit operator bool(JObject obj)
		{
			return obj.BooleanValue;
		}

		public static explicit operator double(JObject obj)
		{
			return obj.DoubleValue;
		}

		public static explicit operator float(JObject obj)
		{
			return obj.FloatValue;
		}

		public static explicit operator ulong(JObject obj)
		{
			return obj.ULongValue;
		}

		public static explicit operator long(JObject obj)
		{
			return obj.LongValue;
		}

		public static explicit operator uint(JObject obj)
		{
			return obj.UIntValue;
		}

		public static explicit operator int(JObject obj)
		{
			return obj.IntValue;
		}

		public static explicit operator ushort(JObject obj)
		{
			return obj.UShortValue;
		}

		public static explicit operator short(JObject obj)
		{
			return obj.ShortValue;
		}

		public static explicit operator byte(JObject obj)
		{
			return obj.ByteValue;
		}

		public static explicit operator sbyte(JObject obj)
		{
			return obj.SByteValue;
		}

		public static JObject CreateString(string str)
		{
			return new JObject(str);
		}

		public static JObject CreateBoolean(bool b)
		{
			return new JObject(b);
		}

		public static JObject CreateNull()
		{
			return new JObject();
		}

		public static JObject CreateNumber(bool isNegative, bool isFractional, bool negativeExponent, ulong integerPart, ulong fractionalPart, int fractionalPartLength, ulong exponent)
		{
			return new JObject(isNegative, isFractional, negativeExponent, integerPart, fractionalPart, fractionalPartLength, exponent);
		}

		public static JObject CreateArray(List<JObject> list)
		{
			return new JObject(list);
		}

		public static JObject CreateObject(Dictionary<string, JObject> dict)
		{
			return new JObject(dict);
		}

		private void MakeInteger(bool isNegative, ulong integerPart)
		{
			IsNegative = isNegative;
			if (!IsNegative)
			{
				ULongValue = integerPart;
				MinInteger = IntegerSize.UInt64;
				if (ULongValue <= long.MaxValue)
				{
					LongValue = (long)ULongValue;
					MinInteger = IntegerSize.Int64;
				}
				if (ULongValue <= uint.MaxValue)
				{
					UIntValue = (uint)ULongValue;
					MinInteger = IntegerSize.UInt32;
				}
				if (ULongValue <= int.MaxValue)
				{
					IntValue = (int)ULongValue;
					MinInteger = IntegerSize.Int32;
				}
				if (ULongValue <= 65535)
				{
					UShortValue = (ushort)ULongValue;
					MinInteger = IntegerSize.UInt16;
				}
				if (ULongValue <= 32767)
				{
					ShortValue = (short)ULongValue;
					MinInteger = IntegerSize.Int16;
				}
				if (ULongValue <= 255)
				{
					ByteValue = (byte)ULongValue;
					MinInteger = IntegerSize.UInt8;
				}
				if (ULongValue <= 127)
				{
					SByteValue = (sbyte)ULongValue;
					MinInteger = IntegerSize.Int8;
				}
				DoubleValue = ULongValue;
				MinFloat = FloatSize.Double;
				if (DoubleValue <= 3.4028234663852886E+38)
				{
					FloatValue = (float)DoubleValue;
					MinFloat = FloatSize.Single;
				}
			}
			else
			{
				LongValue = (long)(0L - integerPart);
				MinInteger = IntegerSize.Int64;
				if (LongValue >= int.MinValue)
				{
					IntValue = (int)LongValue;
					MinInteger = IntegerSize.Int32;
				}
				if (LongValue >= -32768)
				{
					ShortValue = (short)LongValue;
					MinInteger = IntegerSize.Int16;
				}
				if (LongValue >= -128)
				{
					SByteValue = (sbyte)LongValue;
					MinInteger = IntegerSize.Int8;
				}
				DoubleValue = LongValue;
				MinFloat = FloatSize.Double;
				if (DoubleValue >= -3.4028234663852886E+38)
				{
					FloatValue = (float)DoubleValue;
					MinFloat = FloatSize.Single;
				}
			}
		}

		private void MakeFloat(bool isNegative, bool negativeExponent, ulong integerPart, ulong fractionalPart, int fractionalPartLength, ulong exponent)
		{
			DoubleValue = (double)((!isNegative) ? 1 : (-1)) * ((double)integerPart + (double)fractionalPart / Math.Pow(10.0, fractionalPartLength)) * Math.Pow(10.0, (long)((!negativeExponent) ? 1 : (-1)) * (long)exponent);
			MinFloat = FloatSize.Double;
			IsFractional = true;
			if (DoubleValue < 0.0)
			{
				IsNegative = true;
				if (DoubleValue >= -3.4028234663852886E+38)
				{
					FloatValue = (float)DoubleValue;
					MinFloat = FloatSize.Single;
				}
			}
			else if (DoubleValue <= 3.4028234663852886E+38)
			{
				FloatValue = (float)DoubleValue;
				MinFloat = FloatSize.Single;
			}
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(obj, this))
			{
				return true;
			}
			if (!(obj is JObject))
			{
				return false;
			}
			JObject jObject = (JObject)obj;
			if (jObject.Kind != Kind)
			{
				return false;
			}
			switch (Kind)
			{
			case JObjectKind.Array:
				if (ArrayValue.Count != jObject.ArrayValue.Count)
				{
					return false;
				}
				for (int i = 0; i < ArrayValue.Count; i++)
				{
					if (!ArrayValue[i].Equals(jObject.ArrayValue[i]))
					{
						return false;
					}
				}
				return true;
			case JObjectKind.Boolean:
				return BooleanValue == jObject.BooleanValue;
			case JObjectKind.Number:
				return EqualNumber(this, jObject);
			case JObjectKind.Object:
				if (ObjectValue.Count != jObject.ObjectValue.Count)
				{
					return false;
				}
				foreach (KeyValuePair<string, JObject> item in ObjectValue)
				{
					if (!jObject.ObjectValue.ContainsKey(item.Key) || !item.Value.Equals(jObject.ObjectValue[item.Key]))
					{
						return false;
					}
				}
				return true;
			case JObjectKind.String:
				return StringValue == jObject.StringValue;
			default:
				return true;
			}
		}

		public override int GetHashCode()
		{
			switch (Kind)
			{
			case JObjectKind.Array:
				return ArrayValue.GetHashCode();
			case JObjectKind.Boolean:
				return BooleanValue.GetHashCode();
			case JObjectKind.Null:
				return 0;
			case JObjectKind.Object:
				return ObjectValue.GetHashCode();
			case JObjectKind.String:
				return StringValue.GetHashCode();
			case JObjectKind.Number:
				if (IsFractional)
				{
					return DoubleValue.GetHashCode();
				}
				if (IsNegative)
				{
					return LongValue.GetHashCode();
				}
				return ULongValue.GetHashCode();
			default:
				return 0;
			}
		}

		public static bool EqualNumber(JObject o1, JObject o2)
		{
			if (o1.MinFloat != o2.MinFloat || o1.MinInteger != o2.MinInteger || o1.IsNegative != o2.IsNegative || o1.IsFractional != o2.IsFractional)
			{
				return false;
			}
			if (o1.IsFractional)
			{
				return o1.DoubleValue == o2.DoubleValue;
			}
			if (o1.IsNegative)
			{
				return o1.LongValue == o2.LongValue;
			}
			return o1.ULongValue == o2.ULongValue;
		}
	}
}
