//Home Controller

angular.module('myModule').controller("homeController", ['$scope', '$http', function ($scope, $http) {
    $scope.getUserDetails = function () {
        $scope.itemsPerPage = 3;
        $scope.currentPage = 0;
        $http.get('/api/userDetails/')
            .then(function (response) {
                $scope.userDetails = response.data
            });
    };

    $scope.getUserDetails();

    $scope.addUser = function (userName) {
        if (userName.length != 0) {
            $http({
                method: 'POST',
                url: '/api/users',
                data: JSON.stringify(userName),
                headers: {
                    "Content-Type": "application/json;charset=UTF-8"
                }
            }).then(function (result) {
                $scope.getUserDetails();
                console.log(result);
            }, function (error) {
                console.log(error);
            });
        }
        else {
            alert("Please enter a valid name!");
        }


    };

    $scope.deleteUser = function (userId) {
        $http({
            method: 'DELETE',
            url: '/api/users/' + userId,
            headers: {
                "Content-Type": "application/json;charset=UTF-8"
            }
        }).then(function (result) {
            $scope.getUserDetails();
            console.log(result);
        }, function (error) {
            console.log(error);
        });
    }
    //$scope.refresh = setInterval(function () {
    //    $scope.getUserDetails();
    //}, 3000);


    //For sorting based on column header sort arrow
    $scope.sortColumn = "name";
    $scope.reverseSort = false;

    $scope.sortData = function (column) {
        $scope.reverseSort = ($scope.sortColumn == column) ? !$scope.reverseSort : false;
        $scope.sortColumn = column;
    }

    $scope.getSortClass = function (column) {
        if ($scope.sortColumn == column) {
            return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
        }
        return '';
    }

    //Pagination
    $scope.range = function () {
        var rangeSize = 4;
        var ps = [];
        var start;

        start = $scope.currentPage;
        //  console.log($scope.pageCount(),$scope.currentPage)
        if (start > $scope.pageCount() - rangeSize) {
            start = $scope.pageCount() - rangeSize + 1;
        }

        for (var i = start; i < start + rangeSize; i++) {
            if (i >= 0)
                ps.push(i);
        }
        return ps;
    };

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
        }
    };

    $scope.DisablePrevPage = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.pageCount = function () {
        return Math.ceil($scope.userDetails.length / $scope.itemsPerPage) - 1;
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount()) {
            $scope.currentPage++;
        }
    };

    $scope.DisableNextPage = function () {
        return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
    };

    $scope.setPage = function (n) {
        $scope.currentPage = n;
    };
}])