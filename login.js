var app = angular.module("foodApp", []);

app.controller("LoginController", function ($scope, $http) {

    var baseUrl = "http://localhost:5077/api/auth";

    $scope.step = 1;
    $scope.user = {};
    $scope.error = "";

    // SEND OTP
    $scope.sendOtp = function () {

        if (!$scope.user.name || !$scope.user.mobile) {
            $scope.error = "Enter name and mobile number";
            return;
        }

        $http.post(baseUrl + "/send-otp", $scope.user)
            .then(function (res) {

                $scope.step = 2;
                $scope.error = "";

                // show OTP
                setTimeout(() => {
                    alert("Your OTP is: " + res.data.otp);
                }, 100);
            })
            .catch(function () {
                $scope.error = "Failed to send OTP";
            });
    };

    // VERIFY OTP
    $scope.verifyOtp = function () {

        console.log("Entered OTP:", $scope.otp);

        if (!$scope.otp || $scope.otp.toString().trim() === "") {
            $scope.error = "Enter OTP";
            return;
        }

        $http.post(baseUrl + "/verify-otp", {
            mobile: $scope.user.mobile,
            otp: $scope.otp.toString().trim()
        })
        .then(function (res) {

            localStorage.setItem("loggedIn", "true");
            localStorage.setItem("username", res.data.name);

            window.location.href = "index.html";
        })
        .catch(function () {
            $scope.error = "Invalid OTP";
        });
    };

});