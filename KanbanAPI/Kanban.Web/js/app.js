angular.module('kanban', ['ngResource']);

angular.module('kanban').value('apiUrl', 'http://localhost:51586/api')


angular.module('kanban').controller('IndexController', function ($scope, $resource, apiUrl) {
    var ListResource = $resource(apiUrl + '/lists/:listId', { listId: '@id' }, {
        'cards': {
            url: apiUrl + '/lists/:listId/cards',
            method: 'GET',
            isArray: true
        }
    });

    function activate() {
        ListResource.query(function (data) {
            $scope.lists = data;

            $scope.lists.forEach(function (list) {
                list.cards = ListResource.cards({ listId: list.ListId });
            });
        });
    }
 


    $scope.newList = {};

    $scope.addList = function () {
        ListResource.save($scope.newList, function () {
            
            activate();
        });
    };

    //$scope.deleteList = function () {

    //    ListResource.remove($scope.currentList, function () {

    //        activate();
    //    });
    //};

    //CARDS




    activate(); 

});