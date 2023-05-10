let map;
let latitude = document.getElementById("Latitude");
let longitude = document.getElementById("Longitude");
let marker;

function initMap() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: -34.397, lng: 150.644 },
        zoom: 8,
    });
    map.addListener("click", function (event) {
        
            // Удаляем предыдущую метку, если она есть
            if (marker) {
                marker.setMap(null);
            }

            marker = new google.maps.Marker({
                position: event.latLng,
                map: map,
            });

            latitude.value = event.latLng.lat();
            longitude.value = event.latLng.lng();
        
    });
}

function reloadMap() {
    latitude.value = '';
    longitude.value = '';
    if (marker) {
        marker.setMap(null);
    }
}

window.initMap = initMap;


