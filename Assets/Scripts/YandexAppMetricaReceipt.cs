using System;
using System.Runtime.InteropServices;

[Serializable]
[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct YandexAppMetricaReceipt
{
	public string Data
	{
		get;
		set;
	}

	public string Signature
	{
		get;
		set;
	}

	public string TransactionID
	{
		get;
		set;
	}
}
