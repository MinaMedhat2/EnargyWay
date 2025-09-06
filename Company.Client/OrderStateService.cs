// File: Company.Client/Services/OrderStateService.cs
// (ملاحظة: قمت بتصحيح الـ namespace ليكون متوافقاً مع باقي المشروع)
using Company.Client.Models;
using System.Collections.Generic; // <-- إضافة مهمة

namespace Company.Client.Services // <-- تأكد من أن الـ namespace صحيح
{
    // This service acts as a temporary memory for the order details
    // as the user moves from the checkout page to the payment page,
    // and finally to the confirmation page.
    public class OrderStateService
    {
        // Property to hold the data from the FIRST step (Checkout page)
        public ShippingInfoModel? ShippingInfo { get; private set; }

        // --- START: NEW ADDITION FOR CART ---
        // Property to hold the cart items during checkout
        public List<CartItem>? CartItems { get; private set; }
        // --- END: NEW ADDITION FOR CART ---

        // Property to hold the data from the SECOND step (Payment page)
        public FinalOrderModel? FinalOrder { get; private set; }

        // Method called from Checkout.razor to save shipping data
        public void SetShippingInfo(ShippingInfoModel shippingInfo)
        {
            this.ShippingInfo = shippingInfo;
        }

        // --- START: NEW METHOD FOR CART ---
        // Method called from CheckoutCart.razor to save the list of items
        public void SetCartItems(List<CartItem> items)
        {
            this.CartItems = items;
        }
        // --- END: NEW METHOD FOR CART ---

        // This method is called from the Payment page to store all the data.
        public void SetFinalOrder(FinalOrderModel order)
        {
            FinalOrder = order;
        }

        // This method is called after the order is successfully submitted to the database
        // to clear the memory and prevent reusing old data for a new order.
        public void ClearState()
        {
            ShippingInfo = null;
            FinalOrder = null;
            CartItems = null; // <-- إضافة مهمة
        }
    }
}
