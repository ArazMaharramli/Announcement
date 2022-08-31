let infoWindow, marker, autocomplete;
function initMap() {
    var loc = { lat: 40, lng: 49 }
    getUserLocation();
    // The map, centered at user location
    map = new google.maps.Map(document.getElementById("map"), {
        zoom: 15,
        center: loc,
        gestureHandling: "greedy",
        zoomControl: false,

    });
    map.addListener("click", (mapsMouseEvent) => {
        setMarker(mapsMouseEvent.latLng, false);
    });
    marker = new google.maps.Marker({
        position: loc,
        map: map,
    });
    initSearchBox();
}

function setMarker(location, setcenter = true) {
    if (setcenter) {
        map.setCenter(location);
    }
    if (marker != null) {
        marker.setPosition(location);
        $('#longtitudeInput').val(location.lng);
        $('#latitudeInput').val(location.lat);
    }
}

async function getUserLocationFromGPS() {
    navigator.geolocation.getCurrentPosition(
        (position) => {
            pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude,
            };
            setMarker(pos);
            // console.log(position);
        },
        (err) => {
        }
    );
}
async function getUserLocation() {

    var loc = await $.ajax({
        url: "https://www.googleapis.com/geolocation/v1/geolocate?key=AIzaSyCdVSHqtbLoxPRa5o35dyisKmSfHHgM0UI",
        type: 'POST'
    });
    setMarker(loc.location);

}
function initSearchBox() {
    var input = document.getElementById("addressinput");
    // Create the search box and link it to the UI element.
    // const input = document.getElementById("pac-input");
    const searchBox = new google.maps.places.SearchBox(input);
    map.controls.push(input);
    // Bias the SearchBox results towards current map's viewport.
    map.addListener("bounds_changed", () => {
        searchBox.setBounds(map.getBounds());
    });
    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    searchBox.addListener("places_changed", () => {
        const places = searchBox.getPlaces();
        if (places.length == 0) {
            return;
        }
        // For each place, get the icon, name and location.
        const bounds = new google.maps.LatLngBounds();
        places.forEach((place) => {
            if (!place.geometry || !place.geometry.location) {
                // console.log("Returned place contains no geometry");
                return;
            }
            setMarker(place.geometry.location)
            // console.log(place);
            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
    });
}
function fillInAddress() {
    const place = autocomplete.getPlace();
    // console.log(place);
}
