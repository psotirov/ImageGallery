/// <reference path="class.js" />
/// <reference path="persister.js" />
/// <reference path="jquery-2.0.2.js" />
/// <reference path="ui.js" />

var controllers = (function () {
    var rootUrl = "http://maikati.apphb.com/api/";

	var Controller = Class.create({
		init: function () {
			this.persister = persisters.get(rootUrl);
		},
		loadUI: function (selector) {
			if (this.persister.isUserLoggedIn()) {
				this.loadGalleryUI(selector);
			}
			else {
				this.loadLoginFormUI(selector);
			}
			this.attachUIEventHandlers(selector);
		},
		loadLoginFormUI: function (selector) {
			var loginFormHtml = ui.loginForm()
			$(selector).html(loginFormHtml);
		},
		loadGalleryUI: function (selector) {

		    this.persister.user.getUsers(function (user) {
		        var list = ui.GalleryUI(user);
		        $(selector).html(list);
		        $("#sortable").sortable();
		        $("#sortable").disableSelection();
		    }, function () { alert("error, not found albums") });
		},
		loadGalleriesUI: function (userId, selector) {
		    this.persister.user.getGalleries(userId, function (user) {
		        var list = ui.GaleriesUI(user.Galleries);
		        console.log(user);
		        if(user.SessionKey == localStorage.getItem("sessionKey")){
		            list += '<a href="#" class="btn-create-album">create album</a>' +
		                '<input type="type" id="input-title-album" name="name" value=" " />';
		        }

		        $(selector).html(list);
		        $("#sortable-gallery").sortable();
		        $("#sortable-gallery").disableSelection();
		 

		    }, function () { alert("error, not found albums")});
		},
		loadAlbumsUI: function (albumId, selector, success) {
		    this.persister.album.getAlbums(albumId, function (albums) {
		        var list = ui.GaleriesUI(albums[0].Albums);

		        //console.log(albums);
		        //if (albums.SessionKey == localStorage.getItem("sessionKey")) {
		        //    list += '<a href="#">create album</a>';
		        //}
		        success();
		        $(selector).html(list);
		    }, function () { alert("error, not found albums") });
		},
		loadImagesUI: function (albumId, selector) {

		    //alert("render images");
		    this.persister.album.getAlbums(albumId, function (albums) {
		        //console.log(albums);
		        var list = ui.BuildImagesList(albums[0].Images);
		        //console.log(list);
		        $(selector).append($(list));

		        $("#sortable-image").sortable();
		        $("#sortable-image").disableSelection();		      
		    }, function () { alert("error, not found images") });
		},
		loadImageUI: function (imageId, selector) {
		    //alert("render images");
		    this.persister.image.getImageById(imageId, function (image) {
		        console.log(image);
		        //alert("Stignah");
		        var list = ui.buildImageInfo(image);
		        //console.log(list);
		        $(selector).html(list);

		    }, function () { alert("error, not found image") });
		},

		attachUIEventHandlers: function (selector) {
			var wrapper = $(selector);
			var self = this;
			var selectedUnit = {};

			wrapper.on("click", "#btn-login", function () {
				var user = {
					username: $(selector + " #tb-login-username").val(),
					password: $(selector + " #tb-login-password").val()
				}

				self.persister.user.login(user, function () {
					self.loadGalleryUI(selector);
				}, function () {
					wrapper.html("oh no..");
				});


				return false;
			});
			wrapper.on("click", "#btn-register", function () {
			    var user = {
			        username: $(selector + " #tb-login-username").val(),
			        password: $(selector + " #tb-login-password").val()
			    }

			    self.persister.user.register(user, function () {
			        self.loadGalleryUI(selector);
			    }, function () {
			        wrapper.html("oh no..");
			    });
			    return false;
			});
			wrapper.on("click", "#btn-logout", function () {
			    //alert("logout");
				//self.persister.user.logout(function () {
				//	self.loadLoginFormUI(selector);
			    //});
			    localStorage.removeItem("nickname");
			    localStorage.removeItem("sessionKey");
			    nickname = "";
			    sessionKey = "";
			    self.loadLoginFormUI(selector);
			});
			wrapper.on("click", ".btn-create-album", function () {
			    alert("Soon...");
			    //var album = {
			    //    title: $("#input-title-album").val(),
			    //    AlbumId: "1",
                //    userId: null
			    //}

			    //self.persister.album.create(album, function () {
			    //    self.loadGalleryUI(selector);
			    //}, function () {
			    //    wrapper.html("oh no..");
			    //});
			    return false;
			});
			wrapper.on("click", ".image", function () {
			    //alert("Image...");
			    var imageId = $(this).parent().data("image-id");
			    //alert(imageId);
			    self.loadImageUI(imageId, "#galleries");
			    return false;
			});
			wrapper.on("click", ".users", function () {
			    //console.log(target);
			    var userId = $(this).parent().data("user-id");
			    //alert($(this).parent().data("user-id"));
			    self.loadGalleriesUI(userId, "#galleries");
			});
			wrapper.on("click", ".albums", function () {
			    //console.log(target);
			    var albumId = $(this).parent().data("gallery-id");
			    self.loadAlbumsUI(albumId, "#galleries", function () {
			        self.loadImagesUI(albumId, "#galleries");
			    });
			});
		}
	});
	return {
		get: function () {
			return new Controller();
		}
	}
}());

$(function () {
	var controller = controllers.get();
	controller.loadUI("#content");
    // Pubnub - the logged in user subscribes to listen to his/her private channel for notifications
	var channel = "-channel";//user.username + "-channel";

	PUBNUB.subscribe({
	    channel: channel,
	    callback: function (message) {
	        //alert(message);
	        // Received a message --> print it in the page
	        //document.write("<p>" + message + "</p>");
	        var html = '<p>' + message + '</p>';
	        var messages = $("#message-container").append(html);
	    }
	});
});