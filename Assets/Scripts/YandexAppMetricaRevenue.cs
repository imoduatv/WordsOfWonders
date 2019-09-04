using System;
using System.Runtime.InteropServices;

[Serializable]
[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct YandexAppMetricaRevenue
{
	public double Price
	{
		get;
		private set;
	}

	public int? Quantity
	{
		get;
		set;
	}

	public string Currency
	{
		get;
		private set;
	}

	public string ProductID
	{
		get;
		set;
	}

	public YandexAppMetricaReceipt? Receipt
	{
		get;
		set;
	}

	public string Payload
	{
		get;
		set;
	}

	public YandexAppMetricaRevenue(double price, string currency)
	{
		Price = price;
		Quantity = null;
		Currency = currency;
		ProductID = null;
		Receipt = null;
		Payload = null;
	}
}
