angular.module('kanban', ['ngResource']);

angular.module('kanban').value('apiUrl', 'http://localhost:51586/api')


angular.module('kanban').controller('IndexController', function ($scope, $resource, apiUrl) {
    var CardResource = $resource(apiUrl + '/cards/:CardId', { CardId: '@CardId' },
    {
        'save': {method: 'POST'}
    }
    );

var ListResource = $resource(apiUrl + '/lists/:listId', {listId: '@ListId' },
    {
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
    $scope.newCard = {};

    $scope.addList = function () {
        ListResource.save($scope.newList, function () {
            
            activate();
        });
    };

    $scope.deleteList = function (list) {
        list.$remove(function () {           
            activate();
        })
    };
    
    $scope.addCard = function (list) {
        $scope.newCard.ListID = list.ListId;
        CardResource.save($scope.newCard, function () {

            activate();
        });
    };


    $scope.deleteCard = function (card) {
        CardResource.delete(card, function () {
            activate();
        });
    };


    activate(); 

});