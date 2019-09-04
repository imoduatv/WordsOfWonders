using System.Collections.Generic;

namespace Fabric.Answers.Internal
{
	internal class AnswersAndroidImplementation : IAnswers
	{
		private AnswersSharedInstanceJavaObject answersSharedInstance;

		public AnswersAndroidImplementation()
		{
			answersSharedInstance = new AnswersSharedInstanceJavaObject();
		}

		public void LogSignUp(string method, bool? success, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("SignUpEvent", customAttributes);
			answersEventInstanceJavaObject.PutMethod(method);
			answersEventInstanceJavaObject.PutSuccess(success);
			answersSharedInstance.Log("logSignUp", answersEventInstanceJavaObject);
		}

		public void LogLogin(string method, bool? success, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("LoginEvent", customAttributes);
			answersEventInstanceJavaObject.PutMethod(method);
			answersEventInstanceJavaObject.PutSuccess(success);
			answersSharedInstance.Log("logLogin", answersEventInstanceJavaObject);
		}

		public void LogShare(string method, string contentName, string contentType, string contentId, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("ShareEvent", customAttributes);
			answersEventInstanceJavaObject.PutMethod(method);
			answersEventInstanceJavaObject.PutContentName(contentName);
			answersEventInstanceJavaObject.PutContentType(contentType);
			answersEventInstanceJavaObject.PutContentId(contentId);
			answersSharedInstance.Log("logShare", answersEventInstanceJavaObject);
		}

		public void LogInvite(string method, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("InviteEvent", customAttributes);
			answersEventInstanceJavaObject.PutMethod(method);
			answersSharedInstance.Log("logInvite", answersEventInstanceJavaObject);
		}

		public void LogLevelStart(string level, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("LevelStartEvent", customAttributes);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putLevelName", level);
			answersSharedInstance.Log("logLevelStart", answersEventInstanceJavaObject);
		}

		public void LogLevelEnd(string level, double? score, bool? success, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("LevelEndEvent", customAttributes);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putLevelName", level);
			answersEventInstanceJavaObject.InvokeSafelyAsDouble("putScore", score);
			answersEventInstanceJavaObject.PutSuccess(success);
			answersSharedInstance.Log("logLevelEnd", answersEventInstanceJavaObject);
		}

		public void LogAddToCart(decimal? itemPrice, string currency, string itemName, string itemType, string itemId, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("AddToCartEvent", customAttributes);
			answersEventInstanceJavaObject.InvokeSafelyAsDecimal("putItemPrice", itemPrice);
			answersEventInstanceJavaObject.PutCurrency(currency);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putItemName", itemName);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putItemId", itemId);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putItemType", itemType);
			answersSharedInstance.Log("logAddToCart", answersEventInstanceJavaObject);
		}

		public void LogPurchase(decimal? price, string currency, bool? success, string itemName, string itemType, string itemId, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("PurchaseEvent", customAttributes);
			answersEventInstanceJavaObject.InvokeSafelyAsDecimal("putItemPrice", price);
			answersEventInstanceJavaObject.PutCurrency(currency);
			answersEventInstanceJavaObject.PutSuccess(success);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putItemName", itemName);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putItemId", itemId);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putItemType", itemType);
			answersSharedInstance.Log("logPurchase", answersEventInstanceJavaObject);
		}

		public void LogStartCheckout(decimal? totalPrice, string currency, int? itemCount, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("StartCheckoutEvent", customAttributes);
			answersEventInstanceJavaObject.InvokeSafelyAsDecimal("putTotalPrice", totalPrice);
			answersEventInstanceJavaObject.PutCurrency(currency);
			answersEventInstanceJavaObject.InvokeSafelyAsInt("putItemCount", itemCount);
			answersSharedInstance.Log("logStartCheckout", answersEventInstanceJavaObject);
		}

		public void LogRating(int? rating, string contentName, string contentType, string contentId, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("RatingEvent", customAttributes);
			answersEventInstanceJavaObject.InvokeSafelyAsInt("putRating", rating);
			answersEventInstanceJavaObject.PutContentName(contentName);
			answersEventInstanceJavaObject.PutContentType(contentType);
			answersEventInstanceJavaObject.PutContentId(contentId);
			answersSharedInstance.Log("logRating", answersEventInstanceJavaObject);
		}

		public void LogContentView(string contentName, string contentType, string contentId, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("ContentViewEvent", customAttributes);
			answersEventInstanceJavaObject.PutContentName(contentName);
			answersEventInstanceJavaObject.PutContentType(contentType);
			answersEventInstanceJavaObject.PutContentId(contentId);
			answersSharedInstance.Log("logContentView", answersEventInstanceJavaObject);
		}

		public void LogSearch(string query, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("SearchEvent", customAttributes);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putQuery", query);
			answersSharedInstance.Log("logSearch", answersEventInstanceJavaObject);
		}

		public void LogCustom(string eventName, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject eventInstance = new AnswersEventInstanceJavaObject("CustomEvent", customAttributes, eventName);
			answersSharedInstance.Log("logCustom", eventInstance);
		}
	}
}
