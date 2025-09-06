using Company.Client.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Company.Client.Services
{
    public class CartService
    {
        private readonly IJSRuntime _jsRuntime;
        // --- الخطوة 1: حقن AuthService ---
        // نحتاج هذه الخدمة لمعرفة ما إذا كان المستخدم مسجلاً أم لا.
        private readonly AuthService _authService;

        public event Func<Task>? CartChanged;

        // --- الخطوة 2: تعديل الـ Constructor ---
        // نقوم باستقبال AuthService هنا.
        public CartService(IJSRuntime jsRuntime, AuthService authService)
        {
            _jsRuntime = jsRuntime;
            _authService = authService;
        }

        // --- الخطوة 3: الدالة الذكية لتوليد مفتاح التخزين ---
        // هذه هي أهم دالة جديدة. هي التي تحدد اسم السلة في الـ Local Storage.
        private async Task<string> GetCartStorageKeyAsync()
        {
            // إذا كان المستخدم مسجلاً، استخدم اسم المستخدم كمفتاح فريد.
            if (_authService.IsLoggedIn && _authService.CurrentUser != null)
            {
                // مثال: "shoppingCart_mina.mosa"
                return $"shoppingCart_{_authService.CurrentUser.Username}";
            }
            else
            {
                // إذا كان المستخدم زائرًا، تحقق مما إذا كان لديه "معرّف زائر" (Guest ID).
                var guestId = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "guestCartId");

                // إذا لم يكن لديه معرّf، قم بإنشاء واحد جديد عشوائي.
                if (string.IsNullOrEmpty(guestId))
                {
                    guestId = Guid.NewGuid().ToString();
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "guestCartId", guestId);
                }
                // مثال: "shoppingCart_a1b2c3d4-e5f6-..."
                return $"shoppingCart_{guestId}";
            }
        }

        // --- الخطوة 4: تعديل كل الدوال لتستخدم المفتاح الذكي ---
        // كل الدوال التالية تم تعديلها لتستدعي GetCartStorageKeyAsync() أولاً.

        public async Task AddToCart(Product product, int quantity = 1)
        {
            var storageKey = await GetCartStorageKeyAsync(); // <-- تعديل
            var cart = await GetCartItems();
            var cartItem = cart.FirstOrDefault(ci => ci.ProductId == product.ProductID);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = product.ProductID,
                    Name = product.Name,
                    Price = product.Price,
                    ImagePath = product.ImagePath,
                    Quantity = quantity,
                    MaxAvailableStock = product.StockQuantity
                };
                cart.Add(cartItem);
            }
            await SaveCart(cart);
        }

        private async Task SaveCart(List<CartItem> cart)
        {
            var storageKey = await GetCartStorageKeyAsync(); // <-- تعديل
            var cartJson = JsonSerializer.Serialize(cart);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", storageKey, cartJson);
            if (CartChanged != null)
            {
                await CartChanged.Invoke();
            }
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            var storageKey = await GetCartStorageKeyAsync(); // <-- تعديل
            var cartJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", storageKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }
            return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        public async Task<int> GetCartItemCount()
        {
            var cart = await GetCartItems();
            return cart.Sum(item => item.Quantity);
        }

        public async Task UpdateItemQuantity(int productId, int newQuantity)
        {
            var cart = await GetCartItems();
            var itemToUpdate = cart.FirstOrDefault(item => item.ProductId == productId);
            if (itemToUpdate != null)
            {
                if (newQuantity > 0 && newQuantity <= itemToUpdate.MaxAvailableStock)
                {
                    itemToUpdate.Quantity = newQuantity;
                    await SaveCart(cart);
                }
            }
        }

        public async Task RemoveFromCart(int productId)
        {
            var cart = await GetCartItems();
            var itemToRemove = cart.FirstOrDefault(item => item.ProductId == productId);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                await SaveCart(cart);
            }
        }

        public async Task ClearCart()
        {
            var storageKey = await GetCartStorageKeyAsync(); // <-- تعديل
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", storageKey);
            if (CartChanged != null)
            {
                await CartChanged.Invoke();
            }
        }
    }
}
