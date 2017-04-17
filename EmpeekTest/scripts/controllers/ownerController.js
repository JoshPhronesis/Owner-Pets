//ownerController

angular.module('myModule').controller("ownerPetsController", ['$scope', '$http', '$routeParams', function ($scope, $http, $routeParams) {

    $scope.getUserPets = function () {
        $scope.itemsPerPage = 3;
        $scope.currentPage = 0;
        $http({
            url: "/api/userpets",
            params: { id: $routeParams.id },
            method: "GET",
            headers: {
                "Content-Type": "application/json;charset=UTF-8"
            }

        }).then(function (response) {
            $scope.userPets = response.data;
            $scope.userPets.petsCount = $scope.userPets.length;
            if ($scope.userPets.length == 0) {
                $http({
                    url: "/api/users",
                    params: { id: $routeParams.id },
                    method: "GET",
                    headers: {
                        "Content-Type": "application/json;charset=UTF-8"
                    }
                }).then(function (response) {
                    $scope.userPets = [];
                    $scope.userPets.petsCount = 0;
                    $scope.userPets.push(response.data)
                    console.log(response);
                }, function (error) {
                    console.log(error);
                });
            }
            console.log(response);
        }, function (error) {
            console.log(error);
        });
    }
    $scope.getUserPets();

    $scope.addPet = function (petName) {
        $http({
            method: 'POST',
            url: '/api/pets',
            data: JSON.stringify(petName),
            headers: {
                "Content-Type": "application/json;charset=UTF-8"
            }

        }).then(function (result) {
            console.log(result);
        }, function (error) {
            console.log(error);
        });
    };

    $scope.updateUserPet = function (userId, petName) {
        if (petName.length !=0) {
            $scope.addPet(petName);
            var data = {
                petName: petName
            }
            $http({
                url: '/api/userpets/' + userId,
                method: "PUT",
                data: JSON.stringify(data),
                headers: {
                    "Content-Type": "application/json;charset=UTF-8"
                }
            })
                .then(function (result) {
                    $scope.getUserPets();
                }, function (error) {
                    console.log(error);
                });
        }
        else {
            alert("Please enter a valid name!");
        }

    }

    $scope.deleteUserPet = function (userId, petId) {
        var data = {
            petId: petId
        }
        $http({
            method: 'DELETE',
            url: '/api/userpets/' + userId,
            data: JSON.stringify(data),
            headers: {
                "Content-Type": "application/json;charset=UTF-8"
            }
        }).then(function (result) {
            $scope.getUserPets();
            console.log(result);
        }, function (error) {
            console.log(error);
        });
    }

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
        return Math.ceil($scope.userPets.length / $scope.itemsPerPage) - 1;
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