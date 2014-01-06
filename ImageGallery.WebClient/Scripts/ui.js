var ui = (function () {

	function buildLoginForm() {
		var html =
            '<div id="login-form-holder">' +
				'<form>' +
					'<div id="login-form">' +
						'<label for="tb-login-username">Username: </label>' +
						'<input type="text" id="tb-login-username"><br />' +
						'<label for="tb-login-password">Password: </label>' +
						'<input type="password" id="tb-login-password"><br />' +
						'<button id="btn-login" class="button">Login</button>' +
						'<button id="btn-register" class="button">Register</button>' +
					'</div>' +
				'</form>' +
            '</div>';
		return html;
	}

	function buildGalleryUI(user) {
	    var html = '<span id="user-nickname">' +
				localStorage.getItem("nickname") +
		'</span>' +
		'<button id="btn-logout">Logout</button><br/>' +
		'<div id="users-container">' +
			'<h2>Users</h2>' +
			'<div id="users">' + buildUsersList(user) +
	    '</div></div>' +
		'<div id="galleries-container">' +
			'<h2>Galleries</h2>' +
			'<div id="galleries">' +
            '</div>' +
		'</div>';

		return html;
	}

	function buildGalleriesList(galleries) {
	    var list = '<div id="albums-container"><ul class="galleries-list" id="sortable-gallery">';
	    for (var i = 0; i < galleries.length; i++) {
	        var gallery = galleries[i];
			list +=
				'<li data-gallery-id="' + gallery.AlbumId + '" class="ui-state-default">' +
						'<h3>' + $("<div />").html(gallery.Title).text() + '</h3>' +
					'<a href="#" class="albums">' +
                        '<img src="images/folderColored.png" alt="Alternate Text" />' +
					'</a>' +
				'</li>';
		}
		list += "</ul></div>";
		return list;
	}

	function buildImagesList(images) {
	    var list = '<div id="images-container"><ul class="images-list" id="sortable-image">';
	    //console.log(images.length);
	    for (var i = 0; i < images.length; i++) {
	        var image = images[i];
	        list +=
				'<li data-image-id="' + image.ImageId + '"class="ui-state-default"> ' +
						'<h4>' + $("<div />").html(image.Title).text() + '</h4>' +
					'<a href="#" class="image">' +
                        '<img src="images/photo.png" alt="Alternate Text" />' +
					'</a>' +
				'</li>';
	    }
	    list += "</ul></div>";
	    return list;
	}

	function buildImageInfo(image) {
	    var list = '<h2>' + image.Title + '</h2>' +
            '<img src="' + image.Url + '" alt="Alternate Text" id="big-image" />' +
	        '<ul>';
	        
        for (var i = 0; i < image.Comments.length; i++) {
            list += '<li>' + image.Comments[i].Content + '</li>';
            }

        list+='</ul>';
	    return list;
	}

	function buildUsersList(users) {
	    var list = '<ul class="users-list"  id="sortable">';
		for (var i = 0; i < users.length; i++) {
			//var game = gamesList[i];
			list +=
				'<li data-user-id="' + users[i].UserId + '" class="ui-state-default">' +
					'<a href="#" class="users">' + //users[i].username +
						$("<div />").html(users[i].Username).text() +
					'</a>' + 
				'</li>';
		}
		list += "</ul>";
		return list;
	}

	function buildMessagesBox(messages) {
	    var html = "";

	    //console.log(messages);
	    for (var i = 0; i < messages.length; i++) {
	        html += '<p>' +
                'MESSAGE: ' + messages[i].text +
                '</p>';

	        //console.log(messages[i].text);
	    }

	    return html;
	}

	return {
		GalleryUI: buildGalleryUI,
		GaleriesUI: buildGalleriesList,
		loginForm: buildLoginForm,
		usersList: buildUsersList,
		BuildImagesList: buildImagesList,
		buildImageInfo: buildImageInfo
	}

}());