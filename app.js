var app = angular.module("foodApp", []);

app.controller("FoodController", function ($scope, $http) {

    var productApi = "http://localhost:5077/api/products";
    var orderApi = "http://localhost:5077/api/orders";

    $scope.products = [];
    $scope.cart = [];
    $scope.newProduct = {};

    // 👤 USERNAME
    $scope.username = localStorage.getItem("username");

    // 🔓 LOGOUT
    $scope.logout = function () {
        localStorage.removeItem("loggedIn");
        localStorage.removeItem("username");
        window.location.href = "login.html";
    };

    // LOAD PRODUCTS
    $scope.loadProducts = function () {
        $http.get(productApi).then(function (res) {
            $scope.products = res.data;
        });
    };

    // ADD PRODUCT
    $scope.addProduct = function () {
        $http.post(productApi, $scope.newProduct).then(function () {
            $scope.loadProducts();
            $scope.newProduct = {};
        });
    };

    // EDIT PRODUCT
    $scope.editProduct = function (product) {
        $scope.newProduct = {
            id: product.id || product.Id,
            name: product.name || product.Name,
            price: product.price || product.Price,
            imageUrl: product.imageUrl || product.ImageUrl
        };
    };

    // UPDATE PRODUCT
    $scope.updateProduct = function () {

        var id = $scope.newProduct.id;

        if (!id) {
            alert("Select a product to edit first!");
            return;
        }

        $http.put(productApi + "/" + id, $scope.newProduct)
            .then(function () {
                $scope.loadProducts();
                $scope.newProduct = {};
            });
    };

    // CART
    $scope.addToCart = function (product) {
        $scope.cart.push(product);
    };

    $scope.removeFromCart = function (index) {
        $scope.cart.splice(index, 1);
    };

    // TOTAL
    $scope.getTotal = function () {
        var total = 0;
        $scope.cart.forEach(item => {
            total += item.price;
        });
        return total;
    };

    // CHECKOUT
    $scope.checkout = function () {

        if ($scope.cart.length === 0) {
            alert("Cart is empty!");
            return;
        }

        var order = {
            totalAmount: $scope.getTotal(),
            orderDate: new Date()
        };

        $http.post(orderApi, order).then(function () {
            alert("Order placed!");
            $scope.cart = [];
        });
    };

    $scope.loadProducts();
});