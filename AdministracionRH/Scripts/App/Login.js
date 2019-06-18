function onSignIn(googleUser) {
    // Useful data for your client-side scripts:
    //signOut();
    var profile = googleUser.getBasicProfile();
    // The ID token you need to pass to your backend:
    var id_token = googleUser.getAuthResponse().id_token;
    var email = profile.getEmail();
    var imageUrl = profile.getImageUrl();
    var name = profile.getName();
    //alert(profile.getImageUrl());

    $.ajax({
        url: "/Login/Index",
        type: 'POST',
        data: { email: email, imageUrl: imageUrl },
        success: function (result) {

            if (result.success) {
                
                 // window.location.href = 'http://localhost:83/Home/Index';
                  //window.location.href = 'http://apps.adamantine.com.mx/Rh/Home/Index';
                window.location.href = result.urlRedirect;
                

            }
            else {
                signOut();
                alert(result.mensaje);
            }
        },
        error: function (error) {
            alert("error" + JSON.stringify(error));
        }
    });


};


function signOut() {
    var auth2 = gapi.auth2.getAuthInstance();
    auth2.signOut().then(function () {
        console.log('User signed out.');
        //window.location.href = 'http://apps.adamantine.com.mx/Rh/Login/Index';
        window.location.href = 'http://localhost:52002/Login/Index';
    });
    auth2.disconnect();
}

