using Fabric.Answers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    private string currency;

	private Dictionary<string, string> prices;

	public static string coinpack1 = "wowcoinpack1";

	public static string coinpack2 = "wowcoinpack2";

	public static string coinpack3 = "wowcoinpack3";

	public static string coinpack4 = "wowcoinpack4";

	public static string coinpack5 = "wowcoinpack5";

	public static string removeads = "wowremoveads";

	public static string subscription = "wowpro";

	public static string trialsubscription = "wowprotrial2";

	private void Start()
	{
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
        else
        {
            CheckSubscription();
        }
    }

	public void InitializePurchasing()
	{
        if (!IsInitialized())
        {
            ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            configurationBuilder.AddProduct(coinpack1, ProductType.Consumable);
            configurationBuilder.AddProduct(coinpack2, ProductType.Consumable);
            configurationBuilder.AddProduct(coinpack3, ProductType.Consumable);
            configurationBuilder.AddProduct(coinpack4, ProductType.Consumable);
            configurationBuilder.AddProduct(coinpack5, ProductType.Consumable);
            configurationBuilder.AddProduct(removeads, ProductType.NonConsumable);
            configurationBuilder.AddProduct(subscription, ProductType.Subscription);
            configurationBuilder.AddProduct(trialsubscription, ProductType.Subscription);
            UnityPurchasing.Initialize(this, configurationBuilder);
        }
    }

	private bool IsInitialized()
	{
        return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	public void BuyProduct(string id)
	{
		BuyProductID(id);
	}

	private void BuyProductID(string productId)
	{
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                UnityEngine.Debug.Log($"Purchasing product asychronously: '{product.definition.id}'");
                ShopScript.instance.ShowWaiting();
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                UnityEngine.Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                ShopScript.instance.HideWaiting();
            }
        }
        else
        {
            UnityEngine.Debug.Log("BuyProductID FAIL. Not initialized.");
            ShopScript.instance.HideWaiting();
        }
    }

	public void RestorePurchases()
	{
        if (!IsInitialized())
        {
            UnityEngine.Debug.Log("RestorePurchases FAIL. Not initialized.");
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            UnityEngine.Debug.Log("RestorePurchases started ...");
            IAppleExtensions extension = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            extension.RestoreTransactions(delegate (bool result)
            {
                UnityEngine.Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            UnityEngine.Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        UnityEngine.Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
        currency = m_StoreController.products.WithID(removeads).metadata.isoCurrencyCode;
        ShopScript.itemPrices[0] = m_StoreController.products.WithID(removeads).metadata.localizedPrice.ToString() + " " + currency;
        ShopScript.itemPrices[1] = m_StoreController.products.WithID(coinpack1).metadata.localizedPrice.ToString() + " " + currency;
        ShopScript.itemPrices[2] = m_StoreController.products.WithID(coinpack2).metadata.localizedPrice.ToString() + " " + currency;
        ShopScript.itemPrices[3] = m_StoreController.products.WithID(coinpack3).metadata.localizedPrice.ToString() + " " + currency;
        ShopScript.itemPrices[4] = m_StoreController.products.WithID(coinpack4).metadata.localizedPrice.ToString() + " " + currency;
        ShopScript.itemPrices[5] = m_StoreController.products.WithID(coinpack5).metadata.localizedPrice.ToString() + " " + currency;
        ShopScript.itemPrices[6] = m_StoreController.products.WithID(subscription).metadata.localizedPrice.ToString() + " " + currency;
        ShopScript.itemPrices[7] = m_StoreController.products.WithID(trialsubscription).metadata.localizedPrice.ToString() + " " + currency;
        ShopScript.instance.CreateShopItems();
        prices = new Dictionary<string, string>();
        prices.Add(removeads, m_StoreController.products.WithID(removeads).metadata.localizedPrice.ToString());
        prices.Add(coinpack1, m_StoreController.products.WithID(coinpack1).metadata.localizedPrice.ToString());
        prices.Add(coinpack2, m_StoreController.products.WithID(coinpack2).metadata.localizedPrice.ToString());
        prices.Add(coinpack3, m_StoreController.products.WithID(coinpack3).metadata.localizedPrice.ToString());
        prices.Add(coinpack4, m_StoreController.products.WithID(coinpack4).metadata.localizedPrice.ToString());
        prices.Add(coinpack5, m_StoreController.products.WithID(coinpack5).metadata.localizedPrice.ToString());
        prices.Add(subscription, m_StoreController.products.WithID(subscription).metadata.localizedPrice.ToString());
        prices.Add(trialsubscription, m_StoreController.products.WithID(trialsubscription).metadata.localizedPrice.ToString());
        UnityEngine.Debug.Log("parse inapp products...");
        CheckSubscription();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        UnityEngine.Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (string.Equals(args.purchasedProduct.definition.id, removeads, StringComparison.Ordinal))
        {
            UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            ShopScript.instance.NoAdCallback();
        }
        else if (string.Equals(args.purchasedProduct.definition.id, coinpack1, StringComparison.Ordinal))
        {
            UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            ShopScript.instance.FirstGoldPackCallback();
        }
        else if (string.Equals(args.purchasedProduct.definition.id, coinpack2, StringComparison.Ordinal))
        {
            UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            ShopScript.instance.SecondGoldPackCallback();
        }
        else if (string.Equals(args.purchasedProduct.definition.id, coinpack3, StringComparison.Ordinal))
        {
            UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            ShopScript.instance.ThirdGoldPackCallback();
        }
        else if (string.Equals(args.purchasedProduct.definition.id, coinpack4, StringComparison.Ordinal))
        {
            UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            ShopScript.instance.FourthGoldPackCallback();
        }
        else if (string.Equals(args.purchasedProduct.definition.id, coinpack5, StringComparison.Ordinal))
        {
            UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            ShopScript.instance.FifthGoldPackCallback();
        }
        else if (string.Equals(args.purchasedProduct.definition.id, subscription, StringComparison.Ordinal))
        {
            UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            ShopScript.instance.SubscriptionCallback();
        }
        else if (string.Equals(args.purchasedProduct.definition.id, trialsubscription, StringComparison.Ordinal))
        {
            UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            ShopScript.instance.SubscriptionCallback();
        }
        else
        {
            UnityEngine.Debug.Log($"ProcessPurchase: FAIL. Unrecognized product: '{args.purchasedProduct.definition.id}'");
        }
        ShopScript.instance.HideWaiting();
        try
        {
            Answers.LogPurchase(decimal.Parse(prices[args.purchasedProduct.definition.id]), currency, true, args.purchasedProduct.definition.id, null, args.purchasedProduct.definition.id);
            GoogleAnalyticsScript.instance.AdjustPurcaseEvent(args.purchasedProduct.transactionID, double.Parse(prices[args.purchasedProduct.definition.id]), currency);
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Fabric Logpurchase Exception: " + ex.Message);
        }
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        UnityEngine.Debug.Log($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
        ShopScript.instance.HideWaiting();
    }

    private void CheckSubscription()
	{
        bool flag = false;
        Product[] all = m_StoreController.products.all;
        foreach (Product product in all)
        {
            GooglePurchaseData googlePurchaseData = new GooglePurchaseData(product.receipt);
            if (product.hasReceipt)
            {
                UnityEngine.Debug.Log(googlePurchaseData.json.autoRenewing);
                UnityEngine.Debug.Log(googlePurchaseData.json.productId);
                if (googlePurchaseData.json.productId == subscription && googlePurchaseData.json.autoRenewing.ToLower() == "true")
                {
                    UnityEngine.Debug.Log("Subscription active");
                    flag = true;
                }
                if (googlePurchaseData.json.productId == trialsubscription && googlePurchaseData.json.autoRenewing.ToLower() == "true")
                {
                    UnityEngine.Debug.Log("Subscription active");
                    flag = true;
                }
            }
        }
        if (!flag && PlayerPrefsManager.GetPro())
        {
            UnityEngine.Debug.Log("Subscription deactivated.");
            MenuController.instance.DisablePro();
        }
    }
}
