'use strict';

angular.module('ToDoDemo')

.controller('ToDoDemoController', function( $scope, $http ) {

    var url = 'api/todo';

    // Add a new task
    $scope.addTask = function() {

        console.log("New task name is ", $scope.newTask);

        var newTask = { description: $scope.newTask.name, isComplete: $scope.newTask.isComplete };

        var taskAsString = JSON.stringify(newTask);
        console.log("Stringified is ", newTask);

        $http({
            method: 'POST',
            url: 'api/todo',
            data: taskAsString,
            headers: {
                'Content-Type': 'application/json'
            }}).then(function(result) {
                getToDoItems();
                newTask.name = "";
                newTask.isComplete = false;

            }, function(error) {
                console.log("Failure: ", error);
            });

            }




    function getToDoItems() {
        $http.get(url).then(
            function(success) {
                console.log("Received To Do data: ", success);

                $scope.tasks = success.data; 
            },
            function(failure) {
                console.log("Error querying ToDo list");
            })};



    // Initialisation
            $scope.tasks = [];
            $scope.newTask = {};
            $scope.newTask.name = "";
            $scope.newTask.isComplete = false;

    getToDoItems();

});

