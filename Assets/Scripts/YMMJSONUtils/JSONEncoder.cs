using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace YMMJSONUtils
{
	public class JSONEncoder
	{
		private readonly StringBuilder _buffer = new StringBuilder();

		internal static readonly Dictionary<char, string> EscapeChars = new Dictionary<char, string>
		{
			{
				'"',
				"\\\""
			},
			{
				'\\',
				"\\\\"
			},
			{
				'\b',
				"\\b"
			},
			{
				'\f',
				"\\f"
			},
			{
				'\n',
				"\\n"
			},
			{
				'\r',
				"\\r"
			},
			{
				'\t',
				"\\t"
			},
			{
				'\u2028',
				"\\u2028"
			},
			{
				'\u2029',
				"\\u2029"
			}
		};

		private JSONEncoder()
		{
		}

		public static string Encode(object obj)
		{
			JSONEncoder jSONEncoder = new JSONEncoder();
			jSONEncoder.EncodeObject(obj);
			return jSONEncoder._buffer.ToString();
		}

		private void EncodeObject(object obj)
		{
			if (obj == null)
			{
				EncodeNull();
				return;
			}
			if (obj is string)
			{
				EncodeString((string)obj);
				return;
			}
			if (obj is float)
			{
				EncodeFloat((float)obj);
				return;
			}
			if (obj is double)
			{
				EncodeDouble((double)obj);
				return;
			}
			if (obj is int)
			{
				EncodeLong((int)obj);
				return;
			}
			if (obj is uint)
			{
				EncodeULong((uint)obj);
				return;
			}
			if (obj is long)
			{
				EncodeLong((long)obj);
				return;
			}
			if (obj is ulong)
			{
				EncodeULong((ulong)obj);
				return;
			}
			if (obj is short)
			{
				EncodeLong((short)obj);
				return;
			}
			if (obj is ushort)
			{
				EncodeULong((ushort)obj);
				return;
			}
			if (obj is byte)
			{
				EncodeULong((byte)obj);
				return;
			}
			if (obj is bool)
			{
				EncodeBool((bool)obj);
				return;
			}
			if (obj is IDictionary)
			{
				EncodeDictionary((IDictionary)obj);
				return;
			}
			if (obj is IEnumerable)
			{
				EncodeEnumerable((IEnumerable)obj);
				return;
			}
			if (obj is Enum)
			{
				EncodeObject(Convert.ChangeType(obj, Enum.GetUnderlyingType(obj.GetType())));
				return;
			}
			if (obj is JObject)
			{
				JObject jObject = (JObject)obj;
				switch (jObject.Kind)
				{
				case JObjectKind.Array:
					EncodeEnumerable(jObject.ArrayValue);
					break;
				case JObjectKind.Boolean:
					EncodeBool(jObject.BooleanValue);
					break;
				case JObjectKind.Null:
					EncodeNull();
					break;
				case JObjectKind.Number:
					if (jObject.IsFractional)
					{
						EncodeDouble(jObject.DoubleValue);
					}
					else if (jObject.IsNegative)
					{
						EncodeLong(jObject.LongValue);
					}
					else
					{
						EncodeULong(jObject.ULongValue);
					}
					break;
				case JObjectKind.Object:
					EncodeDictionary(jObject.ObjectValue);
					break;
				case JObjectKind.String:
					EncodeString(jObject.StringValue);
					break;
				default:
					throw new ArgumentException("Can't serialize object of type " + obj.GetType().Name, "obj");
				}
				return;
			}
			throw new ArgumentException("Can't serialize object of type " + obj.GetType().Name, "obj");
		}

		private void EncodeNull()
		{
			_buffer.Append("null");
		}

		private void EncodeString(string str)
		{
			_buffer.Append('"');
			foreach (char c in str)
			{
				if (EscapeChars.ContainsKey(c))
				{
					_buffer.Append(EscapeChars[c]);
				}
				else if (c > '\u0080' || c < ' ')
				{
					_buffer.Append("\\u" + Convert.ToString(c, 16).ToUpper(CultureInfo.InvariantCulture).PadLeft(4, '0'));
				}
				else
				{
					_buffer.Append(c);
				}
			}
			_buffer.Append('"');
		}

		private void EncodeFloat(float f)
		{
			_buffer.Append(f.ToString(CultureInfo.InvariantCulture));
		}

		private void EncodeDouble(double d)
		{
			_buffer.Append(d.ToString(CultureInfo.InvariantCulture));
		}

		private void EncodeLong(long l)
		{
			_buffer.Append(l.ToString(CultureInfo.InvariantCulture));
		}

		private void EncodeULong(ulong l)
		{
			_buffer.Append(l.ToString(CultureInfo.InvariantCulture));
		}

		private void EncodeBool(bool b)
		{
			_buffer.Append((!b) ? "false" : "true");
		}

		private void EncodeDictionary(IDictionary d)
		{
			bool flag = true;
			_buffer.Append('{');
			IDictionaryEnumerator enumerator = d.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
					if (!(dictionaryEntry.Key is string))
					{
						throw new ArgumentException("Dictionary keys must be strings", "d");
					}
					if (!flag)
					{
						_buffer.Append(',');
					}
					EncodeString((string)dictionaryEntry.Key);
					_buffer.Append(':');
					EncodeObject(dictionaryEntry.Value);
					flag = false;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			_buffer.Append('}');
		}

		private void EncodeEnumerable(IEnumerable e)
		{
			bool flag = true;
			_buffer.Append('[');
			IEnumerator enumerator = e.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					if (!flag)
					{
						_buffer.Append(',');
					}
					EncodeObject(current);
					flag = false;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			_buffer.Append(']');
		}
	}
}
