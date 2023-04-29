let map;

function initMap() {
    axios.get('/Map/GetLocations')
    .then(function (response) {
        for (var i = 0; i < response.length; i++) {
            map = new google.maps.Map(document.getElementById("map"), {
                center: { lat: response[i].lat, lng: response[i].lat },
                zoom: 5,
            });
        }
    })
    .catch(function (error) {
        console.log(error);
    });

    
}

window.initMap = initMap;


