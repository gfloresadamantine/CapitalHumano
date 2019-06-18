function signOut() {
    var auth2 = gapi.auth2.getAuthInstance();
    auth2.signOut().then(function () {
        console.log('User signed out.');
        //window.location.href = 'http://apps.adamantine.com.mx/Rh/Login/Index';

        window.location.href = 'http://localhost:52002/Login/Index';
        //window.location.href = 'http://localhost:83/Login/Index';
    });
    auth2.disconnect();
}

function onLoad() {
    gapi.load('auth2', function () {
        gapi.auth2.init();
    });
}



