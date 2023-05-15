let map;

function initMap() {

    var longitude = localStorage.getItem("Longitude");
    var latitude = localStorage.getItem("Latitude");

    if (longitude === null && latitude === null) {
        map = new google.maps.Map(document.getElementById("map"), {
            center: { lat: -34.397, lng: 150.644 },
            zoom: 8,
        });
    }
    else {
        map = new google.maps.Map(document.getElementById("map"), {
            center: { lat: Number(latitude), lng: Number(longitude) },
            zoom: 12,
        });
        localStorage.removeItem("Latitude");
        localStorage.removeItem("Longitude");
    }

    

    
    axios.get('/Map/GetLocations')
        .then(function (response) {
            for (var i = 0; i < response.data.length; i++) {
                 createMarker(response.data[i]);
            }
        })
        .catch(function (error) {
            console.log(error);
        });

    function createMarker(location) {
        var link = 'Map/LocationView?name=' + location.name;

        var marker = new google.maps.Marker({
            map: map,
            position: { lat: location.latitude, lng: location.longitude },
            title: location.name,
        });

        marker.addListener('click', function () {
            var contentString = `<div><p>Name:<a href='${link}'> ${location.name}</a></p>`
                + `<p>Address: ${location.address}</p>`
                + `<p>Description: ${location.description}</p></div>`;

            var infowindow = new google.maps.InfoWindow({
                content: contentString
            });

            infowindow.open(map, marker);
        });
    }
}

function moveToLabel(response) {
    if (response.data !== null && response.data !== undefined) {
        map.setCenter({ lat: response.data.latitude, lng: response.data.longitude });
        map.setZoom(12);
    }
}


window.initMap = initMap;


