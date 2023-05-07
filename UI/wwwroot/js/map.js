let map;

function initMap() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: -34.397, lng: 150.644 },
        zoom: 8,
    });
    //axios.get('/Map/GetLocations')
    //.then(function (response) {     
    //    for (var i = 0; i < response.data.length; i++) {
    //        const markerViewWithText = new google.maps.Marker({
    //            map,
    //            position: { lat: response.data[i].latitude, lng: response.data[i].longitude },
    //            title: "Title text for the marker at lat: 37.419, lng: -122.03",
    //        });
    //    }
    //})
    //.catch(function (error) {
    //    console.log(error);
    //});
}

window.initMap = initMap;


