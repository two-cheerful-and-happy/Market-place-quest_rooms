function searchLocation() {
    const url = '../Map/Search?name='
    var searchValue = document.getElementById('searchInput').value;
    document.getElementById('searchInput').value = '';
    axios.get(url + searchValue)
        .then(function (response) {
            if (window.location.pathname == '/') {
                moveToLabel(response);
            }
            else {
                localStorage.setItem("Latitude", response.data.latitude);
                localStorage.setItem("Longitude", response.data.longitude);
                window.location.href = '/';
            }
            
        })
        .catch(function (error) {
            console.log(error);
        });
    console.log(searchValue);
}       