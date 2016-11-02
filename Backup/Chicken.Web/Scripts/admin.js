Array.prototype.contains = function (obj) {
    var i = this.length;
    while (i--) {
        if (this[i] === obj) {
            return true;
        }
    }
    return false;
};

angular.module('app', [])
  .controller('AdminController', ['$scope', '$http', function ($scope, $http) {
      $scope.totalCount = 0;
      $scope.spamCount = 0;
      $scope.posts = [];
      $scope.modifiedPosts = [];
      $scope.isLoading = false;
      $scope.isUpdating = false;
      $scope.groupId = 1;

      $scope.init = function (totalCount, spamCount, groupId, groups) {
          $scope.totalCount = totalCount;
          $scope.spamCount = spamCount;
          $scope.groups = groups;
          $scope.groupId = groupId;
          $scope.getPosts();
      };

      $scope.getPosts = function () {
          if (!$scope.isLoading) {
              $scope.isLoading = true;
              $http.get('/admin/getposts?' +
                  'skip=' + $scope.posts.length +
                  '&groupId=' + $scope.groupId).success(function (data) {
                      for (var i = 0; i < data.length; i++) {
                          $scope.posts.push(data[i]);
                      }
                      $scope.isLoading = false;
                  });
          }
      };

      $scope.changeGroup = function() {
          location.href = '/admin?groupId=' + $scope.groupId;
      };

      $scope.changePost = function (el) {
          if (!$scope.modifiedPosts.contains(el)) {
              $scope.modifiedPosts.push(el);
          }
      };

      $scope.save = function () {
          var modifiedPosts = $scope.modifiedPosts;
          $http.post('/admin/save', modifiedPosts).success(function () {
              alert('Изменения сохранены.');
          });
      };

      $scope.loadNewPosts = function () {
          $scope.isUpdating = true;
          $http.get('/admin/update?groupId=' + $scope.groupId).success(function (data) {
              for (var i = 0; i < data.length; i++) {
                  $scope.posts.unshift(data[i]);
              }
              $scope.isUpdating = false;
          });
      };
  }]);
